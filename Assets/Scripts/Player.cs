using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine;

public class Player : MonoBehaviour
{
public static Player Instance { get; private set; } 

    public string UserId { get; set; }  /*= "5e2ebced654e99057bd3cb2c";*/

    public string UserName { get; set; }

    public Sprite PlayerProfile {get; set; }

    public uint Credits { get; set; }

    public string FacebookId { get; set; }

    public string GameUrl { get; set; } = "ws://golden-frog-baccarat.herokuapp.com";

    public void RefreshProfile(Action<AccountDetailsResponse> callback = null)
    {
        new GameSparks.Api.Requests.AccountDetailsRequest().Send(resp =>
        {
            UserName = resp.DisplayName;
            UserId = resp.UserId;
            Credits = (uint)(resp.Currencies.GetLong("CREDITS") ?? 0);
            //GameUrl = resp.ScriptData.GetString("gameServerUrl");
            FacebookId = resp.ExternalIds.GetString("FB");

            callback?.Invoke(resp);
        });
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
