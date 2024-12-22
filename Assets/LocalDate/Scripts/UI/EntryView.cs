using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class EntryView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Caching.ClearCache(); // 清除所有緩存
        Addressables.ClearResourceLocators();
        Addressables.ClearDependencyCacheAsync("All");
        Addressables.ClearDependencyCacheAsync("Scene");
        Debug.Log("清除緩存完成！");

        //本地資源管理器初始化

        SceneManager.LoadScene("Loading");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
