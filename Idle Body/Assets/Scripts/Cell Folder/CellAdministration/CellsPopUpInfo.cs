using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CellsPopUpInfo : MonoBehaviour
{
    
    int ButtonCellID;
    int cellCount;
    int MaxCellCount;
    float AdnPerSecond;
    public Image CellSpirte;
    Cell CellObject;
    public TextMeshProUGUI CellNameDisplay;
    public Image BGImgae;


    public CellsRoleSystem CellRoleSystem;

    public TextMeshProUGUI MaxCellsDisplay;
    public TextMeshProUGUI cellsDisplay;

    private void Update()
    {
        if (ButtonCellID == 0) // idle cell
        {
            cellsDisplay.text = Stats.stats.IdleCells.ToString(); ;
            MaxCellsDisplay.text = Stats.stats.MaxCells.ToString();
        }
        else if (ButtonCellID == 1) // red blood cell
        {
            cellsDisplay.text = Stats.stats.RedBloodCells.ToString(); ;
            MaxCellsDisplay.text = Stats.stats.MaxRedBloodCells.ToString();
        }
        else if (ButtonCellID == 2) // white blood cell
        {
            cellsDisplay.text = Stats.stats.WhiteBloodCells.ToString(); ;
            MaxCellsDisplay.text = Stats.stats.MaxWhiteBloodCells.ToString();
        }
        else if (ButtonCellID == 3) // palette cell
        {
            cellsDisplay.text = Stats.stats.PalleteCells.ToString(); ;
            MaxCellsDisplay.text = Stats.stats.MaxPaletteCells.ToString();
        }
        else if (ButtonCellID == 4) // macrophage cell
        {
            cellsDisplay.text = Stats.stats.MacrophageCells.ToString(); ;
            MaxCellsDisplay.text = Stats.stats.MaxMacrophageCells.ToString();
        }

    }

    public void ShowCellInfo(int CellID)
    {
        switch (CellID)
        {
            case 0:
                       
                CellObject = CellRoleSystem.CellsScriptableObjects[0];
                CellSpirte.sprite = CellObject.CellSprite;
                CellNameDisplay.text = CellObject.CellName;
                ButtonCellID = CellID;
                BGImgae.color = new Color(0.56f, 0.94f, 1, 1);
                break;
            case 1: 
                CellObject = CellRoleSystem.CellsScriptableObjects[1];
                CellSpirte.sprite = CellObject.CellSprite;
                CellNameDisplay.text = CellObject.CellName;
                ButtonCellID = CellID;
                BGImgae.color = new Color(1, 0.36f, 0.36f, 1);
                break;
            case 2:    
                CellObject = CellRoleSystem.CellsScriptableObjects[2];
                CellSpirte.sprite = CellObject.CellSprite;
                CellNameDisplay.text = CellObject.CellName;
                ButtonCellID = CellID;
                BGImgae.color = new Color(1, 1, 1, 1);
                break;
            case 3:   
                CellObject = CellRoleSystem.CellsScriptableObjects[3];
                CellSpirte.sprite = CellObject.CellSprite;
                CellNameDisplay.text = CellObject.CellName;
                ButtonCellID = CellID;
                BGImgae.color = new Color(1, 0.5f, 0.18f, 1);
                break;

        }
    }

    public void AddButton()
    {
        CellRoleSystem.CellRoleAsign(ButtonCellID);
    }

    public void SubstractButton()
    {
        CellRoleSystem.CellRoleAsign(-ButtonCellID);
    }
    
}
