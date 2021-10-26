using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using System.Globalization;
using GameSparks.Api.Responses;
using Doozy.Engine.UI;

public class RankingsController : MonoBehaviour
{
    public RankingsListController ListController;

    public TextMeshProUGUI RankingsHeaderTitle;

    public UIView loadingView;

    private TimeLeftTimer timeLeftTimer;

    public Toggle dailyLeaderboardToggle;

    void Awake()
    {
        timeLeftTimer = GetComponentInChildren<TimeLeftTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRankingsViewShown()
    {
        dailyLeaderboardToggle.isOn = true;

        LoadDailyLeaderboards();
    }

    private void SetRankingsListHeader(string title, int secondsLeft)
    {
        RankingsHeaderTitle.text = title;
        timeLeftTimer.StartTimer(secondsLeft);
    }

    private void SetRankingsListHeader(string title)
    {
        RankingsHeaderTitle.text = title;
        timeLeftTimer.SetText("");
    }

    private void LoadLeaderboard(string name)
    {
        loadingView.Show();
        ListController.Clear();
        new LeaderboardDataRequest().SetLeaderboardShortCode(name).SetEntryCount(100)
            .Send ((leadResponse) => {
                Debug.Log(leadResponse.JSONString);
                ListController.LoadData(leadResponse.Data.ToArray());
                loadingView.Hide(0.33f);
        });
    }

    public void LoadDailyLeaderboards()
    {
        DateTime now = DateTime.UtcNow;
        DateTime tomorrow = now.AddDays(1).Date;

        var duration = tomorrow - now;

        SetRankingsListHeader("Today", (int)duration.TotalSeconds);
        LoadLeaderboard("PROFIT_LEADERS_DAILY");
    }

    public void LoadWeeklyLeaderboards()
    {
        SetRankingsListHeader("This Week");
        LoadLeaderboard("PROFIT_LEADERS_WEEKLY");
    }

    public void LoadMonthlyLeaderboards()
    {
        string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
        SetRankingsListHeader($"{monthName}, {DateTime.Now.Year}");
        LoadLeaderboard("PROFIT_LEADERS_MONTHLY");
    }

    public void LoadAllTimeLeaderboards()
    {
        SetRankingsListHeader($"The Best In The World");
        LoadLeaderboard("PROFIT_LEADERS_ALL_TIME");
    }
}
