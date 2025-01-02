
using Firebase.Auth;
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
        LoginMgr.Instance.AutoLogin();

    }

    private  void OnGuestLoginClick()
    {
        Debug.Log("OnGuestLoginClick");
        LoginMgr.Instance.GuestLogin();
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


        
    }

    private void OnGoogleLoginClick()
    {
        Debug.Log("OnGoogleLoginClick");
        LoginMgr.Instance.GoogleLogin();
    }
   

    internal void OnDestroy()
    {
        googleLogin_Btn.onClick.RemoveListener(OnGoogleLoginClick);
        lineLogin_Btn.onClick.RemoveListener(OnLineLoginClick);
        accountLogin_Btn.onClick.RemoveListener(OnAccountLoginClick);
        guestLogin_Btn.onClick.RemoveListener(OnGuestLoginClick);
    }
}
