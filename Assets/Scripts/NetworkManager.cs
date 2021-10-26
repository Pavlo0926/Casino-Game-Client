using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GameSparks.Api.Requests;
using GameSparks.Core;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    void Start()
    {
        GS.GameSparksAvailable += OnGameSparksConnected;
    }
    

    private void OnGameSparksConnected(bool _isConnected)
    {
        if(_isConnected) {
            UIManager.Instance.Print("GameSparks Connected...");
        }
        else
        {
            Debug.LogWarning("GameSparks Not Connected...");
        }
    }

    public void DeviceAuthentication_bttn()
    {
        Debug.Log("Attempting Device Authentication...");
        new GameSparks.Api.Requests.DeviceAuthenticationRequest()
            .SetDisplayName("Player 2")
            .Send((auth_response) =>
            {
                if (!auth_response.HasErrors)
                {
                }
                else
                {
                    Debug.LogWarning(auth_response.Errors.JSON); // if we have errors, print them out
                }
            });
    }

    public void FacebookConnect()
    {
        Debug.Log("Connecting Facebook With GameSparks...");// first check if FB is ready, and then login //
                                                            // if it's not ready we just init FB and use the login method as the callback for the init method //
        if (!FB.IsInitialized)
        {
            FB.Init(ConnectGameSparksToGameSparks, null);
        }
        else
        {
            FB.ActivateApp();
            ConnectGameSparksToGameSparks();
        }
    }


    private void ConnectGameSparksToGameSparks()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, (result) =>
            {
                if (FB.IsLoggedIn)
                {
                    new GameSparks.Api.Requests.FacebookConnectRequest()
                        .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                        .SetDoNotLinkToCurrentPlayer(false)// we don't want to create a new account so link to the player that is currently logged in
                        .SetSwitchIfPossible(true)//this will switch to the player with this FB account id they already have an account from a separate login
                            .Send((fbauth_response) =>
                            {
                                if (!fbauth_response.HasErrors)
                                {
                                    Player.Instance.UserId = fbauth_response.UserId;
                                    Player.Instance.UserName = fbauth_response.DisplayName;
                                    UIManager.Instance.ShowHomeScreen();
                                }
                                else
                                {
                                    Debug.LogWarning(fbauth_response.Errors.JSON);//if we have errors, print them out
                                }
                            });
                }
                else
                {
                    Debug.LogWarning("Facebook Login Failed:" + result.Error);
                }
            });// lastly call another method to login, and when logged in we have a callback
        }
        else
        {
            FacebookConnect();// if we are still not connected, then try to process again
        }
    }

    public void UserAuthentication(string username, string password)
    {
        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(username)//set the username for login
            .SetPassword(password)//set the password for login
            .Send((auth_response) =>
            { //send the authentication request
                if (!auth_response.HasErrors)
                { // for the next part, check to see if we have any errors i.e. Authentication failed
                    Debug.Log("GameSparks Authenticated...");
                    Player.Instance.UserId = auth_response.UserId;
                    Player.Instance.UserName = auth_response.DisplayName;
                    UIManager.Instance.ShowHomeScreen();
                    //userNameField.text = auth_response.DisplayName;
                }
                else
                {
                    Debug.LogWarning(auth_response.Errors.JSON); // if we have errors, print them out
                    if (auth_response.Errors.GetString("DETAILS") == "UNRECOGNISED")
                    { // if we get this error it means we are not registered, so let's register the user instead
                        Debug.Log("User Doesn't Exists, Registering User...");
                        new GameSparks.Api.Requests.RegistrationRequest()
                            .SetDisplayName(username.Split('@')[0])
                            .SetUserName(username)
                            .SetPassword(password)
                            .Send((reg_response) =>
                            {
                                if (!reg_response.HasErrors)
                                {
                                    Debug.Log("GameSparks Authenticated...");

                                    Player.Instance.UserId = reg_response.UserId;
                                    Player.Instance.UserName = reg_response.DisplayName;

                                    UIManager.Instance.ShowHomeScreen();
                                }
                                else
                                { 
                                    Debug.LogWarning("Signup Fail : " +auth_response.Errors.JSON); // if we have errors, print them out
                                }
                            });
                    }
                }
            });
    }

}
