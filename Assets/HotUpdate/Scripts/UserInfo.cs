[System.Serializable]
public class UserInfo
{
    public string UserId;         // Firebase 使用者 UID
    public string Email;          // 使用者的電子郵件
    public string DisplayName;    // 使用者的暱稱
    public string PhotoUrl;       // 使用者的頭像 URL
    public bool IsEmailVerified;  // 電子郵件是否驗證

    // 建構函數，初始化用
    public UserInfo(string userId, string email, string displayName = null, string photoUrl = null, bool isEmailVerified = false)
    {
        UserId = userId;
        Email = email;
        DisplayName = displayName ?? "未設置暱稱";
        PhotoUrl = photoUrl ?? "未設置頭像";
        IsEmailVerified = isEmailVerified;
    }

    // 輸出資訊為字串（方便除錯用）
    public override string ToString()
    {
        return $"UID: {UserId}\nEmail: {Email}\nDisplayName: {DisplayName}\nPhotoUrl: {PhotoUrl}\nIsEmailVerified: {IsEmailVerified}";
    }
}