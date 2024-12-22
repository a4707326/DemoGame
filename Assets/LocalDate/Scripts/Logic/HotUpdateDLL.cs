using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using HybridCLR;
using System.Collections.Generic;
using UnityEngine.Networking;

public class HotUpdateDLL : MonoBehaviour
{

    private  Assembly hotUpdateAssembly;


    private async void Start()
    {
        // Addressables 中 DLL 的鍵值（可以是名稱或路徑）
        string dllAddress = "Assets/HotUpdate/Generated/Dlls/HotUpdate.dll.bytes";

        //處理AOT問題
        await LoadMetadataForAOTAssemblies();

        // 加載熱更 DLL
        await LoadHotUpdateDLL(dllAddress);
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

                CallClassMethod("GameMgr", "Entry");
                //CallClassMethod("Loading", "Entry");
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
    /// <summary>
    /// 呼叫熱更程序集的Class的方法，這個方法需要using System.IO;
    /// </summary>
    /// <param name="className"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    private static void CallClassMethod(string className, string methodName)
    {


        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
        Type type = hotUpdateAss.GetType(className);
        type.GetMethod(methodName).Invoke(null, null);
    }

    //處理AOT問題
    private static async Task LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll.bytes",
            "System.dll.bytes",
            "System.Core.dll.bytes",
            // "Newtonsoft.Json.dll", 
            // "protobuf-net.dll",
        };

        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = await LoadBytesFromStreamingAssets(aotDllName);

            if (dllBytes != null)
            {
                int err = (int)HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
                Debug.Log($"LoadMetadataForAOTAssembly: {aotDllName}. ret: {err}");
            }
            else
            {
                Debug.LogError($"元數據檔案 {aotDllName} 無法加載！");
            }
        }
    }

    private static async Task<byte[]> LoadBytesFromStreamingAssets(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (filePath.StartsWith("jar:"))
        {
            // Android環境下的讀取方式
            UnityWebRequest request = UnityWebRequest.Get(filePath);

            // 等待請求完成
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.data;
            }
            else
            {
                Debug.LogError($"無法讀取檔案: {filePath}, 錯誤: {request.error}");
                return null;
            }
        }
        else
        {
            // 在非 Android 平台上，使用 File.ReadAllBytes
            return File.Exists(filePath) ? File.ReadAllBytes(filePath) : null;
        }
    }


}
