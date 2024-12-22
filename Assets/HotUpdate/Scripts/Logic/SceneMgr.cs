using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using static Consts;


public class SceneMgr : Singleton<SceneMgr>
{


    private AsyncOperationHandle<SceneInstance> loadedSceneHandle;
    private Dictionary<string,AsyncOperationHandle<SceneInstance>> loadedSceneHandleDict;


    public event Action<float, string> OnProgressChanged; // 進度更新事件
    public event Action<string, string> OnStatusChanged;  // 狀態更新事件
    public event Action<string, string> OnErrorOccurred;


    public  void Init()
    {
        //loadedSceneHandleDict = new();
        loadedSceneHandleDict = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
    }

    /// <summary>
    ///  使用  Addressables 加載場景時，Unity 會檢查場景所需的資源（例如場景中引用的圖片或其他資產）並自動加載這些資源。
    /// </summary>
    /// <param name="sceneKey"></param>
    public async Task LoadScene(string sceneKey, bool isActive = true)
    {
        //if (loadedSceneHandleDict.ContainsKey(sceneKey))
        //{
        //    if (isActive)
        //    {
        //        ActivateScene(sceneKey);
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"場景 {sceneKey} 已經加載。");
        //        return;
        //    }

        //}


        Debug.Log($"場景開始加載 {sceneKey}");
        // 加載場景，但不自動激活
        loadedSceneHandle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single, false);
        //loadedSceneHandleDict.Add(sceneKey, loadedSceneHandle);



        while (!loadedSceneHandle.IsDone)
        {
            //Debug.Log($"加載進度: {handle.PercentComplete * 100}%");
            // 觸發進度更新事件
            OnProgressChanged?.Invoke(loadedSceneHandle.PercentComplete, sceneKey);

            await Task.Yield();
        }

        if (loadedSceneHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"場景 {sceneKey} 加載成功，但尚未激活！");

            TriggerSceneStatusChanged("Succeeded", sceneKey);
            //OnStatusChanged?.Invoke("Succeeded", sceneKey);
            // 在這裡執行任何需要的初始化邏輯
            // 初始化完成後激活場景
            if (isActive)
            {
                ActivateScene();
            }
            
        }
        else
        {
            Debug.LogError($"場景 {sceneKey} 加載失敗！");
            OnErrorOccurred?.Invoke(sceneKey, "LoadFailed");
        }
    }
    public void ActivateScene()
    {
        if (loadedSceneHandle.Status == AsyncOperationStatus.Succeeded && loadedSceneHandle.IsValid() )
        {
            loadedSceneHandle.Result.ActivateAsync();

            Debug.Log($"場景 {loadedSceneHandle.Result.Scene.name} 已激活！");
        }
        else
        {
            Debug.LogError($"場景 {loadedSceneHandle.Result.Scene.name} 無法激活。可能尚未加載或加載失敗。");
        }
        //if (loadedSceneHandleDict.TryGetValue(sceneKey, out var handle) && handle.Status == AsyncOperationStatus.Succeeded)
        //{

        //}
    }

    public void ActivateScene(string sceneKey)
    {

        if (loadedSceneHandleDict.ContainsKey(sceneKey))
        {
            loadedSceneHandleDict[sceneKey].Result.ActivateAsync();

            Debug.Log($"場景 {sceneKey} 已激活！");
        }
        else
        {
            Debug.LogError($"場景 {sceneKey} 無法激活。可能尚未加載或加載失敗。");
        }
        //if (loadedSceneHandleDict.TryGetValue(sceneKey, out var handle) && handle.Status == AsyncOperationStatus.Succeeded)
        //{

        //}
    }



    /// <summary>
    /// 使用 Addressables.UnloadSceneAsync 卸載場景時，Unity 會自動卸載與該場景相關聯的依賴資源，但前提是這些資源未被其他地方引用。
    /// </summary>
    public async Task UnloadScene(string sceneKey)
    {
        if (loadedSceneHandleDict.TryGetValue(sceneKey, out var handle) && handle.IsValid())
        {
            Debug.Log($"正在卸載場景：{sceneKey}");
            var unloadHandle = Addressables.UnloadSceneAsync(handle);
            await unloadHandle.Task;

            if (unloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"場景 {sceneKey} 卸載成功！");
                loadedSceneHandleDict.Remove(sceneKey);
                TriggerSceneStatusChanged("Unloaded", sceneKey);
            }
            else
            {
                Debug.LogError($"場景 {sceneKey} 卸載失敗！");
                OnErrorOccurred?.Invoke(sceneKey, "UnloadFailed");
            }
        }
        else
        {
            Debug.LogError($"場景 {sceneKey} 無效或未加載，無法卸載！");
        }
    }

    private void TriggerSceneStatusChanged(string status, string sceneKey)
    {
        OnStatusChanged?.Invoke(status, sceneKey);
    }



}
