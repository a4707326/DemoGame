using System;
using System.Collections.Generic;
using UnityEngine;


public class LocalDataMgr : Singleton<LocalDataMgr>
{

    public PlayerData PlayerData;
    public SettingsData SettingsData;


    public void Init()
    {
        PlayerData = new PlayerData();
        SettingsData = new SettingsData();

        //PlayerData = new PlayerData
        //{
        //    Nickname = "Player123",
        //    Email = "player123@example.com",
        //    Money = 1000,
        //    Diamonds = 50,
        //    Level = 5
        //};

        //PlayerData.Show();
        //// 序列化為 JSON
        //string json = PlayerData.ToJson();
        //Debug.Log($"JSON: {json}");
    }


}
