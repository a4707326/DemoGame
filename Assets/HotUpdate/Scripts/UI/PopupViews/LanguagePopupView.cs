using System;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LanguagePopupView : PopupView, IPopup
{


    [SerializeField]
    private Button Language1_Btn;
    [SerializeField]
    private Button Language2_Btn;


    public override void Init(object data)
    {
        base.Init(data);
        Language1_Btn.onClick.AddListener(OnLanguage1Click);
        Language2_Btn.onClick.AddListener(OnLanguage2Click);
    }
    private void OnLanguage1Click()
    {
        Debug.Log("OnLanguage1Click");
        LocalDataMgr.Instance.SettingsData.Language = Consts.Language.zh_TW;
        SwitchLanguage();
        OnExitClick();
    }

    private void OnLanguage2Click()
    {
        Debug.Log("OnLanguage2Click");
        LocalDataMgr.Instance.SettingsData.Language = Consts.Language.en_US;
        SwitchLanguage();
        OnExitClick();
    }

    private async void SwitchLanguage()
    {
        await LocalizationMgr.Instance.SetLanguageAsync(LocalDataMgr.Instance.SettingsData.Language);
        LocalDataMgr.Instance.SettingsData.SaveToPlayerPrefs();
    }


    internal override void OnDestroy()
    {
        base.OnDestroy();
        Language1_Btn.onClick.RemoveAllListeners();
        Language2_Btn.onClick.RemoveAllListeners();
    }


    //private void Awake()
    //{
    //    //元件初始化
    //    Init(null);
    //}

}
