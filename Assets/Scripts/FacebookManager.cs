using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
    public event Action FacebookInitialized;

    private static FacebookManager instance;

    public static FacebookManager Instance
    {
        get 
        {
            return instance;
        }
    }

    void Start()
    {
        
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void Initialize()
    {
       if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        } 
        else 
        {
            FB.ActivateApp();
            FacebookInitialized?.Invoke();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();

            FacebookInitialized?.Invoke();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    public void Login(FacebookDelegate<ILoginResult> onLoggedIn)
    {
        var perms = new List<string>(){"public_profile", "email"};
        FB.LogInWithReadPermissions(perms, onLoggedIn);
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
}
