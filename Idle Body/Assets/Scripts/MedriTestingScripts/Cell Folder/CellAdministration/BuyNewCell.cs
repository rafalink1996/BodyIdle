using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyNewCell : MonoBehaviour
{

    public float IdleCellCost;
    public int IdleCellCostTier;

    private protected float CostMultiplier = 1.15f;

    private string IdleCellsText;
    private string IdleCellCostText;

    public TextMeshProUGUI IdleCellDisplay;
    public TextMeshProUGUI IdleCellCostDisplay;

    CellsRoleSystem cellRoleSystem;

    

   
    void Start()
    {
        IdleCellCost = 15;
        cellRoleSystem = GetComponent<CellsRoleSystem>();
        
    }


    void Update()
    {

        // check tier
        if (IdleCellCost >= 1000)
        {
            IdleCellCost /= 1000;
            IdleCellCostTier++;

        }

        if (IdleCellCost > 0 && IdleCellCost < 1)
        {
            IdleCellCost *= 1000;
            IdleCellCostTier--;
        }


        // Display Cost

        IdleCellsText = Stats.stats.IdleCells.ToString();
        IdleCellCostText = Stats.stats.CurrencyText(IdleCellCostText, IdleCellCost, IdleCellCostTier);

        IdleCellDisplay.text = IdleCellsText;
        IdleCellCostDisplay.text = IdleCellCostText;
    }

    public void addIdleCell()
    {
        if (Stats.stats.cells < Stats.stats.MaxCells)
        {
            if (Stats.stats.ADNTier >= IdleCellCostTier)
            {
                if (Stats.stats.ADNTier == IdleCellCostTier)
                {
                    if (Stats.stats.ADN >= IdleCellCost)
                    {
                        Stats.stats.ADN -= IdleCellCost;
                        
                        IdleCellCost *= CostMultiplier;
                        Stats.stats.IdleCells++;
                        cellRoleSystem.CellRoleAsign(0);
                        Stats.stats.cells++;
                    }
                }
                else
                {

                    Stats.stats.ADN -= IdleCellCost / Mathf.Pow(1000, Stats.stats.ADNTier - IdleCellCostTier);
                    IdleCellCost *= Mathf.Pow(CostMultiplier, Stats.stats.ADNTier - IdleCellCostTier);
                    Stats.stats.cells++;
                    cellRoleSystem.CellRoleAsign(0);
                    Stats.stats.IdleCells++;

                }
            }
            else
            {
                Debug.Log("not enough DNA");
            }
        } else
        {
            Debug.Log("Max Cells reached");
        }
       
    }

    
}
