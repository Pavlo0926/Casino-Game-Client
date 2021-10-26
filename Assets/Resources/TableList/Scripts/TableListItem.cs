using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Doozy.Engine.UI;

public class TableListItem : MonoBehaviour
{
    public TextMeshProUGUI TableNameText;
    public TableListPlayerAvatar[] PlayerAvatars;
    public CanvasGroup[] PlayerSlots;
    public Texture2D LoadingProfileTexture;
    public UIGradient TitleGradient;

    public BigRoad bigRoad;

    private RoomListingData data;


    void Awake()
    {
        bigRoad = GetComponentInChildren<BigRoad>();
        bigRoad.enableSelection = false;
    }

    void Start()
    {
    }

    public void OnTableSelected()
    {
        Debug.LogWarning("Table Selected: " + data.roomId);
        UIManager.Instance.ShowGameScreen(data.roomId);
    }

    public void LoadTitleBarColor(uint minimumBet)
    {
        var color1 = new Color();
        var color2 = new Color();
        switch (this.data.metadata.tableInformation.minimumBet)
        {
            case 100:
                ColorUtility.TryParseHtmlString("#1A9D00", out color1);
                ColorUtility.TryParseHtmlString("#1A6500", out color2);
                break;
            case 1000:
                ColorUtility.TryParseHtmlString("#E100A4", out color1);
                ColorUtility.TryParseHtmlString("#8F007D", out color2);
                break;
            case 10000:
                ColorUtility.TryParseHtmlString("#006AFF", out color1);
                ColorUtility.TryParseHtmlString("#0018FF", out color2);
                break;
            case 100000:
                ColorUtility.TryParseHtmlString("#ED0F09", out color1);
                ColorUtility.TryParseHtmlString("#920F12", out color2);
                break;
            case 1000000:
                ColorUtility.TryParseHtmlString("#878787", out color1);
                ColorUtility.TryParseHtmlString("#424242", out color2);
                break;
            case 10000000:
                ColorUtility.TryParseHtmlString("#FDB600", out color1);
                ColorUtility.TryParseHtmlString("#BD8300", out color2);
                break;
        }

        TitleGradient.m_color1 = color1;
        TitleGradient.m_color2 = color2;
    }

    public void LoadRoomListingData(RoomListingData data)
    {
        this.data = data;
        var minimumBet = data.metadata.tableInformation.minimumBet;

        LoadTitleBarColor(minimumBet);
        TableNameText.text = data.metadata.tableInformation.tableName;
        LoadPlayers(data.clients);
        
        var roadmapData = new RoadmapData();
        var gameResults = new List<GameResult>();
        foreach (var gameHistory in data.metadata.gameHistory)
        {
            var gameResult = GameHistoryDetailsController.ServerGameHistoryToGameResult(gameHistory);
            gameResults.Add(gameResult);
        }

        // Live avatars at table
        //var i = 0;
        //foreach (var playerData in data.metadata.players)
        //{
        //    if (i < PlayerAvatars.Length)
        //    {
        //        string url = "http://graph.facebook.com/" + playerData.facebookId + "/picture?width=64&height=64";
        //        Davinci.get().load(url).setLoadingPlaceholder(LoadingProfileTexture).into(PlayerAvatars[i].profileImage).start();

        //        PlayerAvatars[i].GetComponent<CanvasGroup>().DOFade(1, 0.5f);

        //        i++;
        //    }
        //}

        //for (; i < PlayerAvatars.Length; i++)
        //{
        //    PlayerAvatars[i].GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
        //}

        roadmapData.LoadGameData(gameResults);
        bigRoad.LoadData(roadmapData);
    }

    public void LoadPlayers(int numPlayers)
    {
        for (var i = 0; i < PlayerSlots.Length; i++)
        {
            if (i < numPlayers)
            {
                PlayerSlots[i].DOFade(1, 0.5f);
            }
            else
            {
                PlayerSlots[i].DOFade(0.6f, 0.5f);
            }
        }
    }
}
