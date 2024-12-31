using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField]
    private Avatar avatar;
    //[SerializeField]
    //private Text name_txt;
    [SerializeField]
    private Text money_txt;
    [SerializeField]
    private Text diamonds_txt;
    [SerializeField]
    private Text esnergy_txt;

    [SerializeField]
    private Button enterGame1_Btn;
    [SerializeField]
    private Button enterGame2_Btn;
    [SerializeField]
    private Button enterGame3_Btn;
    [SerializeField]
    private Button setting_Btn;


    private void Awake()
    {
        Init();
    }
    private void Start() 
    {

        
    }
    public void Init()
    {
        Debug.Log("已進入遊戲大廳");
        LocalDataMgr date = LocalDataMgr.Instance;

        enterGame1_Btn.onClick.AddListener(OnEnterGame1Click);
        enterGame2_Btn.onClick.AddListener(OnEnterGame2Click);
        enterGame3_Btn.onClick.AddListener(OnEnterGame3Click);
        setting_Btn.onClick.AddListener(OnSettingClick);
        avatar.Avatar_Btn.onClick.AddListener(OnAvatarClickAsync);

        //name_txt.text = date.PlayerData.Nickname;
        money_txt.text = date.PlayerData.Money.ToString();
        diamonds_txt.text = date.PlayerData.Diamonds.ToString();
        esnergy_txt.text = date.PlayerData.Energy.ToString();

    

    }

    private async void OnAvatarClickAsync()
    {
        Debug.Log("OnAvatarClick");
        await PopupMgr.Instance.ShowPopup(Consts.PopupKey.PlayerInfoPopupView);
    }

    private async void OnSettingClick()
    {
        Debug.Log("OnSettingClick");
        await PopupMgr.Instance.ShowPopup(Consts.PopupKey.SettingPopupView);

    }

    private void OnEnterGame3Click()
    {
        Debug.Log("OnEnterGame3Click");
    }

    private void OnEnterGame2Click()
    {
        Debug.Log("OnEnterGame2Click");
    }

    private async void OnEnterGame1Click()
    {
        Debug.Log("OnEnterGame1Click");
        await SceneMgr.Instance.LoadScene(Consts.SceneKey.Game1Scene);

    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
