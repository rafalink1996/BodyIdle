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
        if (myOrganManager.activeOrganType < 12)
        {
            if (myOrganManager.organTypes[myOrganManager.activeOrganType].organs.Count < 5) // chequear que hayan menos de 5 organos de ese tipo
            {
                int OrganCount = myOrganManager.organTypes[myOrganManager.activeOrganType].organs.Count;
                if (myGameManager.pointsManager.totalPoints >= myOrganManager.organTypes[myOrganManager.activeOrganType].PointCost[OrganCount]) // chequear que el jugador tiene los puntos necesarios
                {
                    if (myGameManager.pointsManager.ComplexityPoints + myOrganManager.organTypes[myOrganManager.activeOrganType].ComplexityCost[OrganCount] <= myGameManager.pointsManager.ComplexityMaxPoints) // chequear que el jugador no sobrepase la cantidad de complexity
                    {
                        //Debug.Log("Organ Can be bought");
                        myGameManager.pointsManager.ManagePoints(-myOrganManager.organTypes[myOrganManager.activeOrganType].PointCost[OrganCount]);
                        myOrganManager.AddNewOrgan(myOrganManager.activeOrganType);
                        myGameManager.OrganViewUI.UpdateOrgans();
                    }
                    else
                    {
                        Debug.Log("Not enough compelxity");
                        Debug.Log("Current Complexity = " + myGameManager.pointsManager.ComplexityPoints + "Max Complexity = " + myGameManager.pointsManager.ComplexityMaxPoints);
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
        else
        {
            Debug.Log("Choosing organ");
        }

    }
    public void OnClickOrgan(int id)
    {
        myGameManager.organManager.AsignNewOrganID(id, out bool canChangeOrgan);
        if (canChangeOrgan)
        {
            Debug.Log("canchangeOrgan");
            myGameManager.topUIManager.ChangeView(0);
            myGameManager.CellViewUI.UpdateBackground(myOrganManager.activeOrganType);
            myGameManager.OrganViewUI.ToggleOrgansButtons(false);
        }
    }

    public void OnClickNewOrgan(int organType)
    {
        if (myGameManager.pointsManager.totalPoints >= myOrganManager.organTypes[organType].PointCost[0])
        {
            Debug.Log("Total points = " + myGameManager.pointsManager.totalPoints);
            Debug.Log("cost = " + myOrganManager.organTypes[organType].ComplexityCost[0]);
            int complexityGain = myGameManager.pointsManager.ComplexityPoints + Mathf.FloorToInt(myOrganManager.organTypes[organType].ComplexityCost[0]);
            if (complexityGain <= myGameManager.pointsManager.ComplexityMaxPoints)
            {
                Debug.Log("created new organ");
                myGameManager.pointsManager.ManagePoints(-myOrganManager.organTypes[organType].ComplexityCost[0]);
                myOrganManager.organTypes[organType].unlocked = true;
                myOrganManager.AddNewOrgan(organType);
                myGameManager.OrganViewUI.newOrgan(organType);

            }
            else
            {
                Debug.Log("not enough complexity available");
            }
        }else
            {
            Debug.Log("not enough energy");
            }
       
    }


    public void OnClickBuyPlatelet()
    {
        myGameManager.OrganViewUI.BuyPlatelet();
    }
    public void OnClickUpgradeMultiplier()
    {

    }

    #endregion Organ View Interactions
}
