using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;



public class PopupMgr : Singleton<PopupMgr>
{


    //// 彈窗的父物件 (可設定於 Canvas 下)
    //[SerializeField] private Transform popupParent;

    // 彈窗池，用於重複使用
    private Dictionary<string, Queue<GameObject>> popupPool = new();

    // 當前顯示的彈窗（以鍵值分類存儲）
    private Dictionary<string, List<GameObject>> activePopups = new();


    private Transform canvasTransform;

    public void Init()
    {

    }


    /// <summary>
    /// 從 Addressables 加載並顯示彈窗
    /// </summary>
    /// <param name="popupKey">Addressables 的鍵值</param>
    /// <param name="transform">父物件的Transform</param>
    /// <param name="data">初始化彈窗的資料</param>
    public async Task<GameObject> ShowPopup(string popupKey, object data = null, Transform transform = null)
    {
        GameObject popup = null;

        if (transform == null)
        {
            if (canvasTransform == null)
            {
                // 尋找場景中的 Canvas
                canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
            }

            transform = canvasTransform;
        }

        string popupName = Tools.GetResName(popupKey);

        // 從池中嘗試取出彈窗
        if (popupPool.ContainsKey(popupName) && popupPool[popupName].Count > 0)
        {
            popup = popupPool[popupName].Dequeue();
            popup.SetActive(true);
        }
        else
        {
            // 使用 Addressables 加載彈窗
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(popupKey);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                popup =  GameObject.Instantiate(handle.Result, transform, false);
            }
            else
            {
                Debug.LogError($"彈窗加載失敗，Key: {popupKey}");
                return null;
            }
        }

        // 初始化彈窗 (需要實現 IPopup 接口)
        IPopup popupLogic = popup.GetComponent<IPopup>();
        popupLogic?.Init(data);

        // 將彈窗加入活動列表
        if (!activePopups.ContainsKey(popupName))
        {
            activePopups[popupName] = new List<GameObject>();
        }
        activePopups[popupName].Add(popup);

        return popup;
    }

    /// <summary>
    /// 關閉所有彈窗
    /// </summary>
    public void CloseAllPopups()
    {
        foreach (var popupList in activePopups.Values)
        {
            foreach (var popup in popupList.ToArray())
            {
                ClosePopup(popup);
            }
        }

        activePopups.Clear();
    }

    /// <summary>
    /// 根據鍵值關閉彈窗
    /// </summary>
    /// <param name="popupKey">彈窗鍵值</param>
    public void ClosePopupsByKey(string popupKey)
    {
        string popupName = Tools.GetResName(popupKey);
        if (activePopups.ContainsKey(popupName))
        {
            foreach (var popup in activePopups[popupName].ToArray())
            {
                ClosePopup(popup);
            }

            activePopups.Remove(popupName);
        }
    }

    /// <summary>
    /// 關閉指定的彈窗
    /// </summary>
    /// <param name="popup">需要關閉的彈窗</param>
    public void ClosePopup(GameObject popup)
    {
        string popupName = popup.name.Replace("(Clone)", "").Trim();

        if (activePopups.ContainsKey(popupName))
        {
            activePopups[popupName].Remove(popup);
        }

        popup.SetActive(false);

        // 放回池中
        if (!popupPool.ContainsKey(popupName))
        {
            popupPool[popupName] = new Queue<GameObject>();
        }
        popupPool[popupName].Enqueue(popup);
    }
}


