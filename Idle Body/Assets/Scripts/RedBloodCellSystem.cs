using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedBloodCellSystem : MonoBehaviour
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

    public int RBC;
    public int MaxRBC = 20;

    private string RBCText;
    private string MaxRBCtext;

    public TextMeshProUGUI RBCDisplay;
    public TextMeshProUGUI MaxRBCDisplay;

    public TextMeshProUGUI RBCDisplay2;
    public TextMeshProUGUI MaxRBCDisplay2;

    public float RedBloodCellADNPS;
    public int RedBloodCellADNPSTier;

    private string RedBloodCellADNPSText;
    public TextMeshProUGUI RedBloodCellADNPSDisplay;

    public GameManager GM;
    public BuyIdleCell BuyIdleCell;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

   
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
       RedBloodCellADNPS = (RBC  * Mathf.Pow(1000, GM.DNAPSTier - GM.DNATier));


        // Display texts
        RBCText = RBC.ToString();
        MaxRBCtext = MaxRBC.ToString();

        RBCDisplay.text = RBCText;
        MaxRBCDisplay.text = MaxRBCtext;

        RBCDisplay2.text = RBCText;
        MaxRBCDisplay2.text = MaxRBCtext;

        RedBloodCellADNPSText = CurrencyText(RedBloodCellADNPSText, RedBloodCellADNPS, RedBloodCellADNPSTier);

        RedBloodCellADNPSDisplay.text = RedBloodCellADNPSText + " DNA/s";
    }
    

    public void AddRedBloodCell()
    {
        if(GM.IdleCells != 0)
        {
            if (RBC < MaxRBC)
            {
                GM.IdleCells--;
                RBC++;
               
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
        if (RBC != 0)
        {
            GM.IdleCells++;
            RBC--;
           
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
