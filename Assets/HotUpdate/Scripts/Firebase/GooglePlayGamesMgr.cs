// Google Play Games 與 Firebase 整合的管理程式
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System;

public class GooglePlayGamesMgr : Singleton<GooglePlayGamesMgr>
{
    //private FirebaseAuth auth;


    public void Init()
    {

    }


    public void Login(Action<string> onSuccess, Action<string> onError = null)
    {

        DebugTools.LogMethodName();

        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.LogWarning("Google Play Games 僅能在 Android 平台上運行。");
            return;
        }

        PlayGamesPlatform.Activate();




        // 嘗試登錄 Google Play Games
        PlayGamesPlatform.Instance.Authenticate(status =>
        {
            if (status == SignInStatus.Success)
            {
                Debug.Log("Google Play Games 登錄成功");

                // 請求伺服器端憑證以整合 Firebase
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, authCode =>
                {
                    if (!string.IsNullOrEmpty(authCode))
                    {                    
                        Debug.Log($"取得 Auth Code: {authCode}");
                        onSuccess?.Invoke(authCode);

                    }
                    else
                    {
                        Debug.LogError("無法取得 Auth Code");
                        onError?.Invoke(authCode);
                    }
                });
            }
            else
            {
                Debug.LogError($"Google Play Games 登錄失敗：{status}");
                onError?.Invoke($"Google Play Games 登錄失敗：{status}");
            }
        });
    }

    //public void Logout()
    //{
    //    if (auth != null && auth.CurrentUser != null)
    //    {
    //        auth.SignOut();
    //        Debug.Log("Firebase 登出成功");
    //    }

    //    Debug.Log("Google Play Games 平台目前不支援完整登出，僅清除本地憑證。您可以要求玩家重新登入以重新驗證。");
    //}


    public void ShowAchievementsUI()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void ShowLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}
