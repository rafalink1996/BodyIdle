using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WhiteBloodCellSystem : MonoBehaviour
{
    private string WhiteBloodCellsText;
    private string MaxWhiteBloodCellsText;

    public TextMeshProUGUI WhiteBloodCellDisplay;
    public TextMeshProUGUI MaxWhiteBloodCellDisplay;


    public TestGameManager GM;




    void Update()
    {
        // Display texts
        WhiteBloodCellsText = Stats.stats.WhiteBloodCells.ToString();
        MaxWhiteBloodCellsText = Stats.stats.MaxWhiteBloodCells.ToString();

        WhiteBloodCellDisplay.text = WhiteBloodCellsText;
        MaxWhiteBloodCellDisplay.text = MaxWhiteBloodCellsText;
 
    }


    public void AddWhiteBloodCell()
    {
        if (Stats.stats.IdleCells != 0)
        {
            if (Stats.stats.WhiteBloodCells < Stats.stats.MaxWhiteBloodCells)
            {
                Stats.stats.IdleCells--;
                Stats.stats.WhiteBloodCells++;
            }
            else
            {
                Debug.Log("Max Red Blood Cells Reached");
            }
        }
        else
        {
            Debug.Log("no Idle Cells available");
        }

    }
    public void RemoveWhiteBloodCell()
    {
        if (Stats.stats.WhiteBloodCells != 0)
        {
            Stats.stats.IdleCells++;
            Stats.stats.WhiteBloodCells--;
        }
    }





}