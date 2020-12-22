using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedBloodCellSystem : MonoBehaviour
{

    private string RedBloodCellsText;
    private string MaxRedBloodCellsText;

    public TextMeshProUGUI RedBloodCellDisplay;
    public TextMeshProUGUI MaxRedBloodCellDisplay;

    public float RedBloodCellADNPS;
    public int RedBloodCellADNPSTier;

    private string RedBloodCellADNPSText;
    //public TextMeshProUGUI RedBloodCellADNPSDisplay;

    public GameManager GM;
    
    

   
    void Update()
    {
        // Tier Incrementation
        if (RedBloodCellADNPS > 1000)
        {
            RedBloodCellADNPS /= 1000;
            RedBloodCellADNPSTier++;
        }

        if (RedBloodCellADNPS > 0 && RedBloodCellADNPS < 1)
        {
            RedBloodCellADNPS *= 1000;
            RedBloodCellADNPSTier--;
        }

        // red blood cell DNA per Second
       RedBloodCellADNPS = (Stats.stats.RedBloodCells * Mathf.Pow(1000, GM.DNAPSTier - Stats.stats.ADNTier));


        // Display texts
        RedBloodCellsText = Stats.stats.RedBloodCells.ToString();
        MaxRedBloodCellsText = Stats.stats.MaxRedBloodCells.ToString();

        RedBloodCellDisplay.text = RedBloodCellsText;
        MaxRedBloodCellDisplay.text = MaxRedBloodCellsText;

       

        RedBloodCellADNPSText = Stats.stats.CurrencyText(RedBloodCellADNPSText, RedBloodCellADNPS, RedBloodCellADNPSTier);

       // RedBloodCellADNPSDisplay.text = RedBloodCellADNPSText + " DNA/s";
    }
    

    public void AddRedBloodCell()
    {
        if(Stats.stats.IdleCells != 0)
        {
            if (Stats.stats.RedBloodCells < Stats.stats.MaxRedBloodCells)
            {
                Stats.stats.IdleCells--;
                Stats.stats.RedBloodCells++;
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
    public void RemoveRedBloodCell()
    {
        if (Stats.stats.RedBloodCells != 0)
        {
            Stats.stats.IdleCells++;
            Stats.stats.RedBloodCells--;    
        }
    }




       
}
