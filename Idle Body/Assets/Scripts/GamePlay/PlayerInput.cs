using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    protected GameManager myGameManager;
    protected OrganManager myOrganManager;
    public void CustomStart()
    {
        myGameManager = GameManager.gameManager;
        if (myGameManager != null)
            myOrganManager = myGameManager.organManager;
    }

    #region Cell View Interactions
    public void OnClickBuyCell()
    {
        int CellID = myGameManager.CellViewUI.CurrentCellType;
        myGameManager.organManager.cellSpawner.BuyCell(out bool Bought, CellID);
        if (Bought)
        {
            myGameManager.CellViewUI.StartBuyCell();
        }
    }
    public void OnClickChangeCellType(int cellType)
    {
        myGameManager.CellViewUI.StartChangeCell(cellType);
    }
    public void OnClickTab()
    {
        myGameManager.CellViewUI.StartCellTab();
    }
    public void OnClickBackToOrgan()
    {
        myGameManager.topUIManager.ChangeView(1);
        
    }
    #endregion Cell View Interactions
    #region Organ View Interactions
    public void OnClickBuyOrgan()
    {
        if(myOrganManager.organTypes[myOrganManager.activeOranType].organs.Count < 5) // chequear que hayan menos de 5 organos de ese tipo
        {
            int OrganCount = myOrganManager.organTypes[myOrganManager.activeOranType].organs.Count;
            if (myGameManager.pointsManager.totalPoints >= myOrganManager.organTypes[myOrganManager.activeOranType].PointCost[OrganCount]) // chequear que el jugador tiene los puntos necesarios
            {
                if(myGameManager.pointsManager.ComplexityPoints + myOrganManager.organTypes[myOrganManager.activeOranType].ComplexityCost[OrganCount] <= myGameManager.pointsManager.ComplexityMaxPoints) // chequear que el jugador no sobrepase la cantidad de complexity
                {
                    Debug.Log("Organ Can be bought");
                    myGameManager.pointsManager.ManagePoints(-myOrganManager.organTypes[myOrganManager.activeOranType].PointCost[OrganCount]);
                    myOrganManager.AddNewOrgan(myOrganManager.activeOranType);
                    myGameManager.OrganViewUI.ShowOrgans();
                }
                else
                {
                    Debug.Log("Not enough compelxity");
                    Debug.Log("Current Complexity = " +  myGameManager.pointsManager.ComplexityPoints + "Max Complexity = " + myGameManager.pointsManager.ComplexityMaxPoints);
                }
            }
            else
            {
                Debug.Log("Not Enough Points");
            }
        }
        else
        {
            Debug.Log("Organ Max");
        }

    }
    public void OnClickOrgan(int id)
    {
        myGameManager.organManager.AsignNewOrganID(id, out bool canChangeOrgan);
        if (canChangeOrgan)
        {
            Debug.Log("canchangeOrgan");
            myGameManager.topUIManager.ChangeView(0);
            myGameManager.OrganViewUI.ToggleOrgansButtons(false);
        }
    }

    #endregion Organ View Interactions
}
