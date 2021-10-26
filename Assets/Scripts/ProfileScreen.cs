using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScreen : MonoBehaviour
{

    [SerializeField] Image profileImage;
    [SerializeField] Text nameText, chipsText, coinstext;
    [SerializeField] Text messageText;
    [SerializeField] Text highestBalance, largestWin, largestWinSteak, currentRank, gamesPlayed, gamesWon, WinningRate;
    private UIPopup popup;

    private int profitLeaderboardRanking = -1;
    private long profitLeaderboardScore = 0;

    private void Start()
    {
        popup = GetComponent<UIPopup>();
    }

    public static UIPopup ShowPlayerProfile(string playerId)
    {
        var popup = UIPopup.GetPopup("ProfilePopup");
        var profileScreen = popup.GetComponent<ProfileScreen>();
        profileScreen.LoadPlayerProfile(playerId);
        popup.Show();

        return popup;
    }

    public void OnRankClick()
    {
        var popup = Alertbox.ShowMessage(StringFormatUtils.PayoutString(profitLeaderboardScore), "Today's Profit", () =>
        {
        }, null, "OK");
    }

    public void LoadPlayerProfile(string playerId)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GET_PROFILE")
            .SetEventAttribute("PLAYER_ID", playerId)
            .Send(resp => 
            {
                var scriptData = resp.ScriptData;

                var player = scriptData.GetGSData("player");
                var profileStats = resp.ScriptData.GetGSData("profileStats");

                var facebookId = player.GetGSData("externalIds").GetString("FB");
                var credits = scriptData.GetGSData("currency").GetGSData("named").GetLong("CREDITS");

                string url = "http://graph.facebook.com/" + facebookId + "/picture?width=256&height=256";
                Davinci.get().load(url).into(profileImage).start();

                Debug.Log("Got profileStats: " + profileStats.JSON);

                nameText.text = player.GetString("displayName");
                chipsText.text = StringFormatUtils.CurrencyString(credits ?? 0);
                coinstext.text = "-";

                highestBalance.text = StringFormatUtils.CurrencyString(profileStats.GetLong("highestBalance") ?? 0);
                largestWin.text = StringFormatUtils.CurrencyString(profileStats.GetLong("largestWin") ?? 0);
                largestWinSteak.text = profileStats.GetLong("longestWinStreak").ToString();
                currentRank.text = "-";
                gamesPlayed.text = profileStats.GetLong("gamesPlayed").ToString();
                gamesWon.text = profileStats.GetLong("gamesWon").ToString();
                var winningRate = (profileStats.GetLong("gamesWon") / (float)profileStats.GetLong("gamesPlayed"));
                WinningRate.text = string.Format("{0:p2}", winningRate);
                messageText.text = profileStats.GetString("message").ToString();

            });

            new GameSparks.Api.Requests.GetLeaderboardEntriesRequest()
                .SetLeaderboards(new List<string>() { "PROFIT_LEADERS_DAILY" })
                .SetPlayer(playerId)
                .Send(resp =>
                {
                    if (resp.HasErrors)
                    {
                        Debug.LogError(resp.Errors.JSON);
                    }
                    else
                    {
                        var now = DateTime.UtcNow;
                        var leaderboardKey = $"PROFIT_LEADERS_DAILY.SNAPSHOT.{now.Year}-{now.Month.ToString().PadLeft(2, '0')}-{now.Day.ToString().PadLeft(2, '0')}";
                        Debug.Log(leaderboardKey);
                        var data = resp.BaseData.GetGSData(leaderboardKey);
                        profitLeaderboardRanking = data.GetInt("rank") ?? -1;
                        profitLeaderboardScore = data.GetLong("SUM-PROFIT") ?? 0;
                        currentRank.text = profitLeaderboardRanking == -1 ? "-" : profitLeaderboardRanking.ToString();
                    }
                });
    }

    public void OnCloseClick()
    {
        popup.Hide();
    }
}
