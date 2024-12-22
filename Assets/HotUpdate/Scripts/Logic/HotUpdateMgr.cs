using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class HotUpdateMgr : MonoSingleton<HotUpdateMgr>
{
    private string curVersion = "1.0.0"; // 當前版本號
    private string curBuild_date;
    private string curHash;
    private string curNotes;

    private Assembly hotUpdateAssembly;
    private List<AsyncOperationHandle> preloadAssetList = new List<AsyncOperationHandle>();

    public event Action<float,string> OnProgressChanged; // 進度更新事件
    public event Action<string, string> OnStatusChanged;  // 狀態更新事件

    //private string remoteVersion = ""; // 線上版本號

    public void Init()
    {
        
        //Caching.ClearCache(); // 清除所有緩存

        //取當前熱更版本號
        //await LoadVersionInfo();
        //await CheckAndUpdateAddressables();
    }



    public async Task<VersionInfo> LoadVersionInfo()
    {
        // 定義 version.json 的鍵
        string versionJsonKey = "Assets/HotUpdate/version.json";

        try
        {
            // 透過 Addressables 讀取 version.json
            var handle = Addressables.LoadAssetAsync<TextAsset>(versionJsonKey);

            // 等待加載完成
            TextAsset versionJson = await handle.Task;

            if (versionJson != null)
            {
                Debug.Log($"成功讀取 version.json: {versionJson.text}");

               
                // 解析 JSON 並轉換為物件
                VersionInfo versionInfo = JsonUtility.FromJson<VersionInfo>(versionJson.text);

                curVersion = versionInfo.Version;
                curBuild_date = versionInfo.Build_date;
                curHash = versionInfo.Hash;
                curNotes = versionInfo.Notes;


                // 確保釋放句柄
                if (handle.IsValid()) Addressables.Release(handle);
                return versionInfo;

            }
            else
            {
                Debug.LogError("version.json 加載失敗！");
                // 確保釋放句柄
                if (handle.IsValid()) Addressables.Release(handle);
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"讀取 version.json 時發生錯誤: {ex.Message}");
            return null;
        }

    }

    public async Task<long> CheckDownloadSize(string label)
    {
        var sizeHandle = Addressables.GetDownloadSizeAsync(label);
        await sizeHandle.Task;

        if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($" {label} 下載所需大小（Bytes）:{sizeHandle.Result}");
            return sizeHandle.Result;
        }
        else
        {
            Debug.LogError("無法檢查下載大小");
            return -1;
        }
    }




    /// <summary>
    /// 預下載並加載資源到內存中
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public async Task PreloadAssetsAndToRam(string label)
    {
        Debug.Log($"開始預加載資源，Label: {label}");

        var handle = Addressables.LoadAssetsAsync<object>(label, null);

        while (!handle.IsDone)
        {
            //Debug.Log($"加載進度: {handle.PercentComplete * 100}%");
            // 觸發進度更新事件
            OnProgressChanged?.Invoke(handle.PercentComplete,label);

            await Task.Yield();
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            preloadAssetList.Add(handle);
            OnStatusChanged?.Invoke("Succeeded", label);
            Debug.Log($"{label}預加載成功");
        }
        else
        {
            Debug.LogError("預加載失敗");
        }
    }

    /// <summary>
    /// 預下載資源及其依賴，但不加載到內存。//用於加載場景會有問題//會有 Unable to load dependent bundle from location 問題暫不使用
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public async Task PreloadAssets(string label)
    {
        Debug.Log($"開始預加載資源，Label: {label}");
        var handle = Addressables.DownloadDependenciesAsync(label);


        while (!handle.IsDone)
        {
            //Debug.Log($"加載進度: {handle.PercentComplete * 100}%");
            // 觸發進度更新事件
            OnProgressChanged?.Invoke(handle.PercentComplete, label);

            await Task.Yield();
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            preloadAssetList.Add(handle);
            OnStatusChanged?.Invoke("Succeeded", label);
            Debug.Log($"{label}預加載成功");
        }
        else
        {
            Debug.LogError("預加載失敗");
        }
    }

    /// <summary>
    /// 清理所有預加載的資源//用於加載場景會有問題
    /// </summary>
    public void ReleasePreloadedAssets()
    {
        foreach (var handle in preloadAssetList)
        {
            Addressables.Release(handle);
        }
        preloadAssetList.Clear();
        Debug.Log("已釋放所有預加載的資源");
    }


    public async Task LoadHotUpdateDLL(string dllAddress)
    {
        try
        {
            // 使用 Addressables 加載 DLL
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(dllAddress);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var dllBytes = handle.Result.bytes;
                hotUpdateAssembly = Assembly.Load(dllBytes);

                Debug.Log($"成功加載熱更 DLL: {hotUpdateAssembly.FullName}");
            }
            else
            {
                Debug.LogError($"加載 DLL 資源失敗: {dllAddress}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"加載熱更 DLL 時發生錯誤: {ex.Message}");
        }
    }


}
