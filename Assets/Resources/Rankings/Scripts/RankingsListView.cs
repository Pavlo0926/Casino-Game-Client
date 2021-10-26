using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameSparks.Api.Responses;

public class RankingsListView : EnhancedScrollerCellView
{
    public Image profileImage;
    public TextMeshProUGUI rankingText;
    public TextMeshProUGUI playerDispayNameText;
    public TextMeshProUGUI creditsText;

    public Texture2D LoadingProfileTexture;

    private string playerId;


    public void SetData(LeaderboardDataResponse._LeaderboardData data)
    {
        var facebookId = data.ExternalIds.GetString("FB");

        string url = "http://graph.facebook.com/" + facebookId + "/picture?width=128&height=128";
        Davinci.get().load(url).setLoadingPlaceholder(LoadingProfileTexture).into(profileImage).start();

        rankingText.text = data.Rank?.ToString();
        playerDispayNameText.text = data.UserName;
        creditsText.text = StringFormatUtils.PayoutString(data.BaseData.GetLong("SUM-PROFIT") ?? 0);
        playerId = data.UserId;
    }

    public void OnRowClick()
    {
        var popup = ProfileScreen.ShowPlayerProfile(playerId);
    }
}
