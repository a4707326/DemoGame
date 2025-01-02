
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Events;
using static Consts;


//控管遊戲整體流程
public class LoginMgr : Singleton<LoginMgr>
{


    public void Init()
    {

    }
    public void GoogleLogin()
    {
        GooglePlayGamesMgr.Instance.Login
        (
            onSuccess: (authCode) =>
            {
                // 使用 Firebase 的 PlayGamesAuthProvider 驗證
                Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
                FirebaseMgr.Instance.GetAuth().SignInWithCredentialAsync(credential).ContinueWith(async task =>
                {
                    if (task.IsCompleted && !task.IsFaulted)
                    {
                        Debug.Log($"Firebase(Google) 登錄成功 : {task.Result.UserId} + {task.Result.Email}");
                        string userId = task.Result.UserId;


                        bool isUserExists = await FirebaseMgr.Instance.IsUserExistsAsync(userId);


                        if (isUserExists) 
                        {
                            //登入
                            // 從Firebase讀取
                            UpdateLocalDate
                             (userId, action: async () =>
                             {
                                 await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                             }
                             );
                        }
                        else
                        {
                            //註冊
                            PlayerData playerData = new PlayerData
                            {
                                UserId = userId,
                                Nickname = Tools.GenRandomNickname(),
                                IsGoogleBound = true,
                            };
                            // 寫入到 Firebase
                            FirebaseMgr.Instance.UpdateJsonData(
                                path: $"{Consts.FirebaseKey.Players}/{playerData.UserId}", // 路徑：Players/123456
                                jsonData: playerData.ToJson(),
                                onSuccess: () =>
                                {
                                    UpdateLocalDate
                                    (playerData.UserId, action: async () =>
                                    {
                                        await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                                    }
                                    );
                                }
                            );
                        }
                    }
                    else
                    {
                        Debug.LogError($"Firebase 登錄失敗：{task.Exception}");
                    }
                });
            },
            onError: (error) => { }
        );





    }
    public void GuestLogin()
    {
        FirebaseMgr.Instance.GuestLogin
        (
            onSuccess: (user) =>
            {
                PlayerData playerData = new PlayerData
                {
                    UserId = user.UserId,
                    Nickname = Tools.GenRandomNickname()
                };

                // 寫入到 Firebase
                FirebaseMgr.Instance.UpdateJsonData
                (
                    path: $"{Consts.FirebaseKey.Players}/{playerData.UserId}", // 路徑：Players/123456
                    jsonData: playerData.ToJson(),
                    onSuccess: () =>
                    {
                        UpdateLocalDate
                        (user.UserId, action: async () =>
                        {
                            await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                        }
                        );
                    }
                );
            }
        );
    }
    public void AccountLogin(string account, string password)
    {
        FirebaseMgr.Instance.LoginWithEmail
        (account, password,
            onSuccess: (user) =>
            {
                UpdateLocalDate
                (user.UserId, action: async () =>
                {
                    await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                }
                );
            },
            onError: (error) =>
            {
            }
        );
    }
    public void AccountRegister(string account, string password)
    {
        FirebaseMgr.Instance.RegisterWithEmail
        (
            account,
            password,
            onSuccess: (user) =>
            {
                PlayerData playerData = new PlayerData
                {
                    UserId = user.UserId,
                    Nickname = Tools.GenRandomNickname(),
                    Account = account,
                    Password = password,
                };

                // 寫入到 Firebase
                FirebaseMgr.Instance.UpdateJsonData(
                    path: $"{Consts.FirebaseKey.Players}/{playerData.UserId}", // 路徑：Players/123456
                    jsonData: playerData.ToJson(),
                    onSuccess: () =>
                    {
                        UpdateLocalDate
                        (playerData.UserId, action: async () =>
                            {
                                await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                            }
                        );
                    }
                );
            }
        );
    }
    private void UpdateLocalDate(string userId, UnityAction action = null)
    {
        // 從Firebase讀取
        FirebaseMgr.Instance.ReadJsonData(
            path: $"{Consts.FirebaseKey.Players}/{userId}",
            onSuccess: (jsonData) =>
            {
                //把Firebase讀取讀取的資料放進本地資料
                LocalDataMgr.Instance.PlayerData.FromJson(jsonData);
                LocalDataMgr.Instance.PlayerData.Show();

                Debug.Log($"讀取成功，玩家暱稱：{LocalDataMgr.Instance.PlayerData.Nickname}");
                action?.Invoke();
            }
        );
    }




    /// <summary>
    /// 自動登入
    /// </summary>
    public void AutoLogin()
    {
        FirebaseMgr.Instance.AutoLogin
        (
            onSuccess: (user) =>
            {
                UpdateLocalDate
                (user.UserId, action: async () =>
                    {
                        await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                    }
                );

            }
        );
    }


}

