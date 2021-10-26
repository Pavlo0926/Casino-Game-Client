using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;
using Doozy.Engine.UI;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class GameHistoryDetailsController : MonoBehaviour
{
    [Header("Summary")]
    public UIView SummaryView;
    public TextMeshProUGUI TableName;
    public TextMeshProUGUI TableLimitsText;
    public TextMeshProUGUI SessionStartTime;
    public TMP_InputField Initial;
    public TMP_InputField Additional;
    public TMP_InputField Final;
    public TMP_InputField Profit;
    public TMP_InputField EarningRate;
    public TMP_InputField XP;
    public TMP_InputField GamesPlayed;
    public TMP_InputField GamesWon;
    public TMP_InputField ShoeIdentifier;
    public TMP_InputField PlayerIdentifier;


    [Header("Game List")]
    public UIView GameListView;
    public GameObject GameListContent;
    public GameObject GameListRowPrefab;

    [Header("Roadmap")]
    public UIView RoadmapView;
    public RoadmapController RoadmapController;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowRoadmapTab()
    {
        UIView.HideViewCategory("GameHistoryDetailsTabs");

        RoadmapView.Show();

    }

    public void ShowGameListTab()
    {
        UIView.HideViewCategory("GameHistoryDetailsTabs");

        GameListView.Show();
    }

    public void ShowSummaryTab()
    {
        UIView.HideViewCategory("GameHistoryDetailsTabs");

        SummaryView.Show();
    }

    public void LoadDetails(GSData rowData)
    {
        var shoeStats = JsonUtility.FromJson<GoldenFrogPlayerShoeStats>(rowData.GetGSData("shoeStats").JSON);
        var gameResults = GSDataToRoadmapData(rowData);
        var tableInformation = JsonUtility.FromJson<GoldenFrogTableInformation>(rowData.GetGSData("tableInformation").JSON);

        // Summary Page
        TableName.text = tableInformation.tableName;
        TableLimitsText.text = $"{NumberUtils.ToFriendlyQuantityString(tableInformation.minimumBet)} / {NumberUtils.ToFriendlyQuantityString(tableInformation.maximumBet)}";
        var sessionStartTime = DateTimeUtils.UnixTimeStampToDateTime(rowData.GetInt("sessionStartTime") ?? 0);
        SessionStartTime.text = sessionStartTime.ToString();
        var profit = (long)shoeStats.finalCredits - shoeStats.initialCredits;
        var earningRate = ((profit) / (double)Math.Abs(shoeStats.initialCredits)) * 100;
        var gameHistoryData = rowData.GetGSDataList("gameHistoryData");
        var totalGamesPlayed = gameHistoryData.Count(d => d.GetGSData("payout") != null);
        var totalGamesWon = gameHistoryData.Count(d =>
        {
            var payout = d.GetGSData("payout");
            if (payout == null)
                return false;

            return payout.GetBoolean("isPass") == false && payout.GetNumber("totalPayout") > 0;
        });

        Initial.text = StringFormatUtils.CurrencyString(shoeStats.initialCredits);
        Additional.text = StringFormatUtils.PayoutString(0);
        Final.text = StringFormatUtils.CurrencyString(shoeStats.finalCredits);
        Profit.text = StringFormatUtils.PayoutString(profit);
        EarningRate.text = StringFormatUtils.PercentagePayoutString(earningRate);
        XP.text = "0";
        GamesPlayed.text = totalGamesPlayed.ToString();
        GamesWon.text = totalGamesWon.ToString();
        ShoeIdentifier.text = rowData.GetNumber("shoeId")?.ToString();
        PlayerIdentifier.text = rowData.GetString("playerId");


        // Roadmap Page
        var roadmapData = new RoadmapData();
        roadmapData.LoadGameData(gameResults);
        RoadmapController.SetData(roadmapData);

        // Games List Page
        LoadGamesList(gameResults);
    }

    private void LoadGamesList(IEnumerable<GameResult> gameResults)
    {
        GameListContent.transform.Clear();

        foreach (var gameResult in gameResults)
        {
            var gameHistory = gameResult.Tag as GoldenFrogGameHistory;

            // Don't list games that the player didn't place a wager for.
            if (gameHistory.payout == null)
                continue;

            var row = Instantiate(GameListRowPrefab, GameListContent.transform);
            var controller = row.GetComponent<GameListRowController>();

            controller.LoadGameResult(gameResult);
        }
    }

    private IEnumerable<GameResult> GSDataToRoadmapData(GSData data)
    {
        var gameHistoryData = data.GetGSDataList("gameHistoryData");

        var gameResults = new List<GameResult>();
        foreach (var gameHistory in gameHistoryData)
        {
            var history = new GoldenFrogGameHistory();

            var evaluation = gameHistory.GetGSData("evaluation");
            var table = gameHistory.GetGSData("table");
            var payout = gameHistory.GetGSData("payout");

            history.evaluation = JsonUtility.FromJson<GoldenFrogEvaluation>(evaluation.JSON);
            history.table = JsonUtility.FromJson<GoldenFrogTable>(table.JSON);
            history.payout = JsonUtility.FromJson<GoldenFrogPayout>(payout?.JSON ?? null);
            history.gameNumber = gameHistory.GetNumber("gameNumber") ?? 0;
            history.gameTimeUtc = gameHistory.GetInt("gameTimeUtc") ?? 0;

            gameResults.Add(ServerGameHistoryToGameResult(history));
        }

        return gameResults;
    }

    public static GameResult ServerGameHistoryToGameResult(GoldenFrogGameHistory gameHistory)
    {
        var result = new GameResult();

        switch (gameHistory.evaluation.outcome)
        {
            case "banker":
                result.GameOutcome = Outcome.Banker;
                break;
            case "player":
                result.GameOutcome = Outcome.Player;
                break;
            case "tie":
                result.GameOutcome = Outcome.Tie;
                break;
        }

        result.Tag = gameHistory;

        return result;
    }
}
