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
        LoginMgr.Instance.AccountLogin(account_IF.text, password_IF.text);
    }

    private void OnRegisterClick()
    {
        Debug.Log("OnRegisterClick");
        LoginMgr.Instance.AccountRegister(account_IF.text, password_IF.text);
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
