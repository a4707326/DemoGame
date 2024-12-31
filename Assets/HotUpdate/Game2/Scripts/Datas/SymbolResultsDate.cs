using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolResultsDate : BaseData
{
    public ReelDate[] Reels;

    public SymbolResultsDate(ReelDate[] reels)
    {
        Reels = reels;
    }



}
