using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

public class GameHistoryRowController : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI TableNameText;
    public TextMeshProUGUI DateText;
    public TextMeshProUGUI GameCountText;
    public TextMeshProUGUI WinPercentageText;
    public TextMeshProUGUI ProfitText;

    private GSData rowData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFromGSData(GSData row)
    {
        rowData = row;

        var sessionStartTime = DateTimeUtils.UnixTimeStampToDateTime(row.GetNumber("sessionStartTime") ?? 0);
        var tableName = row.GetString("tableName");
        var gameHistoryData = row.GetGSDataList("gameHistoryData");

        var totalGamesPlayed = gameHistoryData.Count(d => d.GetGSData("payout") != null);
        var totalGamesWon = gameHistoryData.Count(d =>
        {
            var payout = d.GetGSData("payout");
            if (payout == null)
                return false;

            return payout.GetBoolean("isPass") == false && payout.GetNumber("totalPayout") > 0;
        });

        var shoeStats = JsonUtility.FromJson<GoldenFrogPlayerShoeStats>(row.GetGSData("shoeStats").JSON);
        var totalProfit = (long)shoeStats.finalCredits - shoeStats.initialCredits;

        var winPercentage = String.Format("{0}%", ((totalGamesWon / (double)totalGamesPlayed) * 100).ToString("0.#"));
        if (totalGamesPlayed == 0)
            winPercentage = "0%";

        TableNameText.text = tableName;
        DateText.text = sessionStartTime.ToString();
        GameCountText.text = totalGamesPlayed.ToString();
        WinPercentageText.text = winPercentage;

        ProfitText.text = StringFormatUtils.PayoutString(totalProfit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<GameHistoryController>().SelectRow(rowData);
    }
}
