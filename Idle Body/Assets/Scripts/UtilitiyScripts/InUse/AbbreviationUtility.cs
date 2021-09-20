using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static string AbbreviateNumber(double number)
    {
        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<double, string> pair = abbrevations.ElementAt(i);
            if (number >= pair.Key)
            {
                float roundedNumber = (float)(number / pair.Key);
                return roundedNumber.ToString("F2") + " " + pair.Value;
            }
        }
        return number.ToString();
    }

}
