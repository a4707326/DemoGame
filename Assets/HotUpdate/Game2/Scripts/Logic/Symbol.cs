using TMPro;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    [SerializeField]
    public int Position = 0; // 當前符號的位置

    private TMP_Text textComponent;

    void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        if (textComponent == null)
        {
            Debug.LogError("Symbol 缺少 TMP_Text 組件！");
        }
    }

    /// <summary>
    /// 設定符號的文字
    /// </summary>
    /// <param name="symbol">符號字串</param>
    public void SetSymbol(string symbol)
    {
        if (textComponent != null)
        {
            textComponent.text = symbol;
        }
    }


    public string GetSymbol()
    {
        if (textComponent != null)
        {
            return textComponent.text;
        }
        return null;
    }


}
