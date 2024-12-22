using UnityEditor;
using UnityEngine;
using System.IO;

public class SimpleHybridCLRDLLTool : EditorWindow
{
    // 相對於 Unity 項目根目錄的源 DLL 路徑
    //private const string SourcePath = "HybridCLRData/HotUpdateDlls/[BuildTarget]/HotUpdate.dll"; // 修改為你的相對路徑
    private static string SourcePath => $"HybridCLRData/HotUpdateDlls/{GetBuildTargetName()}/HotUpdate.dll";
    private const string TargetDirectory = "Assets/HotUpdate/Generated/Dlls/";

    private const string TargetFileName = "HotUpdate.dll.bytes";

    [MenuItem("Tools/Run HybridCLR DLL Mover")]
    public static void MoveDLL()
    {
        try
        {
            // 獲取完整源路徑
            string fullSourcePath = Path.Combine(Application.dataPath, "../", SourcePath);

            // 確保源文件存在
            if (!File.Exists(fullSourcePath))
            {
                Debug.LogError($"未找到 DLL 文件：{fullSourcePath}");
                return;
            }

            // 確保目標目錄存在
            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
                Debug.Log($"創建目標目錄：{TargetDirectory}");
            }

            // 構建目標路徑
            string targetPath = Path.Combine(TargetDirectory, TargetFileName);

            // 複製並重命名 DLL
            File.Copy(fullSourcePath, targetPath, true);
            Debug.Log($"DLL 文件已成功移動並重命名為：{targetPath}");

            // 刷新資源
            AssetDatabase.Refresh();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"移動或重命名 DLL 文件時發生錯誤：{ex.Message}");
        }
    }

    // 獲取當前構建目標的名稱
    private static string GetBuildTargetName()
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "StandaloneWindows64";
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            default:
                Debug.LogWarning("未處理的構建目標，請確認配置！");
                return "UnknownPlatform";
        }
    }
}
