using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    Image avatar_Img;
    [SerializeField]
    public Button Avatar_Btn;
    [SerializeField]
    Image avatarFrame_Img;
    [SerializeField]
    TMP_Text name_Txt;
    [SerializeField] 
    private string avatarUrl = "https://pit1.topit.pro/forum/202105/30/132655kw58uwg3sd0595nd.jpg"; // 圖像的 URL

    // Start is called before the first frame update
    async void Start()
    {
        LocalDataMgr date = LocalDataMgr.Instance;
        name_Txt.text = date.PlayerData.Nickname.ToString();
        avatar_Img.sprite = await LoadAvatar(avatarUrl);

    }
    // 公開方法來載入頭像
    public async Task<Sprite> LoadAvatar(string url)
    {
        avatarUrl = url;
        return await DownloadAvatarAsync(avatarUrl);
    }

    // 異步下載頭像
    private async Task<Sprite> DownloadAvatarAsync(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var operation = request.SendWebRequest();

            // 等待請求完成
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("頭像下載成功！");
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                return ConvertTextureToSprite(texture);
            }
            else
            {
                Debug.LogError($"下載頭像失敗：{request.error}");
                return null;
            }
        }
    }

    // 將 Texture2D 轉換為 Sprite
    private Sprite ConvertTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }

}
