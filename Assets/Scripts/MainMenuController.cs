using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FacebookManager.Instance.FacebookInitialized += OnFacebookInitialized;
        FacebookManager.Instance.Initialize();
    }

    private void OnFacebookInitialized()
    {
        Debug.Log("Facebook Initialized");
    }

    public void OnFacebookLoginClick()
    {
        FacebookManager.Instance.Login((r) =>
        {
            new GameSparks.Api.Requests.FacebookConnectRequest()
                    .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                    .SetDoNotLinkToCurrentPlayer(false)// we don't want to create a new account so link to the player that is currently logged in
                    .SetSwitchIfPossible(true)//this will switch to the player with this FB account id they already have an account from a separate login
                    .Send((fbauth_response) => {
                        if (!fbauth_response.HasErrors)
                        {
                            Player.Instance.UserId = fbauth_response.UserId;
                            Player.Instance.UserName = fbauth_response.DisplayName;
                            
                            UIManager.Instance.ShowHomeScreen();
                        }
                        else
                        {
                            // TODO: Login Failed
                            Debug.LogWarning(fbauth_response.Errors.JSON);
                        }
                    });
        });
    }
}
