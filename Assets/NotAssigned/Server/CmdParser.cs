using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//未分模塊的消息解析器
public class CmdParser
{
    public static void OnLogin(Cmd cmd)
    {
        Debug.Log("S_OnLogin");

        //檢查協議正確性
        if (!CheckCmd(cmd, typeof(LoginCmd))) return;

        //驗證
        //找到玩家的資料
        Server.Instance.curPlayer = new Player();
        var player = Server.Instance.curPlayer;

        //向客戶端發送消息
        Server.Instance.SendCmd(new LoginSuccessCmd() {userID = player.userID ,name = player.name ,money = player.money ,avatarID = player.avatarID});
    }


    //檢查協議正確性
    public static bool CheckCmd(Cmd cmd, Type targetType)
    {
        if (cmd.GetType() != targetType)
        {
            Debug.LogError($"S_需要{targetType}但收到{cmd.GetType()}");
            return false;
        }
        return true;
    }
}

