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




    // Start is called before the first frame update
    void Start()
    {
        DNADisplay.text = DNA.ToString();
        DNAPC = 1;
        MaxCells = 20;
     

        /*
        InteligenceLevel = 1;
        EnduranceLevel = 1;
        DexterityLevel = 1;
        StrenghtLevel = 1;


        InteligenceLevelText.text = InteligenceLevel.ToString();
        DexterityLevelText.text = DexterityLevel.ToString();
        EnduranceLevelText.text = EnduranceLevel.ToString();
        */

        StartCoroutine(AutoTick());

    }

    // Update is called once per frame
    void Update()
    {





        realDNAPSProducerTiers = new[]
        {

            DNAPCTier

        };

        /*DNAPSProducersTiers = new[]
          {


          };
        */

       // DNAPSTier = MaxValue(DNAPSProducersTiers);
       // RealDNAPSTier = MaxValue(realDNAPSProducerTiers);





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




    /*
    // upgrade Main Character
    public void upgradeButton(float costTier, float cost, UpgradeManager.UpgardeTypes type, float valueTier, float Value)
    {
        if (DNATier >= costTier)
        {


            if (DNATier == costTier)
            {
                if (DNA >= cost)
                {
                    if (type == UpgradeManager.UpgardeTypes.Skill)
                    {
                        SkillLevel++;
                        DNA -= cost;

                        Debug.Log("skill Upgraded");
                        RS.ChanceToWin += 10;

                    } else if (type == UpgradeManager.UpgardeTypes.Patience)
                    {
                        PatienceLevel++;
                        DNA -= cost;
                        Debug.Log("Patience Upgraded");
                    } else if (type == UpgradeManager.UpgardeTypes.TeamWork)
                    {
                        TeamworkLevel++;
                        DNA -= cost;
                        Debug.Log("Teamwork Upgraded");
                    }
                    else if (type == UpgradeManager.UpgardeTypes.Equipment)
                    {

                        EquipmentLevel++;
                        DNA -= cost;
                        Debug.Log("Equipment Upgraded");

                        DNAPC += 1; //Mathf.Pow(1.02f, EquipmentLevel);
                    }
                }
                else
                {
                    Debug.Log("notEnoughPoints");
                }

            }
            else
            {
                if (type == UpgradeManager.UpgardeTypes.Skill)
                {
                    DNA -= cost / Mathf.Pow(1000, DNATier - costTier);
                    Debug.Log("skill Upgraded");
                    SkillLevel++;
                    RS.ChanceToWin += 10;


                } else if (type == UpgradeManager.UpgardeTypes.Patience)
                {
                    DNA -= cost / Mathf.Pow(1000, DNATier - costTier);
                    Debug.Log("Patience Upgraded");
                    PatienceLevel++;

                } else if (type == UpgradeManager.UpgardeTypes.TeamWork)
                {
                    DNA -= cost / Mathf.Pow(1000, DNATier - costTier);
                    Debug.Log("Teamwork Upgraded");
                    TeamworkLevel++;

                }
                else if (type == UpgradeManager.UpgardeTypes.Equipment)
                {
                    DNA -= cost / Mathf.Pow(1000, DNATier - costTier);
                    Debug.Log("Teamwork Upgraded");
                    EquipmentLevel++;
                    DNAPC += 1;//Mathf.Pow(1.02f, EquipmentLevel);



                }
            }
        }
    }

 */










}
