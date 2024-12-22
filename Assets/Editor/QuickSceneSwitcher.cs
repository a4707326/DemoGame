using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Linq;


public class QuickSceneSwitcher : EditorWindow
{
    private Vector2 scrollPos;
    private string[] scenes;

    [MenuItem("Tools/Quick Scene Switcher")]
    public static void OpenWindow()
    {
        GetWindow<QuickSceneSwitcher>("Quick Scene Switcher");
    }

    private void OnEnable()
    {
        RefreshScenes();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("快速場景切換", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("刷新場景列表"))
        {
            RefreshScenes();
        }

        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (scenes != null && scenes.Length > 0)
        {
            foreach (var scenePath in scenes)
            {
                if (GUILayout.Button(scenePath))
                {
                    OpenScene(scenePath);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("未找到場景，請添加場景到 Build Settings。");
        }

        EditorGUILayout.EndScrollView();
    }

    private void RefreshScenes()
    {
        scenes = AssetDatabase.FindAssets("t:Scene")
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .ToArray();
    }

    private void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            Close();
        }
    }
}
