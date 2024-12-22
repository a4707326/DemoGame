using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour, IPopup
{


    // 離開按鈕
    [SerializeField]
    private Button exit_Btn;

    public virtual void Init(object data)
    {
        exit_Btn.onClick.AddListener(OnExitClick);
    }



    internal void OnExitClick()
    {
        Debug.Log("OnExitClick");
        OnDestroy();
        PopupMgr.Instance.ClosePopup(gameObject);
        
    }
    internal virtual void OnDestroy()
    {
       exit_Btn.onClick.RemoveAllListeners();
    }

}
