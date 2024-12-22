using UnityEngine;

public class ClearCache : MonoBehaviour
{
    [ContextMenu("Clear Cache")]
    public void ClearAddressablesCache()
    {
        if (Caching.ClearCache())
        {
            Debug.Log("本地緩存已清理！");
        }
        else
        {
            Debug.LogWarning("緩存清理失敗！");
        }
    }
}
