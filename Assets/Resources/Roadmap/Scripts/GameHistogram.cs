using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using System;

public class GameHistogram : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI totalGames;
    public TextMeshProUGUI bankerGames;
    public TextMeshProUGUI playerGames;
    public TextMeshProUGUI tieGames;
    public TextMeshProUGUI jinChanGames;
    public TextMeshProUGUI koiGames;

    public bool showAsPercent = false;

    private int totalGamesCount;
    private int bankerGamesCount;
    private int playerGamesCount;
    private int tieGamesCount;
    private int jinChanGamesCount;
    private int koiGamesCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadData(RoadmapData data)
    {
        totalGamesCount = data.Games.Count;
        bankerGamesCount = data.Games.Where(g => g.GameOutcome == Outcome.Banker).Count();
        playerGamesCount = data.Games.Where(g => g.GameOutcome == Outcome.Player).Count();
        tieGamesCount = data.Games.Where(g => g.GameOutcome == Outcome.Tie).Count();
        jinChanGamesCount = data.Games.Where(g =>
        {
            var gameHistory = g.Tag as GoldenFrogGameHistory;

            if (gameHistory == null)
                return false;

            return gameHistory.evaluation.isJinChan7;
        }).Count();
        koiGamesCount = data.Games.Where(g =>
        {
            var gameHistory = g.Tag as GoldenFrogGameHistory;

            if (gameHistory == null)
                return false;

            return gameHistory.evaluation.isKoi8;
        }).Count();

        RefreshView();
    }

    private void RefreshView()
    {
        if (showAsPercent)
        {
            totalGames.text = totalGamesCount.ToString();

            var bankerPercent = bankerGamesCount / (double)totalGamesCount;
            var playerPercent = playerGamesCount / (double)totalGamesCount;
            var tiePercent = tieGamesCount / (double)totalGamesCount;
            var jinChanPercent = jinChanGamesCount / (double)totalGamesCount;
            var koiPercent = koiGamesCount / (double)totalGamesCount;

            bankerGames.text = String.Format("{0}%", (bankerPercent * 100).ToString("0.#"));
            playerGames.text = String.Format("{0}%", (playerPercent * 100).ToString("0.#"));
            tieGames.text = String.Format("{0}%", (tiePercent * 100).ToString("0.#"));
            jinChanGames.text = String.Format("{0}%", (jinChanPercent * 100).ToString("0.#"));
            koiGames.text = String.Format("{0}%", (koiPercent * 100).ToString("0.#"));
        }
        else
        {
            totalGames.text = totalGamesCount.ToString();
            bankerGames.text = bankerGamesCount.ToString();
            playerGames.text = playerGamesCount.ToString();
            tieGames.text = tieGamesCount.ToString();
            jinChanGames.text = jinChanGamesCount.ToString();
            koiGames.text = koiGamesCount.ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        showAsPercent = !showAsPercent;

        RefreshView();
    }
}
