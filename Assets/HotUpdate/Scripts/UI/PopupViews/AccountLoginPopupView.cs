using System;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountLoginPopupView : PopupView, IPopup
{


    // 帳號輸入框
    [SerializeField]
    private TMP_InputField account_IF;

    // 密碼輸入框
    [SerializeField]
    private TMP_InputField password_IF;

    // 登入按鈕
    [SerializeField]
    private Button login_Btn;

    // 註冊按鈕
    [SerializeField]
    private Button register_Btn;



    public override void Init(object data)
    {
        base.Init(data);
        login_Btn.onClick.AddListener(OnLoginClick);
        register_Btn.onClick.AddListener(OnRegisterClick);


        account_IF.text = "a123456@aa.bb.com";
        password_IF.text = "a123456";
    }

    //Action<FirebaseUser> onSuccess, Action<string> onError)
    private void OnLoginClick()
    {
        Debug.Log("OnLoginClick");

        FirebaseMgr.Instance.LoginWithEmail(account_IF.text, password_IF.text,
            onSuccess: (user) =>
            {
                // 從Firebase讀取
                FirebaseMgr.Instance.ReadJsonData(
                    path: $"{Consts.FirebaseKey.Players}{user.UserId}",
                    onSuccess: async (jsonData) =>
                    {
                        LocalDataMgr.Instance.PlayerData.FromJson(jsonData);
                        LocalDataMgr.Instance.PlayerData.Show();
                        Debug.Log($"讀取成功，玩家暱稱：{LocalDataMgr.Instance.PlayerData.Nickname}");
                        await SceneMgr.Instance.LoadScene(Consts.SceneKey.LobbyScene);

                    }
                );
            },
            onError: (error) =>
            { 
            }
        );


  

    }

    private void OnRegisterClick()
    {
        Debug.Log("OnRegisterClick");


        FirebaseMgr.Instance.RegisterWithEmail
        (
            account_IF.text,
            password_IF.text,
            onSuccess: (user) =>
            {
                PlayerData playerData = new PlayerData
                {
                    UserId = user.UserId,
                    Nickname = Tools.GenRandomNickname(),
                    Account = account_IF.text,
                    Password = password_IF.text,
                };

                // 寫入到 Firebase
                FirebaseMgr.Instance.UpdateJsonData(
                    path: $"{Consts.FirebaseKey.Players}{playerData.UserId}", // 路徑：Players/123456
                    jsonData: playerData.ToJson(),
                    onSuccess: () => 
                    {
                        Debug.Log("資料寫入成功！");
                        // 從Firebase讀取
                        FirebaseMgr.Instance.ReadJsonData(
                            path: $"Players/{playerData.UserId}",
                            onSuccess: async (jsonData) =>
                            {
                                LocalDataMgr.Instance.PlayerData.FromJson(jsonData);
                                LocalDataMgr.Instance.PlayerData.Show();
                                Debug.Log($"讀取成功，玩家暱稱：{LocalDataMgr.Instance.PlayerData.Nickname}");
                                await SceneMgr.Instance.LoadScene(Consts.SceneKey.LobbyScene);

                            }
                        );
                    }
                );
            }
        );
    }

    internal override void OnDestroy()
    {
        base.OnDestroy();
        login_Btn.onClick.RemoveAllListeners();
        register_Btn.onClick.RemoveAllListeners();
    }

    //private void Awake()
    //{
    //    //元件初始化
    //    Init(null);
    //}

}
