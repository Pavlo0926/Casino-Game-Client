using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameSparks.Api.Responses;

public class FriendsListView : EnhancedScrollerCellView
{
    public Image profileImage;
    public Text playerDispayNameText;
    public Text connectivityStatus;

    public Image connectivityImage;

    public Texture2D LoadingProfileTexture;

    private string playerId;


    public void SetData(ListGameFriendsResponse._Player data)
    {
        var facebookId = data.ExternalIds.GetString("FB");

        string url = "http://graph.facebook.com/" + facebookId + "/picture?width=128&height=128";
        Davinci.get().load(url).setLoadingPlaceholder(LoadingProfileTexture).into(profileImage).start();

        playerDispayNameText.text = data.DisplayName;
        var isOnline = data.Online ?? false;
        connectivityStatus.text = isOnline ? "Online" : "Offline";
        playerId = data.Id;
    }

    public void OnRowClick()
    {
        var popup = ProfileScreen.ShowPlayerProfile(playerId);
    }
}
