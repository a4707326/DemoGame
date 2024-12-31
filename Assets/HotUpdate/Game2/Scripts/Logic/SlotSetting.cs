using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlasticGui.Help.GuiHelp;

public class SlotSetting : MonoBehaviour
{
    [Header("Reels")]
    public List<Reel> Reels;


    [Header("Symbols")]
    public string[] Symbols = { "A", "B", "C", "D", "E", "F" }; // 基本可用符號


    [Header("Time")]
    private float spinningTime = 4f;    // 高速旋轉時間
    private float decelerateTime = 3f;   // 停輪減速時間，如果symbolResultsStack跑完會直接停

}
