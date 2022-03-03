﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BreakInfinity;

public static class AbbreviationUtility
{
    private static readonly SortedDictionary<double, string> abbrevations = new SortedDictionary<double, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" },
         {1000000000000, "t" },
         {1000000000000000, "q" },
         {1000000000000000000, "Q" },
     };
    private static readonly SortedDictionary<BigDouble, string> Breakfinityabbrevations = new SortedDictionary<BigDouble, string>
     {
         {1e3,"K"},
         {1e6, "M" },
         {1e9, "B" },
         {1e12, "t" },
         {1e15, "q" },
         {1e18, "Q" },
         {1e21, "s" },
         {1e24, "S" },
         {1e27, "o" },
         {1e30, "n" },
         {1e33, "d" },
         {1e36, "U" },
     };

    public static string AbbreviateNumber(double number)
    {
        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<double, string> pair = abbrevations.ElementAt(i);
            if (number >= pair.Key)
            {
                float roundedNumber = (float)(number / pair.Key);
                //return roundedNumber.ToString("F2") + " " + pair.Value;
                return (decimal.Truncate((decimal)roundedNumber * 100) / 100).ToString("F2") + " " + pair.Value;
            }
        }
        return (decimal.Truncate((decimal)number * 100) / 100).ToString("F2");
        //return number.ToString();
    }

    public static string AbbreviateBigDoubleNumber(BigDouble number)
    {
        for (int i = Breakfinityabbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<BigDouble, string> pair = Breakfinityabbrevations.ElementAt(i);
            if (number >= pair.Key)
            {
                float roundedNumber = (float)(number / pair.Key);
                //return roundedNumber.ToString("F2") + " " + pair.Value;
                return (decimal.Truncate((decimal)roundedNumber * 100) / 100).ToString("F2") + " " + pair.Value;
            }
        }
        float fNumber = (float)number;
        return (decimal.Truncate((decimal)fNumber * 100) / 100).ToString("F2");
        //return number.ToString();
    }

}
