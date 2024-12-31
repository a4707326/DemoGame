using System;
using UnityEngine;
using UnityEngine.UI;

public class Game1View : MonoBehaviour
{

    [SerializeField]
    private Button setting_Btn;

    private void Awake()
    {
        Init();
    }


    public void Init()
    {
        DebugTools.LogMethodName();
        setting_Btn.onClick.AddListener(OnSettingClick);

    }

    private async void OnSettingClick()
    {
        DebugTools.LogMethodName();
        await SceneMgr.Instance.LoadScene(Consts.SceneKey.LobbyScene);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
