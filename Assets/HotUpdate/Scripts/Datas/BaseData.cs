using System;
using UnityEngine;
using static Consts;

[System.Serializable]
public abstract class BaseData
{

    public virtual string ToJson()
    {
        return JsonUtility.ToJson(this);
    }


    /// <summary>
    /// 把Firebase讀取讀取的資料放進本地資料，從 JSON 字串加載資料到當前物件，支援子類別
    /// </summary>
    public virtual void FromJson(string json)
    {
        // 獲取子類別的型別（動態解析當前物件型別）
        Type currentType = this.GetType();
        object updatedData = JsonUtility.FromJson(json, currentType);

        // 將解析後的資料覆蓋當前物件的字段
        foreach (var field in currentType.GetFields())
        {
            field.SetValue(this, field.GetValue(updatedData));
        }
    }

    public virtual void Show()
    {
        Debug.Log($" Show JSON: {ToJson()}");
    }

    public virtual void SaveToPlayerPrefs()
    {
        PlayerPrefsReflector.SaveToPlayerPrefs(this);
    }
    public virtual void LoadFromPlayerPrefs()
    {
        PlayerPrefsReflector.LoadFromPlayerPrefs(this);
    }
    public virtual void ClearPlayerPrefs()
    {
        Type currentType = this.GetType();
        PlayerPrefsReflector.ClearPlayerPrefs(currentType);
    }

}


