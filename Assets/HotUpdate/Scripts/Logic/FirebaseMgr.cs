using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine.Events;
using Firebase.Database;
using Google.MiniJSON;
using Newtonsoft.Json;

public class FirebaseMgr : Singleton<FirebaseMgr>
{
    //private GameObject root;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private DatabaseReference databaseRef;

    internal void Init()
    {
        //初始化Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference; 

                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }


    /// <summary>
    /// 自動登入
    /// </summary>
    /// <param name="onSuccess">登入成功的回調</param>
    /// <param name="onError">登入失敗的回調</param>
    public void AutoLogin(Action<FirebaseUser> onSuccess = null, Action<string> onError = null)
    {
        if (auth == null)
        {
            Debug.LogError("FirebaseAuth 尚未初始化！");
            onError?.Invoke("FirebaseAuth 尚未初始化");
            return;
        }

        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log($"檢測到已登入的用戶: {user.Email ?? "匿名用戶"}_{user.UserId}");
            onSuccess?.Invoke(user);
        }
        else
        {
            Debug.Log("未檢測到已登入用戶，請手動登入");
            onError?.Invoke("No user detected.");
        }
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
            if (user != null)
            {
                Debug.Log($"用戶已登入: {user.Email ?? "匿名用戶"}_{user.UserId}");
            }
            else
            {
                Debug.Log("用戶已登出");
            }
        }
    }

    public void RegisterWithEmail(string email, string password, Action<FirebaseUser> onSuccess, Action<string> onError = null)
    {
        
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Registration canceled.");
                onError?.Invoke("Registration canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Registration error: " + task.Exception);
                onError?.Invoke($"Registration error: {task.Exception.Message}");
                return;
            }
            if (task.IsCompletedSuccessfully)
            {
                FirebaseUser user = task.Result.User;

                Debug.LogFormat("User registered successfully: {0} ({1})",
                                user.DisplayName, user.Email);
                onSuccess?.Invoke(user);
                return;
            }

            Debug.LogError("Unknown registration error.");
            onError?.Invoke("Unknown registration error.");
        });
    }


    /// <summary>
    /// 匿名訪客登入
    /// </summary>
    /// <param name="onSuccess">登入成功回調</param>
    /// <param name="onError">登入失敗回調</param>
    public void GuestLogin(Action<FirebaseUser> onSuccess, Action<string> onError = null )
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                onError?.Invoke("Guest login canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                onError?.Invoke($"Guest login error: {task.Exception.Message}");
                return;
            }
            if (task.IsCompletedSuccessfully)
            {
                user = task.Result.User;
                Debug.Log($"Guest logged in successfully: {user.UserId}");
                onSuccess?.Invoke(user);
            }
        });
    }

    public void LoginWithEmail(string email, string password, Action<FirebaseUser> onSuccess, Action<string> onError = null)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login canceled.");
                onError?.Invoke("Login canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login error: " + task.Exception);
                onError?.Invoke($"Login error: {task.Exception.Message}");
                return;
            }
            if (task.IsCompletedSuccessfully)
            {
                user = task.Result.User;
                Debug.LogFormat("User logged in successfully: {0} ({1})", user.DisplayName, user.Email);
                onSuccess?.Invoke(user);
                return;
            }

            Debug.LogError("Unknown login error.");
            onError?.Invoke("Unknown login error.");
        });
    }

    //登入
    public async void Logout()
    {
        auth.SignOut();
        user = null;
        Debug.Log("User logged out.");
        await SceneMgr.Instance.LoadScene(Consts.SceneKey.LoginScene);
        //await SceneMgr.Instance.LoadScene(AddressablesKeys.SceneKeys.Login_unity);
    }


    public bool IsLogin()
    {
        if (user == null || auth == null)
        {
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// 寫入資料
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="data">寫入資料</param>
    /// <param name="callback">callback</param>
    public void WriteData(string path, Dictionary<string, object> data, UnityAction callback = null)
    {
        databaseRef.Child(path).SetValueAsync(data).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    callback?.Invoke();
                });
            }
            else
            {
                Debug.LogError("資料寫入失敗: " + task.Exception);
            }
        });
    }


    /// <summary>
    /// 移除資料
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="callback">callback</param>
    public void RemoveData(string path, UnityAction callback = null)
    {
        databaseRef.Child(path).RemoveValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    callback?.Invoke();
                });
            }
            else
            {
                Debug.LogError("移除資料失敗: " + task.Exception);
            }
        });
    }

    /// <summary>
    /// 讀取資料
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="callback">讀取結果回傳</param>
    public void ReadData<T>(string path, UnityAction<T> callback) where T : class, new()
    {
        databaseRef.Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;

                T data = default;
                if (snapshot.Exists)
                {
                    data = JsonUtility.FromJson<T>(snapshot.GetRawJsonValue());
                }

                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    if (data == null)
                    {
                        data = new T();
                    }
                    callback?.Invoke(data);
                });
            }
            else
            {
                Debug.LogError("讀取資料失敗: " + task.Exception);
            }
        });
    }
    /// <summary>
    /// 更新資料
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="updates">更新資料</param>
    /// <param name="callback">callback</param>
    public void UpdateData(string path, Dictionary<string, object> updates, UnityAction callback = null)
    {
        databaseRef.Child(path).UpdateChildrenAsync(updates).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    callback?.Invoke();
                });
            }
            else
            {
                Debug.LogError("更新資料失敗: " + task.Exception);
            }
        });
    }

    /// <summary>
    /// 將 JSON 格式資料寫入 Firebase 資料庫
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="jsonData">JSON 格式的資料</param>
    /// <param name="onSuccess">成功回調</param>
    /// <param name="onError">失敗回調</param>
    public void WriteJsonData(string path, string jsonData, UnityAction onSuccess = null, UnityAction<string> onError = null)
    {
        //// 將 JSON 資料轉換為 Firebase 可接受的物件
        //object jsonObject = JsonUtility.FromJson<object>(jsonData);


        // 寫入 Firebase
        databaseRef.Child(path).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    Debug.Log($"資料成功寫入到路徑: {path}");
                    onSuccess?.Invoke();
                });
            }
            else
            {
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    Debug.LogError($"資料寫入失敗 (路徑: {path})：" + task.Exception);
                    onError?.Invoke(task.Exception?.Message ?? "未知錯誤");
                });
            }
        });
    }
    /// <summary>
    /// 從 Firebase 資料庫讀取 JSON 格式的資料
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="onSuccess">成功回調，返回 JSON 字串</param>
    /// <param name="onError">失敗回調</param>
    public void ReadJsonData(string path, UnityAction<string> onSuccess, UnityAction<string> onError = null)
    {
        databaseRef.Child(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                string jsonData = snapshot.GetRawJsonValue();
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    if (string.IsNullOrEmpty(jsonData)) 
                    {
                        Debug.LogError($"資料讀取失敗 (路徑: {path})：" + task.Exception);
                        return;
                    }
                    Debug.Log($"ReadJsonData成功讀取資料：{jsonData}");
                    onSuccess?.Invoke(jsonData);
                });
            }
            else
            {
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    Debug.LogError($"ReadJsonData資料讀取失敗 (路徑: {path})：" + task.Exception);
                    onError?.Invoke(task.Exception?.Message ?? "未知錯誤");
                });
            }
        });
    }
    /// <summary>
    /// 更新資料 (支援 JSON)
    /// </summary>
    /// <param name="path">資料路徑</param>
    /// <param name="jsonData">JSON 字符串</param>
    /// <param name="onSuccess">更新成功回調</param>
    /// <param name="onError">更新失敗回調</param>
    public void UpdateJsonData(string path, string jsonData, Action onSuccess = null, Action<string> onError = null)
    {
        try
        {
            // 將 JSON 字符串轉換為字典
            var dataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);

            // 使用 UpdateChildrenAsync 更新資料
            databaseRef.Child(path).UpdateChildrenAsync(dataDictionary).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    Debug.LogError(task.Exception?.Message ?? "Unknown error occurred while updating data.");
                    onError?.Invoke(task.Exception?.Message ?? "Unknown error occurred while updating data.");
                }
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON 解析失敗: {ex.Message}");
            onError?.Invoke($"JSON 解析失敗: {ex.Message}");
        }
    }


}
