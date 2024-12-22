using System;
using System.Reflection;
using UnityEngine;

public static class PlayerPrefsReflector
{
    /// <summary>
    /// 使用反射將 BaseData 或其子類屬性存儲到 PlayerPrefs
    /// </summary>
    public static void SaveToPlayerPrefs<T>(T data) where T : BaseData
    {
        Type type = data.GetType(); // 使用 data.GetType() 來取得運行時類型（真實子類）

        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            string key = $"{type.Name}_{field.Name}"; // Key 使用 類名_屬性名

            object value = field.GetValue(data);

            if (value is int intValue)
                PlayerPrefs.SetInt(key, intValue);
            else if (value is float floatValue)
                PlayerPrefs.SetFloat(key, floatValue);
            else if (value is string stringValue)
                PlayerPrefs.SetString(key, stringValue);
            else if (value is bool boolValue)
                PlayerPrefs.SetInt(key, boolValue ? 1 : 0);
        }
        PlayerPrefs.Save();
        Debug.Log($"[PlayerPrefsReflector] {type.Name} saved to PlayerPrefs.");
    }

    /// <summary>
    /// 使用反射從 PlayerPrefs 加載資料並填充到 BaseData 或其子類屬性
    /// </summary>
    public static void LoadFromPlayerPrefs<T>(T data) where T : BaseData
    {
        //Type type = typeof(T);
        Type type = data.GetType(); // 使用 data.GetType() 來取得運行時類型（真實子類）

        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            string key = $"{type.Name}_{field.Name}";

            if (!PlayerPrefs.HasKey(key))
                continue;

            if (field.FieldType == typeof(int))
                field.SetValue(data, PlayerPrefs.GetInt(key));
            else if (field.FieldType == typeof(float))
                field.SetValue(data, PlayerPrefs.GetFloat(key));
            else if (field.FieldType == typeof(string))
                field.SetValue(data, PlayerPrefs.GetString(key));
            else if (field.FieldType == typeof(bool))
                field.SetValue(data, PlayerPrefs.GetInt(key) == 1);
        }
        Debug.Log($"[PlayerPrefsReflector] {type.Name} loaded from PlayerPrefs.");
    }

    /// <summary>
    /// 清除指定 BaseData 或其子類的 PlayerPrefs 資料
    /// </summary>
    public static void ClearPlayerPrefs(Type type)
    {
        // 驗證傳入的 Type 是否繼承自 BaseData
        if (!typeof(BaseData).IsAssignableFrom(type))
        {
            Debug.LogError($"[PlayerPrefsReflector] Type {type.Name} does not inherit from BaseData.");
            return;
        }


        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            string key = $"{type.Name}_{field.Name}";
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        Debug.Log($"[PlayerPrefsReflector] {type.Name} data cleared from PlayerPrefs.");
    }
    public static void ClearPlayerPrefs<T>() where T : BaseData
    {
        ClearPlayerPrefs(typeof(T));
    }
}
