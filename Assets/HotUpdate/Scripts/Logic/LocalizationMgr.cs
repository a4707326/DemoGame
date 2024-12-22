using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;
using System.Linq;

public class LocalizationMgr : Singleton<LocalizationMgr>
{
    private Dictionary<string, StringTable> loadedTables = new();
    public async Task Init() 
    {
        // 等待 GetAllTables 的非同步結果
        var handle = LocalizationSettings.StringDatabase.GetAllTables();
        await handle.Task;

        // 確保操作成功
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var tableCollection = handle.Result;
            //List<string> tableNames = new();

            foreach (var table in tableCollection)
            {
                if (table is StringTable stringTable)
                {
                    //tableNames.Add(stringTable.name); // 獲取表的名稱

                    await LoadStringTableAsync(RemoveParentheses(stringTable.name));
                }
            }

            //起始語言設定
            await SetLanguageAsync(LocalDataMgr.Instance.SettingsData.Language);
        }
        else
        {
            Debug.LogError("無法獲取 StringTable 集合");
        }


        //await LoadStringTableAsync("Nick1stST");
        //await LoadStringTableAsync("Nick2ndST");
        //await LoadStringTableAsync("MainST");
    }


    /// <summary>
    /// 異步加載指定的 String Table
    /// </summary>
    /// <param name="tableName">String Table 名稱</param>
    /// <returns></returns>
    public async Task<bool> LoadStringTableAsync(string tableName)
    {
        if (loadedTables.ContainsKey(tableName))
        {
            Debug.Log($"表 {tableName} 已經加載過了。");
            return true;
        }

        var localizedTable = new LocalizedStringTable { TableReference = tableName };

        // 獲取 String Table 資料
        var tableHandle = localizedTable.GetTableAsync();
        await tableHandle.Task;

        if (tableHandle.Status == AsyncOperationStatus.Succeeded && tableHandle.Result != null)
        {
            loadedTables[tableName] = tableHandle.Result;
            Debug.Log($"表 {tableName} 加載成功！");
            return true;
        }
        else
        {
            Debug.LogError($"表 {tableName} 加載失敗！");
            return false;
        }
    }

    // 獲取特定鍵的本地化字符串
    public string GetString(string stringName, string key)
    {
        StringTable stringTable = loadedTables[stringName];
        var entry = stringTable.GetEntry(key);
        return entry?.LocalizedValue ?? $"Key '{key}' not found!";
    }

    // 獲取指定鍵範圍的本地化字符串
    public  List<string> GetStringListInRange(string stringName, int start, int end)
    {
        StringTable stringTable = loadedTables[stringName];
        var results = new List<string>();
        int index = 0;
        foreach (var entry in stringTable)
        {
            if (index >= start && index <= end)
            {
                results.Add(entry.Value.LocalizedValue);
            }
            index++;
        }
        return results;
    }

    // 獲取所有本地化字符串
    public List<string> GetStringList(string stringName)
    {
        StringTable stringTable = loadedTables[stringName];
        var results = new List<string>();
        foreach (var entry in stringTable)
        {
            results.Add(entry.Value.LocalizedValue);
        }
        return results;
    }

    public async Task<StringTable> GetStringTableAsync(string tableName)
    {
        var handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
        await handle.Task; // 等待操作完成

        var table = handle.Result;

        if (table == null)
        {
            Debug.LogError($"StringTable '{tableName}' 加載失敗或不存在！");
            return null;
        }

        return table;
    }


    private string RemoveParentheses(string input)
    {
        int index = input.IndexOf('_'); // 找到第一個括號的位置
        if (index >= 0)
        {
            return input.Substring(0, index).Trim(); // 截取括號前的部分並移除多餘空格
        }
        return input; // 如果沒有括號，直接返回原字串
    }

    public string GetCurrentLanguage()
    {
        return LocalizationSettings.SelectedLocale.Identifier.Code;
    }

    //await LocalizationMgr.Instance.SetLanguageAsync("zh-TW"); // 切換到繁體中文
    //await LocalizationMgr.Instance.SetLanguageAsync("en-US"); // 切換到英文
    public async Task SetLanguageAsync(string languageCode)
    {
        // 獲取所有可用的 Locale
        var availableLocales = LocalizationSettings.AvailableLocales.Locales;

        // 根據語言代碼尋找對應的 Locale
        var targetLocale = availableLocales.FirstOrDefault(locale => locale.Identifier.Code == languageCode);

        if (targetLocale != null)
        {
            // 切換當前語言
            LocalizationSettings.SelectedLocale = targetLocale;
            Debug.Log($"語言已切換至：{targetLocale.LocaleName}");
        }
        else
        {
            Debug.LogError($"找不到對應的語言代碼：{languageCode}");
        }

        // Optional: 等待任何需要重新加載的本地化表完成
        await LocalizationSettings.InitializationOperation.Task;
    }

    //我的LocalizationMgr要有以下方法
    //1.輸入tableName跟key取單一表string
    //2.輸入tableName取單一表List<string>
    //3.輸入tableName跟int start, int end 取單一表List<string>依編號如1~20
}
