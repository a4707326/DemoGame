
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Consts;

public class LoginView : MonoBehaviour
{
    //[SerializeField]
    //private TMP_InputField account_IF;
    //[SerializeField]
    //private TMP_InputField password_IF;


    [SerializeField]
    private Button googleLogin_Btn;
    [SerializeField]
    private Button lineLogin_Btn;
    [SerializeField]
    private Button accountLogin_Btn;
    [SerializeField]
    private Button guestLogin_Btn;



    private void Awake()
    {
        //instance = this;
        //註冊分發
        //Net.instance.parserDict.Add(typeof(LoginSuccessCmd), LoginSuccess);
        //Net.instance.parserDict.Add(typeof(LoginFaildCmd), LoginSuccess);

        //元件初始化
        //_account_IF = transform.Find<InputField>("Account_IF");
        //_password_IF = transform.Find<InputField>("Password_IF");
        //Start_Btn = transform.Find<Button>("Start_Btn");

        googleLogin_Btn.onClick.AddListener(OnGoogleLoginClick);
        lineLogin_Btn.onClick.AddListener(OnLineLoginClick);
        accountLogin_Btn.onClick.AddListener(OnAccountLoginClick);
        guestLogin_Btn.onClick.AddListener(OnGuestLoginClick);
    }
    void Start()
    {
        AudioMgr.Instance.PlayMusic(Consts.MusicKey.Music01);
        FirebaseMgr.Instance.AutoLogin(
            onSuccess :(user) =>
            {
                // 從Firebase讀取
                FirebaseMgr.Instance.ReadJsonData(
                    path: $"{Consts.FirebaseKey.Players}{user.UserId}", 
                    onSuccess: async (jsonData) =>
                    {
                        //把Firebase讀取讀取的資料放進本地資料
                        LocalDataMgr.Instance.PlayerData.FromJson(jsonData);
                        LocalDataMgr.Instance.PlayerData.Show();

                        //await SceneMgr.Instance.LoadScene(SceneKeys.Lobby_unity);
                        await SceneMgr.Instance.LoadScene(SceneKey.LobbyScene);
                    }
                );
            }
        );

    }

    private  void OnGuestLoginClick()
    {
        Debug.Log("OnGuestLoginClick");
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


        //await SceneMgr.Instance.LoadScene(Consts.SceneKey.LobbyScene);
    }

 


    private async void OnAccountLoginClick()
    {
        Debug.Log("OnAccountLoginClick");


        await PopupMgr.Instance.ShowPopup(PopupKey.AccountLoginPopupView);
        //await PopupMgr.Instance.ShowPopup(AddressablesKeys.PrefabsKeys.AccountLoginPopupView_prefab);


    }


    private void OnLineLoginClick()
    {
        Debug.Log("OnLineLoginClick");



        //PlayerData playerData = new PlayerData
        //{
        //    UserId = "pQ1b8ldt7OfHtgBW6XwlnsqX1ml2",
        //    Nickname = "Player123",
        //    Account = "player123",
        //    Password = "password123",
        //    Email = "player123@example.com",
        //    PhoneNumber = "123-456-7890",
        //    IsGoogleBound = false,
        //    IsLineBound = true,
        //    IsFBBound = false,
        //    IsAnonymous = false,
        //    Money = 1000,
        //    Diamonds = 50,
        //    Energy = 100,
        //    PhotoUrl = "http://example.com/photo.jpg",
        //    Level = 5
        //};

        //// 寫入到 Firebase
        //FirebaseMgr.Instance.UpdateJsonData(
        //    path: $"Players/{playerData.UserId}", // 路徑：Players/123456
        //    jsonData: playerData.ToJson(),
        //    onSuccess: () => { Debug.Log("資料寫入成功！"); },
        //    onError: (error) => { Debug.LogError($"資料寫入失敗：{error}"); }
        //);

        //// 從Firebase讀取
        //FirebaseMgr.Instance.ReadJsonData(
        //path: "Players/123456",
        //onSuccess: (jsonData) =>
        //{
        //    //LocalDataMgr.Instance.PlayerData = JsonUtility.FromJson<PlayerData>(jsonData);

        //    LocalDataMgr.Instance.PlayerData.FromJson(jsonData);
        //    LocalDataMgr.Instance.PlayerData.Show(); 
        //    Debug.Log($"讀取成功，玩家暱稱：{playerData.Nickname}");

        //},
        //onError: (error) =>
        //{
        //    Debug.LogError($"讀取資料失敗：{error}");
        //}
        //);
        
        
    }

    private void OnGoogleLoginClick()
    {
        Debug.Log("OnGoogleLoginClick");
        //SceneMgr.Instance.LoadScene(Consts.SceneKey.LobbyScene);

    }

    internal void OnDestroy()
    {
        googleLogin_Btn.onClick.RemoveListener(OnGoogleLoginClick);
        lineLogin_Btn.onClick.RemoveListener(OnLineLoginClick);
        accountLogin_Btn.onClick.RemoveListener(OnAccountLoginClick);
        guestLogin_Btn.onClick.RemoveListener(OnGuestLoginClick);
    }
}
