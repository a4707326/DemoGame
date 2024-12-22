using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupKeyDatabase", menuName = "Popup/Key Database")]
public class PopupKeyDatabase : ScriptableObject
{
    public List<string> popupKeys; // 動態存儲鍵值
}
