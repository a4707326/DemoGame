using UnityEngine;

public class SeamlessBackground : MonoBehaviour
{
    public Transform Background1; // 第一張背景
    public Transform Background2; // 第二張背景
    public float ScrollSpeed = 2f; // 滾動速度

    private Vector3 startPosition; // 背景開始位置的 X 坐標
    private Vector3 endPosition;   // 背景結束位置的 X 坐標
    private float backgroundWidth; // 背景的寬度

    void Start()
    {
        // 計算背景寬度，假設背景的寬度由 Renderer 的 bounds 決定
        backgroundWidth = Background1.GetComponent<SpriteRenderer>().bounds.size.x;

        // 動態計算開始與結束位置
        startPosition = Background2.localPosition;
        endPosition = new Vector3(Background1.localPosition.x - backgroundWidth, Background1.localPosition.y, Background1.localPosition.z);

        // 初始化第二張背景的位置
        Background2.localPosition = new Vector3(Background1.localPosition.x + backgroundWidth, Background1.localPosition.y, Background1.localPosition.z);
    }

    void Update()
    {
        // 讓背景圖片持續向左移動
        Background1.localPosition += Vector3.left * ScrollSpeed * Time.deltaTime;
        Background2.localPosition += Vector3.left * ScrollSpeed * Time.deltaTime;

        JudgeRelocate(Background1);
        JudgeRelocate(Background2);


    }
    // 當背景圖片到達結束位置時，將它移動到開始位置
    void JudgeRelocate(Transform transform) 
    {
        if (transform.localPosition.x <= endPosition.x)
        {
            transform.localPosition = startPosition;
        }
    }
    public void Stop()
    {
        ScrollSpeed = 0;
    }
    public void Run()
    {
        ScrollSpeed = 2f;
    }

}
