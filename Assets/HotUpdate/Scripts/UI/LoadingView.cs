using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Consts;


public class LoadingView : MonoBehaviour
{
    // Start is called before the first frame update

    //private string prefabAddress = "Assets/Addressables/Characters/Enemy_1.prefab"; // 替換為你的資源地址
    private Slider progressBar; // 指向 UI 的進度條 (Slider)
    private TextMeshProUGUI progressText; // 指向顯示進度百分比的 TextMeshPro (TextMeshProUGUI)
    //bool isLastPreLoad = false;
    int finishedCount = 0;
    int finishedNeed = 2;


    async void Start()
    {
        progressText = transform.Find<TextMeshProUGUI>("progressText");

        progressText.text = "加載中";


        // 訂閱
        HotUpdateMgr.Instance.OnProgressChanged += RefreshProgress;
        HotUpdateMgr.Instance.OnStatusChanged += UpdateStatus;
        SceneMgr.Instance.OnProgressChanged += RefreshProgress;
        SceneMgr.Instance.OnStatusChanged += UpdateStatus;

        //取當前熱更版本號
        await HotUpdateMgr.Instance.LoadVersionInfo();


        finishedCount = 0;
        //判斷是否需要預載
        if (await HotUpdateMgr.Instance.CheckDownloadSize("All") > 0f)
        {
            await HotUpdateMgr.Instance.PreloadAssetsAndToRam("All");
        }
        else
        {
            finishedCount++;
        }

        await HotUpdateMgr.Instance.CheckDownloadSize("Scene");
        //await SceneMgr.Instance.LoadScene(AddressablesKeys.SceneKeys.Login_unity, false);
        await SceneMgr.Instance.LoadScene(SceneKey.LoginScene, false);
        //if (await HotUpdateMgr.Instance.CheckDownloadSize("Scene") > 0f)
        //{

        //    //isLastPreLoad = true;
        //    //await HotUpdateMgr.Instance.PreloadAssets("Scene"); 
        //    await SceneMgr.Instance.LoadScene(SceneKey.LoginScene, false);
        //    //await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene, false);
        //}
        //else
        //{
        //    finishedCount += 1;
        //}


        if (finishedCount >= finishedNeed)
        {
            progressText.text = $"加載場景中";
            //await Task.Delay(15 * 1000); // 停頓一秒

            await HotUpdateMgr.Instance.CheckDownloadSize("Scene");
            SceneMgr.Instance.ActivateScene();
            //await SceneMgr.Instance.LoadScene(SceneKey.LoginScene);
        }





    }

    private void RefreshProgress(float progress,string label)
    {
        //// 更新進度條 UI
        //progressBar.value = progress; 
        progressText.text = $"{label} 加載進度: {progress * 100}%";
    }
    private void UpdateStatus(string Status, string label)
    {
        if (Status == "Succeeded" )
        {
            //Debug.Log($"{label} 預加載完成");
            finishedCount++;
        }
    }

    private void OnDestroy()
    {

        HotUpdateMgr.Instance.OnProgressChanged -= RefreshProgress;
        HotUpdateMgr.Instance.OnStatusChanged -= UpdateStatus;
        SceneMgr.Instance.OnProgressChanged -= RefreshProgress;
        SceneMgr.Instance.OnStatusChanged -= UpdateStatus;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
