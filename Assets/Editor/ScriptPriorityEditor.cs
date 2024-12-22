using UnityEditor;
using UnityEngine;

public class ScriptPriorityEditor : EditorWindow
{
    private static bool useDLLScripts = false; // 優先使用 DLL 的標誌
    private const string DllPath = "Assets/HotUpdate/Dlls/HotUpdate.dll.bytes"; // DLL 路徑

    [MenuItem("Tools/Script Priority Manager")]
    public static void ShowWindow()
    {
        var window = GetWindow<ScriptPriorityEditor>("Script Priority Manager");
        window.minSize = new Vector2(300, 150);
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Priority Settings", EditorStyles.boldLabel);

        // 切換選項
        useDLLScripts = EditorGUILayout.Toggle("優先使用 DLL 腳本", useDLLScripts);

        GUILayout.Space(10);

        if (GUILayout.Button("應用設置", GUILayout.Height(30)))
        {
            ApplySettings();
        }

        GUILayout.Space(10);

        if (useDLLScripts)
        {
            EditorGUILayout.HelpBox("當前設置為優先使用 DLL 腳本，請確保 DLL 準備好。", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("當前設置為優先使用專案內腳本。", MessageType.Info);
        }
    }

    private void ApplySettings()
    {
        if (useDLLScripts)
        {
            // 設置 DLL 為優先腳本來源
            EnableDLLScripts();
        }
        else
        {
            // 恢復到專案內腳本
            DisableDLLScripts();
        }

        // 刷新資產資料庫
        AssetDatabase.Refresh();
    }

    private void EnableDLLScripts()
    {
        Debug.Log("切換到優先使用 DLL 腳本");
        // 假設 DLL 已經加載，此處可進一步添加檢查和驗證邏輯。
        // 例如動態加載 DLL 或設置相關的執行邏輯。
    }

    private void DisableDLLScripts()
    {
        Debug.Log("切換到優先使用專案內腳本");
        // 如果需要執行恢復邏輯，比如卸載 DLL 或啟用專案內腳本。
    }
}
