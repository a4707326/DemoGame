using Firebase;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AudioMgr : MonoSingleton<AudioMgr>
{
    [Header("Audio Sources")]
    public AudioSource musicSource;  // 用於播放背景音樂
    public AudioSource sfxSource;   // 用於播放音效

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();  // 背景音樂字典
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();    // 音效字典

    /// <summary>
    /// 初始化音樂和音效字典，從 Addressables 中加載。
    /// </summary>
    public async Task Init()
    {
        musicSource = gameObject.AddComponent<AudioSource>(); // 動態添加 AudioSource
        sfxSource = gameObject.AddComponent<AudioSource>(); // 動態添加 AudioSource

        AudioClip clip = await ResMgr.Instance.GetResAsync<AudioClip>(Consts.MusicKey.Music01);

        musicDict.Add(Consts.MusicKey.Music01, clip);


   
        SetMusicVolume(LocalDataMgr.Instance.SettingsData.MusicVolume);



        

        //await musicDict = GetKeysByLabel("Music");
        //await sfxDict = GetKeysByLabel("SFX");

        //// 加載背景音樂
        //foreach (var key in musicKeys)
        //{
        //    await LoadAudioClip(key, musicDict);
        //}

        //// 加載音效
        //foreach (var key in sfxKeys)
        //{
        //    await LoadAudioClip(key, sfxDict);
        //}
    }

    /// <summary>
    /// 播放背景音樂
    /// </summary>
    public void PlayMusic(string musicName, bool loop = true)
    {
        if (musicDict.TryGetValue(musicName, out var clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"背景音樂 {musicName} 不存在！");
        }
    }

    /// <summary>
    /// 停止播放背景音樂
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// 暫停或恢復背景音樂
    /// </summary>
    public void ToggleMusicPause()
    {
        LocalDataMgr date = LocalDataMgr.Instance;

        if (date.SettingsData.MusicVolume == 0f)
        {
            date.SettingsData.MusicVolume = 1f;
        }
        else
        {
            date.SettingsData.MusicVolume = 0f;
        }
        date.SettingsData.SaveToPlayerPrefs();

        SetMusicVolume(date.SettingsData.MusicVolume);

        
        //if (musicSource.isPlaying)
        //{
        //    musicSource.Pause();
        //}
        //else
        //{
        //    musicSource.UnPause();
        //}
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySFX(string sfxName, float volume = 1f)
    {
        if (sfxDict.TryGetValue(sfxName, out var clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"音效 {sfxName} 不存在！");
        }
    }

    /// <summary>
    /// 設定背景音樂音量
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// 設定音效音量
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// 從 Addressables 加載音效並存入字典
    /// </summary>
    private async void LoadAudioClip(string key, Dictionary<string, AudioClip> targetDict)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(key);

        // 等待加載完成
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            targetDict[key] = handle.Result;
            Debug.Log($"成功加載音效：{key}");
        }
        else
        {
            Debug.LogError($"無法加載音效：{key}");
        }
    }
    /// <summary>
    /// 從 Addressables 獲取指定標籤的所有資源鍵值
    /// </summary>
    private async Task<List<string>> GetKeysByLabel(string label)
    {
        var keys = new List<string>();

        // 查詢具有指定標籤的資源
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(label);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var location in handle.Result)
            {
                keys.Add(location.PrimaryKey); // 獲取資源的 PrimaryKey
            }

            Debug.Log($"成功取得標籤為 '{label}' 的資源鍵值，共 {keys.Count} 筆");
        }
        else
        {
            Debug.LogError($"查詢標籤 '{label}' 的資源時發生錯誤");
        }

        return keys;
    }
}
