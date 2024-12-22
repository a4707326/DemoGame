using TMPro; // 引入 TextMeshPro 命名空間
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.IO;
using System;

public class ResourceLoaderWithProgress : MonoBehaviour
{
    public string prefabAddress = "Assets/Addressables/Characters/Enemy_1.prefab"; // 替換為你的資源地址
    public Slider progressBar; // 指向 UI 的進度條 (Slider)
    public TextMeshProUGUI progressText; // 指向顯示進度百分比的 TextMeshPro (TextMeshProUGUI)

    void Start()
    {
        //Caching.ClearCache();
        
        LoadRemotePrefabWithProgress();
    }

    void LoadRemotePrefabWithProgress()
    {
        // 開始加載 Addressable 資源
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);

        // 啟動協程更新進度條
        StartCoroutine(UpdateProgress(handle));

        // 加載完成時執行
        handle.Completed += (operation) =>
        {
            
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                // 加載成功，生成實例
                Instantiate(operation.Result, Vector3.zero, Quaternion.identity);
                Debug.Log("熱資源加載成功！");
                progressText.text = "資源加載成功";

                // 使用 Addressables 加載 DLL 文件
                Addressables.LoadAssetAsync<TextAsset>("Assets/HotUpdate/Dlls/HotUpdate.dll.bytes").Completed += OnHotUpdateDllLoaded;


                //SceneManager.LoadScene("Login");
            }
            else
            {
                // 加載失敗
                Debug.LogError($"資源加載失敗：{operation.OperationException.Message}");
                progressText.text = "資源加載失敗";
            }

            // 隱藏進度條
            //if (progressBar != null) progressBar.gameObject.SetActive(false);
            //if (progressText != null) progressText.gameObject.SetActive(false);
        };
    }

    private void OnHotUpdateDllLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("成功加載 HotUpdate.dll.bytes 文件");
            
            Assembly hotUpdateAss = Assembly.Load(handle.Result.bytes);

            Type type = hotUpdateAss.GetType("Hello");
            type.GetMethod("Run").Invoke(null, null);

            //// 取得 DLL 的字節數據
            //byte[] dllBytes = handle.Result.bytes;

            //try
            //{
            //    // 加載 Assembly
            //    Assembly hotUpdateAssembly = Assembly.Load(dllBytes);
            //    Debug.Log("Assembly 加載成功");

            //    // 測試調用 Assembly 中的類和方法
            //    InvokeHotUpdateEntry(hotUpdateAssembly);
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogError($"加載 Assembly 失敗: {e}");
            //}
        }
        else
        {
            Debug.LogError("加載 HotUpdate.dll.bytes 失敗: " + handle.Status);
        }
    }

    System.Collections.IEnumerator UpdateProgress(AsyncOperationHandle<GameObject> handle)
    {
        // 在資源加載過程中更新進度
        while (!handle.IsDone)
        {
            float progress = handle.PercentComplete; // 獲取加載進度
            Debug.Log("熱更新資源加載中"+ progress );
            if (progressBar != null)
            {
                progressBar.value = progress; // 設置進度條值
            }
            if (progressText != null)
            {
                progressText.text =  $"熱更新資源加載中 : {(progress * 100):0}%"; // 使用 TextMeshPro 顯示進度文字
            }
            yield return null;
        }
    }
}
