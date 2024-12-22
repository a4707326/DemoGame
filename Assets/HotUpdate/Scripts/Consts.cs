using static AddressablesKeys;
using static Consts;

[System.Serializable]
public class Consts
{
    // 路徑相關常量
    public static class SceneKey
    {
        public const string LoginScene = SceneKeys.Login_unity;
        public const string LobbyScene = SceneKeys.Lobby_unity;
        public const string Game1Scene = SceneKeys.Game1View_unity;
        public const string SampleScene = SceneKeys.SampleScene_unity;

    }

    public static class PopupKey
    {
        public const string PopupView = PrefabsKeys.PopupView_prefab;
        public const string AccountLoginPopupView = PrefabsKeys.AccountLoginPopupView_prefab;
        public const string SettingPopupView = PrefabsKeys.SettingPopupView_prefab;
        public const string LanguagePopupView = PrefabsKeys.LanguagePopupView_prefab;
        public const string PlayerInfoPopupView = PrefabsKeys.PlayerInfoPopupView_prefab;


    }

    public static class FirebaseKey
    {
        public const string Players = "Players/";
    }


    public static class MusicKey
    {
        public const string Music01 = DefaultLocalGroupKeys.Music01_wav;
    }

    public static class Language
    {
        public const string zh_TW = "zh-TW";
        public const string en_US = "en-US";
    }


    // 遊戲設定相關常量
    public static class GameSettings
    {
        public const int MaxPlayerCount = 10;
        public const int DefaultLives = 3;
    }

    // 網絡相關常量
    public static class Network
    {
        public const string BaseUrl = "https://api.example.com";
        public const int TimeoutSeconds = 30;
    }

    // 標籤（Addressables Label）相關
    public static class Labels
    {
        public const string PreloadAssets = "Preload";
        public const string AudioAssets = "Audio";
    }


}