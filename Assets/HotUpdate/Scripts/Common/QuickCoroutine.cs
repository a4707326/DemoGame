using System;
using System.Collections;
using UnityEngine;

public class QuickCoroutine : Singleton<QuickCoroutine>
{
    private GameObject root;
    private QuickCoroutineHandler coroutineHandler;

    // 初始化方法
    public void Init()
    {
        if (root == null)
        {
            root = new GameObject("QuickCoroutine");
            GameObject.DontDestroyOnLoad(root); // 確保物件在場景切換時不被銷毀
            coroutineHandler = root.AddComponent<QuickCoroutineHandler>(); // 添加具體處理腳本
        }
    }

    public Coroutine StartCorontine(IEnumerator routine)
    {
        if (coroutineHandler == null)
        {
            Debug.LogError("QuickCoroutine has not been initialized. Call Init() first.");
            return null;
        }
        return coroutineHandler.StartCoroutine(routine);
    }

    internal void StartCorontine()
    {
        throw new NotImplementedException();
    }
}

// 具體用來處理 Coroutine 的 MonoBehaviour
public class QuickCoroutineHandler : MonoBehaviour
{
    // 可擴展其他功能
}
