using System;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopupView : PopupView, IPopup
{


    // 登出按鈕
    [SerializeField]
    private Button logout_Btn;
    [SerializeField]
    private Button language_Btn;
    [SerializeField]
    private Button music_Btn;
    [SerializeField]
    private Button sfx_Btn;

    [SerializeField]
    private TMP_Text music_Text;
    [SerializeField]
    private TMP_Text sfx_Text;

    public override void Init(object data)
    {
        base.Init(data);
        logout_Btn.onClick.AddListener(OnLogoutClick);
        language_Btn.onClick.AddListener(OnLanguageClick);
        music_Btn.onClick.AddListener(OnMusicClick);
        sfx_Btn.onClick.AddListener(OnSFXClick);

        if (LocalDataMgr.Instance.SettingsData.MusicVolume > 0)
        {
            music_Text.text = "音樂:ON";
        }
        else
        {
            music_Text.text = "音樂:OFF";
        }
        if (LocalDataMgr.Instance.SettingsData.SoundVolume > 0)
        {
            sfx_Text.text = "音效:ON";
        }
        else
        {
            sfx_Text.text = "音效:OFF";
        }

    }

    private void OnSFXClick()
    {
        Debug.Log("OnSFXClick");
        if (LocalDataMgr.Instance.SettingsData.SoundVolume > 0)
        {
            sfx_Text.text = "音效:ON";
        }
        else
        {
            sfx_Text.text = "音效:OFF";
        }

    }

    private void OnMusicClick()
    {
        Debug.Log("OnMusicClick");
        AudioMgr.Instance.ToggleMusicPause();
        if (LocalDataMgr.Instance.SettingsData.MusicVolume > 0)
        {
            music_Text.text = "音樂:ON";
        }
        else
        {
            music_Text.text = "音樂:OFF";
        }
    }

    private void OnLogoutClick()
    {
        Debug.Log("OnLogoutClick");
        FirebaseMgr.Instance.Logout();
    }
    private async void OnLanguageClick()
    {
        Debug.Log("OnLanguageClick");
        await PopupMgr.Instance.ShowPopup(Consts.PopupKey.LanguagePopupView);
        
    }

    internal override void OnDestroy()
    {
        base.OnDestroy();
        logout_Btn.onClick.RemoveAllListeners();
        language_Btn.onClick.RemoveAllListeners();
        music_Btn.onClick.RemoveAllListeners();
        sfx_Btn.onClick.RemoveAllListeners();
    }


    //private void Awake()
    //{
    //    //元件初始化
    //    Init(null);
    //}

}
