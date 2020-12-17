using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
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

    
    public int Cells;
    public int MaxCells;

    public int IdleCells;
    

    private string CellsText;
    private string MaxCellsText;

    public TextMeshProUGUI CellsDisplay;
    public TextMeshProUGUI MaxCellsDisplay;

    public float DNA; // current game points
    public float DNAPS;
    public float DNAPC;


    public int DNATier; // current game points tier
    public int DNAPSTier;
    public int DNAPCTier;

    public string DNAText;
    public string DNAPStext;
   

    public TextMeshProUGUI DNADisplay;
    public TextMeshProUGUI DNAPSDisplay;
  

/*
    // possible upgrades
    public int InteligenceLevel; 
    public int EnduranceLevel;
    public int DexterityLevel;
    public int StrenghtLevel;


    public int InteligenceUpgradeCost;
    public int EnduranceUpgradeCost;
    public int DexterityUpgradeCost;
    public int StrenghtLevelCost;

    public int InteligenceUpgradeCostTier;
    public int EnduranceUpgradeCostTier;
    public int DexterityUpgradeCostTier;
    public int StrenghtLevelTier;

    public TextMeshProUGUI InteligenceLevelText;
    public TextMeshProUGUI EnduranceLevelText;
    public TextMeshProUGUI DexterityLevelText;
    public TextMeshProUGUI StrenghtLevelText;
*/

    public bool DNAPSActive;
    public int[] DNAPSProducersTiers;


    //Combines All sources of income

    public float RealDNAPS;
    public int RealDNAPSTier;
    public string RealDNAPSText;
    public int[] realDNAPSProducerTiers;

    public RedBloodCellSystem RBCS;


    //autotick RedBloodCells

    CellsRoleSystem cellsRoleSystem;
    float RedBloodCellPointsPS;
    int RedBloodCellCount;





    // Start is called before the first frame update
    void Start()
    {
        cellsRoleSystem = GetComponent<CellsRoleSystem>();
        DNADisplay.text = DNA.ToString();
        DNAPC = 1;
        MaxCells = 20;
   

        //StartCoroutine(AutoTick());
        StartCoroutine(AutoTickRedBloodCells());

    }

    // Update is called once per frame
    void Update()
    {





        realDNAPSProducerTiers = new[]
        {

            DNAPCTier

        };

        




        DNAPS = (RBCS.RedBloodCellADNPS);

        

        #region TierIncrementation
        if (DNA >= 1000)
        {
            DNA /= 1000;
            DNATier++;

        }

        if (DNATier != 0)
        {
            if (DNA > 0 && DNA < 1)
            {
                DNA *= 1000;
                DNATier--;
            }
        }
       



        if (DNAPS >= 1000)
        {
            DNAPS /= 1000;
            DNAPSTier++;

        }

        if (DNAPS > 0 && DNAPS < 1)
        {
            DNAPS *= 1000;
            DNAPSTier--;
        }



        if (DNAPC >= 1000)
        {
            DNAPC /= 1000;
            DNAPCTier++;

        }

        if (DNAPC > 0 && DNAPC < 1)
        {
            DNAPC *= 1000;
            DNAPCTier--;
        }

        #endregion

        DNATier = Mathf.Clamp(DNATier, 0, Sufix.Length);
        DNAPSTier = Mathf.Clamp(DNAPSTier, 0, Sufix.Length);
        DNAPCTier = Mathf.Clamp(DNAPCTier, 0, Sufix.Length);

 
        DNAText = CurrencyText(DNAText, DNA, DNATier);
        DNAPStext = CurrencyText(DNAPStext, DNAPS, DNAPSTier);
        RealDNAPSText = CurrencyText(RealDNAPSText, RealDNAPS, RealDNAPSTier);

        DNADisplay.text = DNAText;
        DNAPSDisplay.text = DNAPStext + (" DNA/S");

        //GPPSDisplay.text = CurrencyText(GPPSDisplay.text, GPPS, GPPSTier);
        //GPPCDisplay.text = CurrencyText(GPPCDisplay.text, GPPC, GGPCTier);


//      InteligenceLevelText.text = InteligenceLevel.ToString();
//      DexterityLevelText.text = DexterityLevel.ToString();
//      EnduranceLevelText.text = EnduranceLevel.ToString();
//      StrenghtLevelText.text = StrenghtLevel.ToString();


        if (DNA <= 0)
        {
            DNA = 0;
        }


        CellsText = Cells.ToString();
        MaxCellsText = MaxCells.ToString();

        CellsDisplay.text = CellsText;
        MaxCellsDisplay.text = MaxCellsText;


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

    public void PressButton(int GamePointsEarned)

    {
        if (DNAPCTier > DNATier)
        {

            DNA += DNAPC * Mathf.Pow(1000, DNAPCTier - DNATier);

        }
        else if (DNAPCTier < DNATier)
        {
            
            DNA += DNAPC / Mathf.Pow(1000, DNATier - DNAPCTier); 

        }
        else
        {  
            DNA += DNAPC;
        }
        
        
    }
    IEnumerator AutoTick()
    {
        while (true)
        {
            Debug.Log("tick)");
            yield return new WaitForSeconds(1f);
            if (DNAPSTier > DNATier)
            {
                
                   DNA += (DNAPS * Mathf.Pow(1000, DNAPSTier - DNATier)) * .1f;      

            }
            else if (DNAPSTier < DNATier)
            {

                DNA += (DNAPS / Mathf.Pow(1000, DNATier - DNAPSTier)) * .1f;
                
            }

            else
            {

                 DNA += DNAPS;

            }
        }

    }

    IEnumerator AutoTickRedBloodCells()
    {
        while (true)
        {

            yield return new WaitForSeconds(3f);
            if (cellsRoleSystem.RedBloodCells.Count != 0)
            {
                if (cellsRoleSystem.RedBloodCells.Count > RedBloodCellCount)
                {
                    RedBloodCell cellImage = cellsRoleSystem.RedBloodCells[RedBloodCellCount].GetComponent<RedBloodCell>();
                    cellImage.showPointsGained();
                    if (DNAPSTier > DNATier)
                    {
                        DNA += (1 * Mathf.Pow(1000, DNAPSTier - DNATier)) * .1f;
                    }
                    else if (DNAPSTier < DNATier)
                    {
                        DNA += (1 / Mathf.Pow(1000, DNATier - DNAPSTier)) * .1f;
                    }
                    else
                    {
                        DNA += 1;
                    }
                    RedBloodCellCount++;
                }
                else
                {
                    RedBloodCellCount = 0;
                }
            } else
            {
                Debug.Log("no Red Blood Cells");
            }
           

            
        }
    }



        int MaxValue(int[] intArray)
    {
        int max = intArray[0];
        for (var i = 1; i < intArray.Length; i++)
        {
            if (intArray[i] > max)
            {
                max = intArray[i];
            }
        }

        return max;
    }

}
