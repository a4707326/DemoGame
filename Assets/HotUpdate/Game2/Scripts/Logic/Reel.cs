using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public enum ReelState
    {
        Initial,
        Spinning,
        Stopping,
        Stopped
    }


    private SlotSetting slotSetting;
    
    [Header("各階段時間控制（秒）")]
    [SerializeField]
    private float spinningTime = 4f;    // 高速旋轉時間
    private float decelerateTime = 3f;   // 停輪減速時間，如果symbolResultsStack跑完會直接停

    [Header("速度設定")]
    private float spinningSpeed = 600f;          // 高速旋轉時的速度

    private List<Symbol> symbolComponents = new List<Symbol>();
    private Dictionary<int, float> symbolYMap = new Dictionary<int, float>();
    private float symbolX;
    private float symbolHeight;
    private int symbolCount;

    private ReelState currentState = ReelState.Initial;
    private Stack<string> symbolResultsStack = null; //SymbolResults的Stack 用於最終紀錄反序要生成的symbol
    private float currentSpeed;
    private float stoppingProgress;
    private float spinningTimer;

    void Start()
    {
        //InitSymbols();
        //currentState = ReelState.Initial;
    }

    void FixedUpdate()
    {

        if (currentState == ReelState.Spinning || currentState == ReelState.Stopping)
        {
            float moveStep = currentSpeed * Time.fixedDeltaTime;

            // 統一更新符號位置，防止每個符號移動不同步
            UpdateSymbolPositions(moveStep);

            //轉輪中
            if (currentState == ReelState.Spinning)
            {
                spinningTimer += Time.fixedDeltaTime;
                if (spinningTimer >= spinningTime)
                {
                    currentState = ReelState.Stopping;
                    stoppingProgress = 0f;
                }
            }
            //停輪中
            else if (currentState == ReelState.Stopping)
            {
                //速度趨近0
                stoppingProgress += Time.fixedDeltaTime / decelerateTime;
                currentSpeed = Mathf.Lerp(spinningSpeed, 0, stoppingProgress);
                //
                if (symbolResultsStack.Count == 0)
                {
                    AlignSymbols();
                    currentState = ReelState.Stopped;
                    Debug.Log("Reel 停止，符號已對齊");
                }
            }
        }

    }

    

    public void InitSymbols(SlotSetting slotSetting)
    {
        currentState = ReelState.Initial;
        this.slotSetting = slotSetting;

        var gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            Debug.LogError("ReelContainer 缺少 GridLayoutGroup 組件！");
            return;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
        symbolHeight = gridLayout.cellSize.y + gridLayout.spacing.y;

        symbolCount = gameObject.transform.childCount;
        symbolComponents.Clear();
        symbolYMap.Clear();

        for (int i = 0; i < symbolCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            var symbol = child.GetComponent<Symbol>();
            if (symbol == null)
            {
                Debug.LogError("子物件缺少 Symbol 組件！");
                continue;
            }

            symbol.Position = i + 1;
            symbol.SetSymbol(GetRandomSymbol());
            symbolComponents.Add(symbol);

            float posY = symbol.GetRectTransform().anchoredPosition.y;
            symbolYMap[symbol.Position] = posY;
        }

        symbolYMap[symbolCount + 1] = symbolYMap[symbolCount] - symbolHeight;
        symbolX = symbolComponents[0].GetRectTransform().anchoredPosition.x;
    }

    public void StartSpin(ReelDate reelDate)
    {
        string[] symbolResults = reelDate.Symbols;

        if (symbolResults == null)
        {
            Debug.LogWarning("Reel 停輪結果為空！");
            return;
        }

        if (currentState != ReelState.Initial && currentState != ReelState.Stopped)
        {
            Debug.LogWarning("Reel 必須在初始或停止狀態才能啟動！");
            return;
        }

        currentSpeed = spinningSpeed;
        spinningTimer = 0f;
        stoppingProgress = 0f;

        symbolResultsStack = symbolResults != null && symbolResults.Length == symbolCount
            ? new Stack<string>(symbolResults)
            : null;

        currentState = ReelState.Spinning;
        Debug.Log("Reel 開始旋轉");
    }

    private void UpdateSymbolPositions(float moveStep)
    {
        foreach (var symbol in symbolComponents)
        {
            var rect = symbol.GetRectTransform();

            // 每幀統一移動距離
            rect.anchoredPosition -= new Vector2(0, moveStep);

 

            int nextPos = symbol.Position + 1;


            //Debug.Log("CurPos : " + symbol.Position + "nextPos : " + nextPos);
            //Debug.Log("moveStep" + moveStep);

            if (rect.anchoredPosition.y <= symbolYMap[nextPos])
            {
                Vector2 vector = new Vector2(symbolX, symbolYMap[nextPos]);


                //如果跑到最後一個位置移到位址1
                if (nextPos >= symbolYMap.Count)
                {
                    symbol.Position = 1;
                    vector = new Vector2(symbolX, symbolYMap[1]);
                }
                else
                {
                    symbol.Position = nextPos;
                }


                // 對齊位置
                rect.anchoredPosition = vector;

                // 更新符號（依據停止階段控制符號內容）
                if (symbol.Position == 1)
                {
                    if (currentState == ReelState.Stopping && symbolResultsStack.Count > 0)
                    {
                        symbol.SetSymbol(symbolResultsStack.Pop());
                        //DebugTools.Log("PP", symbolResultsStack.Count);
                    }
                    else
                    {
                        symbol.SetSymbol(GetRandomSymbol());
                        //Debug.Log("RR");
                    }
                }


            }
        }
    }

    //最終對齊
    private void AlignSymbols()
    {
        foreach (var symbol in symbolComponents)
        {
            var rect = symbol.GetRectTransform();
            float targetY = symbolYMap[symbol.Position];
            rect.anchoredPosition = new Vector2(symbolX, targetY);
        }
    }

    //取隨機Symbol
    private string GetRandomSymbol()
    {
        return slotSetting.Symbols[Random.Range(0, slotSetting.Symbols.Length)];
    }


}
