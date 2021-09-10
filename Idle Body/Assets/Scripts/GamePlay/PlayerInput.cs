using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    protected GameManager myGameManager;
    public void CustomStart()
    {
        myGameManager = GameManager.gameManager;
        
    }

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



}
