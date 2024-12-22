using System;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public class PlayerData
//{
//    public string UserId;
//    public string Nickname;
//    public string Email;
//    public int Money;
//    public int Diamonds;
//    public int Energy;
//    public string PhotoUrl;
//    public bool IsEmailVerified;
//    public bool IsPhoneBound;
//    public bool IsGoogleBound;
//    public bool IsLineBound;
//    public bool IsFacebookBound;
//    public bool IsAnonymous;
//}

//[Serializable]
//public class GameSettings
//{
//    public float Volume;
//    public float Sensitivity;
//    public string Language;
//}

//[Serializable]
//public class GameData
//{
//    public PlayerData PlayerData = new PlayerData();
//    public GameSettings Settings = new GameSettings();
//    public Dictionary<string, object> CustomData = new Dictionary<string, object>(); // 可擴展數據
//}

public class GameDataMgr : Singleton<GameDataMgr>
{

    //private const string SaveKey = "GameData"; // 用於存檔的 PlayerPrefs 鍵值



    ///// <summary>
    ///// 初始化數據
    ///// </summary>
    //public void Initialize()
    //{
    //    LoadGameData();
    //}

    ///// <summary>
    ///// 保存遊戲數據
    ///// </summary>
    //public void SaveGameData()
    //{
    //    string json = JsonUtility.ToJson(GameData);
    //    PlayerPrefs.SetString(SaveKey, json);
    //    PlayerPrefs.Save();
    //    Debug.Log("遊戲數據已保存");
    //}

    ///// <summary>
    ///// 加載遊戲數據
    ///// </summary>
    //public void LoadGameData()
    //{
    //    if (PlayerPrefs.HasKey(SaveKey))
    //    {
    //        string json = PlayerPrefs.GetString(SaveKey);
    //        GameData = JsonUtility.FromJson<GameData>(json);
    //        Debug.Log("遊戲數據已加載");
    //    }
    //    else
    //    {
    //        Debug.Log("未找到遊戲數據，使用默認值");
    //        GameData = new GameData(); // 初始化默認值
    //    }
    //}

    ///// <summary>
    ///// 重置遊戲數據
    ///// </summary>
    //public void ResetGameData()
    //{
    //    GameData = new GameData();
    //    SaveGameData();
    //    Debug.Log("遊戲數據已重置");
    //}
}
