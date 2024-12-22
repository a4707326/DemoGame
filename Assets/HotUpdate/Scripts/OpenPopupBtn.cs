using System;
using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;


public class OpenPopupBtn : MonoBehaviour 
{

    private Button btn_Btn;

    [SerializeField]
    private string popupKey;


    private void Awake()
    {
        btn_Btn = GetComponent<Button>();


        btn_Btn.onClick.AddListener(OnOpenPopup);
    }

    private async void OnOpenPopup()
    {
        await PopupMgr.Instance.ShowPopup(popupKey);
    }
}