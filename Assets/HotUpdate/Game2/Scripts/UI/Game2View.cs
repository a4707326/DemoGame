using NUnit.Framework.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game2View : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Button setting_Btn;
    [SerializeField]
    public Button spin_Btn;
    [SerializeField]
    public TMP_Text result_Text; // 使用 TMP_Text 替代 Text



    private void Awake()
    {
        Init();
    }


    public void Init()
    {
        DebugTools.LogMethodName();


        ReelDate reel1 = new(new string[] { "A", "B", "A", "A", "E", "F" });
        ReelDate reel2 = new(new string[] { "A", "B", "A", "D", "E", "F" });
        ReelDate reel3 = new(new string[] { "A", "B", "C", "A", "E", "F" });
        SymbolResultsDate resultsDate = new(new ReelDate[] { reel1, reel2, reel3 });

        resultsDate.Show();//{"Reels":[{"Symbols":["A","B","C","D","E","F"]},{"Symbols":["A","B","C","D","E","F"]},{"Symbols":["A","B","C","D","E","F"]}]}

        setting_Btn.onClick.AddListener(OnSettingClick);
        spin_Btn.onClick.AddListener(() => SlotMgr.Instance.StartSpin(resultsDate));
        result_Text.text = "Press Spin!";

    }

    private async void OnSettingClick()
    {

        await SceneMgr.Instance.LoadScene(Consts.SceneKey.LobbyScene);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
