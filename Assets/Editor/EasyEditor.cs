using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;

using UnityEngine.UI;

public class EasyEditor : Editor
{
    //快速返回啟動介面
    [MenuItem("Custom/GoToEntry")]
    public static void GoToEntry()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/LocalDate/Scenes/Entry.unity");
    }

    [MenuItem("Custom/GoToLobby")]
    public static void GoToLobby()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/HotUpdate/Scenes/Lobby.unity");
    }
   
    [MenuItem("Custom/GoToGame1View")]
    public static void GoToGame1()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/HotUpdate/Scenes/Game1View.unity");
    }

    [MenuItem("Custom/GoToGame2View")]
    public static void GoToGame2()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/HotUpdate/Scenes/Game2View.unity");
    }



    ////把配置文件放進Resources內
    //[MenuItem("Custom/ConfigToResources")]
    //public static void ConfigToResources()
    //{
    //    //Debug.Log("ConfigToResources");
    //    var srcPath = Application.dataPath + "/../Config/";
    //    var detPath = Application.dataPath + "/Resources/Config/";

    //    //清空目錄重建
    //    if (File.Exists(srcPath))
    //    {
    //        Directory.Delete(detPath, true);
    //        Directory.CreateDirectory(detPath);
    //    }


    //    //複製檔案
    //    foreach (var filePath in Directory.GetFiles(srcPath))
    //    {
    //        string fileName = Path.GetFileName(filePath);

    //        File.Copy(filePath, detPath + fileName + ".bytes", true);
    //    }
    //    Debug.Log("配置文件複製完畢");
    //}

}
