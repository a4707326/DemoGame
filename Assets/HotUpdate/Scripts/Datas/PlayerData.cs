using System;
using UnityEngine;

[System.Serializable]
public class PlayerData : BaseData
{
    // 使用者ID
    public string UserId;

    // 使用者基本資訊
    public string Nickname;
    public string Account;
    public string Password;
    public string Email;
    public string PhoneNumber;

    // 綁定狀態
    public bool IsGoogleBound;
    public bool IsLineBound;
    public bool IsFBBound;
    public bool IsAnonymous;

    // 資源相關
    public int Money;
    public int Diamonds;
    public int Energy;

    // 其他屬性
    public string PhotoUrl;
    public int Level;

    //Firebase
    private readonly string firebasePath;

    // Constructor（初始化默認值）
    public PlayerData()
    {
        UserId = "";
        Nickname = "";
        Account = string.Empty;
        Password = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
        IsGoogleBound = false;
        IsLineBound = false;
        IsFBBound = false;
        IsAnonymous = true;
        Money = 0;
        Diamonds = 0;
        Energy = 0; // 默認滿體力
        PhotoUrl = string.Empty;
        Level = 0; // 初始等級為1
    }

    //public string ToJson()
    //{
    //    return JsonUtility.ToJson(this);
    //}

    ////把Firebase讀取讀取的資料放進本地資料
    //public void FromJson(string json)
    //{
    //    PlayerData updatedData = JsonUtility.FromJson<PlayerData>(json);
    //    foreach (var field in typeof(PlayerData).GetFields())
    //    {
    //        field.SetValue(this, field.GetValue(updatedData));
    //    }
    //}

    //public void Show()
    //{
    //    Debug.Log($" Show JSON: {ToJson()}");
    //}

}
