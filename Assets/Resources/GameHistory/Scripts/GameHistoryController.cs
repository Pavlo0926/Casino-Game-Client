using Doozy.Engine.UI;
using GameSparks.Api.Requests;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameHistoryController : MonoBehaviour
{
    public GameObject GameHistoryRowPrefab;
    public GameObject List;
    public UIView loadingView;


    void Awake()
    {
    }

    void Update()
    {
    }

    public static UIPopup CreateGameHistoryDialog()
    {
        var popup = UIPopup.GetPopup("GameHistory");
        var controller = popup.GetComponent<GameHistoryController>();

        controller.LoadGameHistory();

        return popup;
    }

    public void ShowGameDetails(GSData gameData)
    {
        var popup = UIPopup.GetPopup("GameHistoryDetailsPopup");

        var detailsController = popup.GetComponentInChildren<GameHistoryDetailsController>();

        detailsController.LoadDetails(gameData);
        popup.Show(true);
    }

    public void SelectRow(GSData gameData)
    {
        ShowGameDetails(gameData);
    }

    public void LoadGameHistory()
    {
        loadingView.Show();
        new LogEventRequest()
            .SetEventKey("GET_GAME_HISTORY")
            .Send((response) => {
                var scriptData = response.ScriptData;

                var results = scriptData.GetGSDataList("results");

                foreach (var result in results)
                {
                    var row = Instantiate(GameHistoryRowPrefab, List.transform);
                    var rowController = row.GetComponent<GameHistoryRowController>();

                    rowController.LoadFromGSData(result);
                }

                loadingView.Hide();
            });
    }
}
