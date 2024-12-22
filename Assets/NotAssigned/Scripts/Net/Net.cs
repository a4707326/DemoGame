using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IClinet
{
    //void Connect();
    void SendCmd(Cmd cmd);
    void Recive(Cmd cmd);
}



//客戶端訪問服務器
public class Net : Singleton<Net>, IClinet
{
    IServer server;

    //分發解析器
    public Dictionary<Type, Action<Cmd>> parserDict = new();


    List<Cmd> cacheList = new();
    private bool pause;
    public bool Pause 
    {
        get { return pause; }
        set 
        {
            pause = value;
            if(value == false) 
            {
                Recive(null);
            }
        }
    }

    public Net()
    {
        //註冊要分發的消息
        //parserDict.Add(typeof(IsLoginSuccess), Login.instance.LoginSuccess);
    }


    public void Connect(Action successCallback,Action failedCallBack )
    {
        Debug.Log("開始服務器連接");

        //給server附值
        server = Server.Instance;
        server.Connected(this);


        //這裡開發階段只判定成功
        if ( true )
        {
            if (successCallback != null)
            {
                successCallback();
            }
        }
        else
        {
            if (failedCallBack != null)
            {
                failedCallBack();
            }
        }
       
    }

    public void Recive(Cmd cmd)
    {
        if(cmd != null)
        {
            Debug.Log("客戶端收到消息" + cmd.GetType());
            cacheList.Add(cmd);
        }

        if (Pause) return;

        foreach (var cacheCmd in cacheList)
        {
            //Action<Cmd> func;
            //if (parserDict.TryGetValue(cacheCmd.GetType(), out func))
            //{
            //    if(func != null)
            //    {
            //        func(cacheCmd);
            //    }
            //    else
            //    {

            //    }
            //}

            //用消息解析器去分發任務
            if (parserDict.ContainsKey(cacheCmd.GetType()))
            {
                Debug.Log($"已分發消息 {cacheCmd.GetType()}");
                parserDict[cacheCmd.GetType()](cacheCmd);
            }
            else
            {
                Debug.Log($"未分發消息 {cacheCmd.GetType()}");
            }
        }

        cacheList.Clear();

        ////用消息解析器去分發任務
        //if (parserDict.ContainsKey(cmd.GetType()))
        //{
        //    Debug.Log($"已分發消息 {cmd.GetType()}");
        //    parserDict[cmd.GetType()](cmd);
        //}
        //else
        //{
        //    Debug.Log($"未分發消息 {cmd.GetType()}");
        //}
    }

    public void SendCmd(Cmd cmd)
    {
        server.Recive(cmd);
    }

    //檢查協議正確性
    public static bool CheckCmd(Cmd cmd,Type targetType)
    {
        if (cmd.GetType() != targetType)
        {
            Debug.LogError($"S_需要{targetType}但收到{cmd.GetType()}");
            return false;
        }
        return true;
    }

   
}
