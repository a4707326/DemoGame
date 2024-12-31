using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReelDate : BaseData
{
    //public Dictionary<int, Symbol> Symbols;
    public string[] Symbols;


    public ReelDate(string[] strings)
    {
        Symbols = strings;
    }

}
