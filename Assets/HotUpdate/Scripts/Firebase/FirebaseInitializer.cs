using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;


public class FirebaseInitializer : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    //public void RegisterWithEmail(string email, string password)
    //{
    //    auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
    //    {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("Registration canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("Registration error: " + task.Exception);
    //            return;
    //        }

    //        FirebaseUser newUser = task.Result.User;
    //        Debug.LogFormat("User registered successfully: {0} ({1})",
    //                        newUser.DisplayName, newUser.Email);
    //    });
    //}



}
