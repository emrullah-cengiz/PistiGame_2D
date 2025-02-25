using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Color = System.Drawing.Color;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public static string ToAbbreviated(this int num, string currencyPrefix = "", [CanBeNull] string prefixColor = "#a0ffa8")
    {
        float value = num;
        string suffix = "";

        switch (num)
        {
            case >= 1_000_000_000:
                value /= 1_000_000_000f; suffix = "B";
                break;
            case >= 1_000_000:
                value /= 1_000_000f; suffix = "M";
                break;
            case >= 1_000:
                value /= 1_000f; suffix = "K";
                break;
        }

        if(prefixColor is not null)
            currencyPrefix = $"<color={prefixColor}>{currencyPrefix}</color>";
                
        return currencyPrefix + (suffix == "" ? num : value.ToString("0.#") + suffix);
    }

    
}