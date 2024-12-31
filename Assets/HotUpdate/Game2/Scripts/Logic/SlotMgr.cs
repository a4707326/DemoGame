using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlasticGui.Help.GuiHelp;


public class SlotMgr : MonoSingleSingleton<SlotMgr>
{
    
    public SlotSetting SlotSetting;

    private List<Reel> reels;
    private SymbolResultsDate resultsDate;


    private bool isSpinning = false;

    void Start()
    {
        Init();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //TimersMgr.SetTimer(this,2f,  StartSpin);
            //StartSpin();
        }
        //TimersMgr.SetTimer(this, 5f, () => { Debug.Log("123456789"); });
    }

    public void Init()
    {
        reels = SlotSetting.Reels;
        foreach (Reel reel in reels) 
        {
            reel.InitSymbols(SlotSetting);
        }

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="resultsDate"></param>
    /// <param name="intervalDelay"></param>

    public async void StartSpin(SymbolResultsDate resultsDate, int intervalDelay = 200)
    {
        await Task.Delay(0);

        this.resultsDate = resultsDate;
        if (isSpinning) return;

        isSpinning = true;
        //ResultText.text = "Spinning...";


        if (reels.Count != resultsDate.Reels.Length) 
        {
            Debug.LogError("resultsDate資料Reel數不對");
            return;
        }


        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].StartSpin(resultsDate.Reels[i]);
            await Task.Delay(intervalDelay);
        }


        await Task.Delay(5000);

        //ResultText.text = $"Result: {result}";
        isSpinning = false;
    }

    private SymbolResultsDate GetResult()
    {
        return resultsDate;
    }
}
