using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

//產生版本version.json使用
public class AddressablesVersionIncrementor
{
    private const string VersionFilePath = "Assets/HotUpdate/Generated/version.json"; // version.json 的路徑
    private const string AddressablesBuildPath = "Library/com.unity.addressables/aa/"; // Addressables 資源存放的目錄

    [MenuItem("Tools/Addressables/Build with Version Update")]
    public async static void BuildAddressablesWithVersionUpdate()
    {
        // 1. 獲取 Addressables 的構建路徑
        string buildPath = GetBuildPath();
        // 2. 清空目標目錄
        ClearBuildDirectory(buildPath);

        // 3. 更新版本號與哈希值
        await UpdateVersionFile();

        // 4. 移動Dll
        SimpleHybridCLRDLLTool.MoveDLL();

        //5. 清空 Addressables 的構建
        AddressableAssetSettings.CleanPlayerContent();

        // //4. 執行 Addressables 的構建
        //AddressableAssetSettings.BuildPlayerContent();


    }

    private async static Task UpdateVersionFile()
    {
        // 確保 version.json 存在
        if (!File.Exists(VersionFilePath))
        {
            CreateInitialVersionFile();
        }

        // 讀取現有版本號
        string jsonContent = File.ReadAllText(VersionFilePath);
        VersionInfo versionInfo = JsonUtility.FromJson<VersionInfo>(jsonContent);

        // 更新版本號
        IncrementVersion(versionInfo);

        // 計算 Addressables 資源的哈希值
        string hash = "X";
        //string hash = CalculateAddressablesHash();
        if (!string.IsNullOrEmpty(hash))
        {
            versionInfo.Hash = hash;
        }

        await Task.Run(() =>
        {
            // 保存更新後的 version.json
            string updatedJsonContent = JsonUtility.ToJson(versionInfo, true);
            File.WriteAllText(VersionFilePath, updatedJsonContent);
        });


        //Debug.Log($"Addressables 構建完成，版本號已更新至: {versionInfo.Version}，哈希值: {versionInfo.Hash}");
        Debug.Log($"版本號已更新至: {versionInfo.Version}，哈希值: {versionInfo.Hash} ");
    }

    private static void CreateInitialVersionFile()
    {
        var initialVersion = new VersionInfo
        {
            Version = "1.0.0",
            Build_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Hash = "",
            Notes = "Initial version"
        };

        string jsonContent = JsonUtility.ToJson(initialVersion, true);
        File.WriteAllText(VersionFilePath, jsonContent);
        Debug.Log("已創建初始版本文件: " + VersionFilePath);
    }

    private static void IncrementVersion(VersionInfo versionInfo)
    {
        string[] parts = versionInfo.Version.Split('.');
        if (parts.Length == 3 && int.TryParse(parts[2], out int patchNumber))
        {
            patchNumber++;
            versionInfo.Version = $"{parts[0]}.{parts[1]}.{patchNumber}";
        }
        else
        {
            Debug.LogError("無法解析版本號，將重置為 1.0.0");
            versionInfo.Version = "1.0.0";
        }

        versionInfo.Build_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        versionInfo.Notes = "Auto-incremented version during Addressables build";
    }

    private static string CalculateAddressablesHash()
    {
        try
        {
            if (Directory.Exists(AddressablesBuildPath))
            {
  
                StringBuilder hashBuilder = new StringBuilder();
                string[] files = Directory.GetFiles(AddressablesBuildPath, "*", SearchOption.AllDirectories);

                using (MD5 md5 = MD5.Create())
                {
                    foreach (string file in files)
                    {
                        byte[] fileData = File.ReadAllBytes(file);
                        byte[] fileHash = md5.ComputeHash(fileData);
                        hashBuilder.Append(BitConverter.ToString(fileHash).Replace("-", "").ToLowerInvariant());
                    }
                }

                using (MD5 finalMd5 = MD5.Create())
                {
                    byte[] overallHash = finalMd5.ComputeHash(Encoding.UTF8.GetBytes(hashBuilder.ToString()));
                    return BitConverter.ToString(overallHash).Replace("-", "").ToLowerInvariant();
                }
            }
            else
            {
                Debug.LogError($"Addressables 目錄未找到: {AddressablesBuildPath}");
                return string.Empty;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"計算 Addressables 哈希值失敗: {ex.Message}");
            return string.Empty;
        }
    }

    private static string GetBuildPath()
    {
        // 使用 Addressables 設定中的 Remote.BuildPath
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        var profileSettings = settings.profileSettings;
        string activeProfileId = settings.activeProfileId;
        string buildPath = profileSettings.GetValueByName(activeProfileId, "Remote.BuildPath");

        // 替換 [BuildTarget] 為當前目標平台
        buildPath = buildPath.Replace("[BuildTarget]", EditorUserBuildSettings.activeBuildTarget.ToString());
        return Path.GetFullPath(buildPath); // 獲取絕對路徑
    }

    private static void ClearBuildDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            try
            {
                // 刪除目錄內的所有檔案和子目錄
                Directory.Delete(path, true);
                Debug.Log($"已清空目標目錄：{path}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"清空目標目錄時發生錯誤：{ex.Message}");
            }
        }
        else
        {
            Debug.Log($"目標目錄不存在：{path}，無需清理。");
        }

        // 確保目錄存在
        Directory.CreateDirectory(path);
    }
}
