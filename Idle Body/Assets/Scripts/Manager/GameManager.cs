using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{

    private string CellsText;
    private string MaxCellsText;

    public TextMeshProUGUI CellsDisplay;
    public TextMeshProUGUI MaxCellsDisplay;

    
    public float DNAPS;
    public float DNAPC;


    
    public int DNAPSTier;
    public int DNAPCTier;

    public string DNAText;
    public string DNAPStext;
   

    public TextMeshProUGUI DNADisplay;
    public TextMeshProUGUI DNAPSDisplay;
  

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
        DNADisplay.text = Stats.stats.ADN.ToString();
        DNAPC = 1;
        Stats.stats.MaxCells = 20;
        Stats.stats.MaxRedBloodCells = 20;
        Stats.stats.MaxWhiteBloodCells = 5;
   

        //StartCoroutine(AutoTick());
        StartCoroutine(AutoTickRedBloodCells());

        //Application.targetFrameRate = 30;

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

        if (Stats.stats.ADN >= 1000)
        {
            Stats.stats.ADN /= 1000;
            Stats.stats.ADNTier ++;

        }

        if (Stats.stats.ADNTier != 0)
        {
            if (Stats.stats.ADN > 0 && Stats.stats.ADN < 1)
            {
                Stats.stats.ADN *= 1000;
                Stats.stats.ADNTier--;
            }
        }
       
        // DNA per Second

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


        // DNA per click
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

        Stats.stats.ADNTier = Mathf.Clamp(Stats.stats.ADNTier, 0, Stats.stats.Sufix.Length);
        DNAPSTier = Mathf.Clamp(DNAPSTier, 0, Stats.stats.Sufix.Length);
        DNAPCTier = Mathf.Clamp(DNAPCTier, 0, Stats.stats.Sufix.Length);

 
        DNAText = Stats.stats.CurrencyText(DNAText, Stats.stats.ADN, Stats.stats.ADNTier);
        DNAPStext = Stats.stats.CurrencyText(DNAPStext, DNAPS, DNAPSTier);
        RealDNAPSText = Stats.stats.CurrencyText(RealDNAPSText, RealDNAPS, RealDNAPSTier);

        DNADisplay.text = DNAText;
        DNAPSDisplay.text = DNAPStext + (" DNA/S");

 


        if (Stats.stats.ADN <= 0)
        {
            Stats.stats.ADN = 0;
        }


        CellsText = Stats.stats.cells.ToString();
        MaxCellsText = Stats.stats.MaxCells.ToString();

        CellsDisplay.text = CellsText;
        MaxCellsDisplay.text = MaxCellsText;


    }

    

    public void PressButton(int GamePointsEarned)
    {
        if (DNAPCTier > Stats.stats.ADNTier)
        {
            Stats.stats.ADN += DNAPC * Mathf.Pow(1000, DNAPCTier - Stats.stats.ADNTier);
        }
        else if (DNAPCTier < Stats.stats.ADNTier)
        {
            Stats.stats.ADN += DNAPC / Mathf.Pow(1000, Stats.stats.ADNTier - DNAPCTier); 
        }
        else
        {
            Stats.stats.ADN += DNAPC;
        }   
    }


    IEnumerator AutoTick()
    {
        while (true)
        {
            Debug.Log("tick)");
            yield return new WaitForSeconds(1f);
            if (DNAPSTier > Stats.stats.ADNTier)
            {
                Stats.stats.ADN += (DNAPS * Mathf.Pow(1000, DNAPSTier - Stats.stats.ADNTier)) * .1f;      
            }
            else if (DNAPSTier < Stats.stats.ADNTier)
            {
                Stats.stats.ADN += (DNAPS / Mathf.Pow(1000, Stats.stats.ADNTier - DNAPSTier)) * .1f;     
            }

            else
            {
                Stats.stats.ADN += DNAPS;
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
                    CellBehaviour CellPlus1 = cellsRoleSystem.RedBloodCells[RedBloodCellCount].GetComponent<CellBehaviour>();
                    CellPlus1.showPointsGained();
                    if (DNAPSTier > Stats.stats.ADNTier)
                    {
                        Stats.stats.ADN += (1 * Mathf.Pow(1000, DNAPSTier - Stats.stats.ADNTier)) * .1f;
                    }
                    else if (DNAPSTier < Stats.stats.ADNTier)
                    {
                        Stats.stats.ADN += (1 / Mathf.Pow(1000, Stats.stats.ADNTier - DNAPSTier)) * .1f;
                    }
                    else
                    {
                        Stats.stats.ADN += 1;
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
