using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    public static Stats stats;

    // ---- Sufix ---- //

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



    // ---- currencies ---- //

    // currency 

    public float ADN;
    public float GoldenMolecules;
    public float cells;

    // currency tier

    public int ADNTier;
    public int GoldenMoleculesTier;


    // ----- Cells ----- //

    // Amount:
    public int IdleCells;
    public int RedBloodCells;
    public int WhiteBloodCells;
    public int PalleteCells;
    public int BCells;
    public int MacrophageCells;

    // MaxAmount

    public int MaxCells;
    public int MaxRedBloodCells;
    public int MaxWhiteBloodCells;
    public int MaxPaletteCells;
    public int MaxBCells;
    public int MaxMacrophageCells;


    // Body Level

    public int BodyLevel;

    

    // -----  singleton check ---- //

    void Awake()
    {
        if (stats == null)
        {
            DontDestroyOnLoad(gameObject);
            stats = this;
        }
        else if (stats != this)
        {
            Destroy(gameObject);
        }
    }



    // --------- Globaly used Functions ------------ //

    public string CurrencyText(string currencyText, float currency, int tier)
    {
        if (tier - 1 > -1)
        {
            currencyText = currency.ToString("#.00") + " " + Stats.stats.Sufix[tier - 1];
        }
        else
        {
            currencyText = currency.ToString("#.00");
        }
        return currencyText;
    }
}
