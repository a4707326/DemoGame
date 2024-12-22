using UnityEngine;

[System.Serializable]
public class SettingsData : BaseData
{
    public float MusicVolume = 1f;//0~1
    public float SoundVolume = 1f;//0~1
    public bool IsMuted = false;
    public string Language = Consts.Language.zh_TW;
    public string Account = "";
    public string Password = "";


    // 預設建構子，初始化默認值
    public SettingsData()
    {

        LoadFromPlayerPrefs();
        Show();
        //SoundVolume = 1.0f; // 預設音效音量為最大
        //MusicVolume = 1.0f; // 預設音樂音量為最大
        //IsMuted = false;    // 預設不靜音
        //Language = "en";    // 預設語言為英文
        //Account = string.Empty;
        //Password = string.Empty;
    }

    //// 顯示目前設定 (Debug 用)
    //public void Show()
    //{
    //    Debug.Log($"SettingsData: SoundVolume={SoundVolume}, MusicVolume={MusicVolume}, IsMuted={IsMuted}, Language={Language}, Account={Account}, Password={(string.IsNullOrEmpty(Password) ? "Empty" : "****")}");
    //}

}
