using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        // 確保物件在場景切換時不被銷毀
        DontDestroyOnLoad(this.gameObject);
    }
}
