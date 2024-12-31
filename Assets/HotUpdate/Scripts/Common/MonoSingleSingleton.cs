using UnityEngine;

//繼承用單例mono版，只用於單一場景
public class MonoSingleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 在場景中嘗試查找實例
                _instance = FindObjectOfType<T>();

                // 如果場景中找不到，則創建一個新的實例
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // 可選：防止手動創建多個單例對象
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject); // 如果已經存在單例，銷毀新的對象
        }
    }
}
