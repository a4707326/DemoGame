#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public static class AddressablesKeyGenerator
{
    private const string OutputFilePath = "Assets/HotUpdate/Generated/AddressablesKeys.cs";

    [MenuItem("Tools/Addressables/Generate Addressables Keys")]
    public static void GenerateKeys()
    {
        GenerateKeysInternal();
    }

    // 註冊 Addressables Build 完成後的回調
    [InitializeOnLoadMethod]
    private static void RegisterBuildCallback()
    {
        // 如果當前進入 Play Mode 或即將進入 Play Mode，則不執行
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        BuildScript.buildCompleted -= OnBuildCompleted; // 防止重複註冊
        BuildScript.buildCompleted += OnBuildCompleted;
        Debug.Log("AddressablesKeyGenerator: Registered OnBuildCompleted callback.");
    }

    private static void OnBuildCompleted(AddressableAssetBuildResult result)
    {
        if (result == null || !string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError($"Addressables build failed: {result?.Error}");
            return;
        }

        Debug.Log("Addressables build succeeded. Generating Addressables Keys...");
        GenerateKeysInternal();
    }

    private static void GenerateKeysInternal()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("Addressable Asset Settings is null. Please check your Addressables configuration.");
            return;
        }

        // 提取 Group 和資源
        var groupedKeys = settings.groups
            .Where(group => group.entries.Any()) // 過濾掉空的 Group
            .ToDictionary(
                group => FormatClassName(group.Name),
                group => GetAllEntriesRecursively(group)
            );

        GenerateClassFile(groupedKeys);
    }

    private static void GenerateClassFile(Dictionary<string, List<KeyValuePair<string, string>>> groupedKeys)
    {
        using var writer = new StreamWriter(OutputFilePath);

        writer.WriteLine("// 自動生成的常量類，請勿手動修改");
        writer.WriteLine("public static class AddressablesKeys");
        writer.WriteLine("{");

        foreach (var group in groupedKeys)
        {
            writer.WriteLine($"    public static class {group.Key}Keys");
            writer.WriteLine("    {");

            foreach (var key in group.Value)
            {
                writer.WriteLine($"        public const string {key.Key} = \"{key.Value}\";");
            }

            writer.WriteLine("    }");
        }

        writer.WriteLine("}");
        AssetDatabase.Refresh();
        Debug.Log($"AddressablesKeys.cs successfully generated at: {OutputFilePath}");
    }

    private static List<KeyValuePair<string, string>> GetAllEntriesRecursively(AddressableAssetGroup group)
    {
        var allEntries = new List<KeyValuePair<string, string>>();

        foreach (var entry in group.entries)
        {
            // 遞迴處理主資源和所有子資源
            var gatheredAssets = new List<AddressableAssetEntry>();
            entry.GatherAllAssets(gatheredAssets, includeSelf: true, recurseAll: true, includeSubObjects: true);

            foreach (var asset in gatheredAssets)
            {
                allEntries.Add(new KeyValuePair<string, string>(
                    GenerateKeyFromAddress(asset.address), asset.address));
            }
        }

        return allEntries;
    }

    private static string GenerateKeyFromAddress(string address)
    {
        string fileNameWithExtension = Path.GetFileName(address); // 提取文件名（包含擴展名）
        string formatted = FormatKeyName(fileNameWithExtension);
        return formatted;
    }

    private static string FormatKeyName(string key)
    {
        string formatted = Regex.Replace(key, @"[^\w]", "_"); // 替換非法字符為 "_"
        if (char.IsDigit(formatted[0])) formatted = $"Key_{formatted}"; // 如果首字符是數字，添加前綴
        return formatted;
    }

    private static string FormatClassName(string className)
    {
        return Regex.Replace(className, @"[^\w]", ""); // 清理非法字符
    }
}
#endif