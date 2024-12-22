using System;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPopupView : PopupView, IPopup
{


    //// 登出按鈕
    //[SerializeField]
    //private Button logout_Btn;
    //[SerializeField]
    //private Button language_Btn;


    public override void Init(object data)
    {
        base.Init(data);
        //logout_Btn.onClick.AddListener(OnLogoutClick);
        //language_Btn.onClick.AddListener(OnLanguageClick);
    }



    //private void OnLogoutClick()
    //{
    //    Debug.Log("OnLogoutClick");
    //    FirebaseMgr.Instance.Logout();
    //}
    //private async void OnLanguageClick()
    //{
    //    Debug.Log("OnLanguageClick");
    //    await PopupMgr.Instance.ShowPopup(Consts.PopupKey.LanguagePopupView);
        
    //}

    internal override void OnDestroy()
    {
        base.OnDestroy();
        //logout_Btn.onClick.RemoveAllListeners();
        //language_Btn.onClick.RemoveAllListeners();
    }


    //private void Awake()
    //{
    //    //元件初始化
    //    Init(null);
    //}

}
