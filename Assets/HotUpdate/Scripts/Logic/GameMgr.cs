using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Reflection;
using UnityEngine.Windows;
using Timers;

//控管遊戲整體流程
public class GameMgr : Singleton<GameMgr>
{

    public static void Entry() 
    {
        Instance.Init();
    }

    public async void Init()
    {

        //熱更資源管理器初始化
        UnityMainThreadDispatcher.Instance.Init();
        QuickCoroutine.Instance.Init();
        HotUpdateMgr.Instance.Init();
        TimersMgr.Instance.Init();
        LocalDataMgr.Instance.Init();
        FirebaseMgr.Instance.Init();
        SceneMgr.Instance.Init();
        PopupMgr.Instance.Init();
        await AudioMgr.Instance.Init();
        await LocalizationMgr.Instance.Init();


        //進入載入腳本
        GameObject.Find("Loading").AddComponent<LoadingView>();

    }
}

