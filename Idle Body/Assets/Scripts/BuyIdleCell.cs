using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyIdleCell : MonoBehaviour
{

    public string[] Sufix = new string[]
    {
        "Thousand", // 0
        "Million", // 1
        "Billion", // 2
        "Trillion", // 3
        "Quadrillion", // 4
        "Quintillion", // 5
        "Hextillion", // 6
        "Septillion", //7
        "Octillion", // 8
        "Nonillion",// 9

    };

    public GameManager GM;

    
    

    public float IdleCellCost;
    public int IdleCellCostTier;

    public int quantity;
    private protected float CostMultiplier = 1.15f;

    private string IdleCellsText;
    private string IdleCellCostText;

    public TextMeshProUGUI IdleCellDisplay;
    public TextMeshProUGUI IdleCellCostDisplay;

    CellsRoleSystem cellRoleSystem;

    

    // Start is called before the first frame update
    void Start()
    {
        IdleCellCost = 15;
        cellRoleSystem = GetComponent<CellsRoleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
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

        IdleCellsText = GM.IdleCells.ToString();
        IdleCellCostText = CurrencyText(IdleCellCostText, IdleCellCost, IdleCellCostTier);

        IdleCellDisplay.text = IdleCellsText;
        IdleCellCostDisplay.text = IdleCellCostText;
    }

    public void addIdleCell()
    {
        if (GM.Cells < GM.MaxCells)
        {
            if (GM.DNATier >= IdleCellCostTier)
            {
                if (GM.DNATier == IdleCellCostTier)
                {
                    if (GM.DNA >= IdleCellCost)
                    {
                        GM.DNA -= IdleCellCost;
                        
                        IdleCellCost *= CostMultiplier;
                        GM.IdleCells++;
                        cellRoleSystem.CellRoleAsign(0);
                        GM.Cells++;
                    }
                }
                else
                {

                    GM.DNA -= IdleCellCost / Mathf.Pow(1000, GM.DNATier - IdleCellCostTier);
                   
                    IdleCellCost *= Mathf.Pow(CostMultiplier, GM.DNATier - IdleCellCostTier);
                    GM.Cells++;
                    cellRoleSystem.CellRoleAsign(0);
                    GM.IdleCells++;

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

    private string CurrencyText(string currencyText, float currency, int tier)
    {
        if (tier - 1 > -1)
        {
            currencyText = currency.ToString("#.00") + " " + Sufix[tier - 1];
        }
        else
        {
            currencyText = currency.ToString("#.00");
        }
        return currencyText;
    }
}
