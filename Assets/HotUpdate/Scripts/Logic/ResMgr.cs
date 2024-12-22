using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnityEngine.ResourceManagement.AsyncOperations;

// 資源管理器，加載方式和使用邏輯分離
// 支持：熱更新與對象池
public class ResMgr : Singleton<ResMgr>
{
    // 非同步加載 GameObject 實例
    public async Task<GameObject> GetInstanceAsync(string key)
    {
        var prefab = await GetResAsync<GameObject>(key);
        if (prefab != null)
            return GameObject.Instantiate(prefab);
        Debug.LogError($"Failed to load prefab at {key}");
        return null;
    }

    // 非同步加載資源
    public async Task<T> GetResAsync<T>(string key) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;

        Debug.LogError($"Failed to load resource at {key} with type {typeof(T)}");
        return null;
    }

    // 同步加載（適合特殊情況，如 Addressables 已經緩存的資源）
    public T GetRes<T>(string key) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        handle.WaitForCompletion(); // 強制等待加載完成
        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;

        Debug.LogError($"Failed to load resource at {key} with type {typeof(T)}");
        return null;
    }

    // 加載 Sprite 圖集中的單個 Sprite
    public async Task<Sprite> GetSpriteAsync(string atlasName, string spriteName)
    {
        var atlas = await GetResAsync<SpriteAtlas>("Atlas/" + atlasName);
        if (atlas != null)
            return atlas.GetSprite(spriteName);

        Debug.LogError($"Failed to load sprite atlas: {atlasName}");
        return null;
    }
}
