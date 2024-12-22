

using System;
using UnityEngine;

public static class Tools
{
    public static string GetResName(string fullPath)
    {
        // 找到最後一個 "/" 的位置
        int lastSlashIndex = fullPath.LastIndexOf('/');
        // 找到最後一個 "." 的位置
        int lastDotIndex = fullPath.LastIndexOf('.');

        // 提取從最後一個 "/" 到 "." 之間的部分
        if (lastSlashIndex >= 0 && lastDotIndex > lastSlashIndex)
        {
            return fullPath.Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1);
        }

        return fullPath; // 返回原始路徑（如果路徑格式不正確）
    }




    /// <summary>
    /// 自動生成暱稱
    /// </summary>
    /// <returns>隨機生成的暱稱</returns>
    public static string GenRandomNickname()
    {

        //string[] adjectives = { "Brave", "Clever", "Swift", "Mighty", "Wise", "Cheerful", "Fierce", "Lucky", "Calm", "Bold"};
        //string[] nouns = { "Tiger", "Eagle", "Dragon", "Lion", "Wolf", "Panther", "Falcon", "Hawk", "Bear", "Phoenix" };
        string[] adjectives = LocalizationMgr.Instance.GetStringList("Nick1stST").ToArray();
        string[] nouns = LocalizationMgr.Instance.GetStringList("Nick2ndST").ToArray(); ;

       
        System.Random random = new();

        string adjective = adjectives[random.Next(adjectives.Length)];
        string noun = nouns[random.Next(nouns.Length)];
        int number = random.Next(1000, 9999); // 生成4位隨機數

        return $"{adjective}{noun}{number}";
    }

}
