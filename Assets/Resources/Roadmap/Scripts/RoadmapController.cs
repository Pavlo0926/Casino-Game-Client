using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RoadmapController : MonoBehaviour
{
    private BigRoad bigRoad;
    private BeadPlate beadPlate;
    private BigEyeRoad bigEye;
    private SmallRoad smallRoad;
    private CockroachRoad cockroachRoad;
    private GameHistogram gameHistogram;
    private GameForecast gameForecast;
    private GamePreviewController gamePreviewController;

    private RoadmapData data;

    private int selectedIndex;

    void Awake()
    {
        bigRoad = GetComponentInChildren<BigRoad>();
        beadPlate = GetComponentInChildren<BeadPlate>();
        bigEye = GetComponentInChildren<BigEyeRoad>();
        smallRoad = GetComponentInChildren<SmallRoad>();
        cockroachRoad = GetComponentInChildren<CockroachRoad>();
        gameHistogram = GetComponentInChildren<GameHistogram>();
        gameForecast = GetComponentInChildren<GameForecast>();
        gamePreviewController = GetComponentInChildren<GamePreviewController>();

        //var data = new RoadmapData();
        //var koi8Data = new GoldenFrogGameHistory();
        //koi8Data.evaluation.isKoi8 = true;
        //koi8Data.table.playerCard1 = "Ad";
        //koi8Data.table.playerCard2 = "Kh";
        //koi8Data.table.playerCard3 = "8d";
        //koi8Data.table.bankerCard1 = "Ah";
        //koi8Data.table.bankerCard2 = "Ks";

        //var jinChan7Data = new GoldenFrogGameHistory();
        //jinChan7Data.evaluation.isJinChan7 = true;

        //var koi8DataWithAnyOver6 = new GoldenFrogGameHistory();
        //koi8DataWithAnyOver6.evaluation.isKoi8 = true;
        //koi8DataWithAnyOver6.evaluation.isAny8Over6 = true;

        //var natural9Over7BankerData = new GoldenFrogGameHistory();
        //natural9Over7BankerData.evaluation.isNatural9Over7 = true;

        //data.LoadGameData(new List<GameResult>()
        //{
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = koi8Data },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = koi8DataWithAnyOver6 },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = jinChan7Data },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = jinChan7Data },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = natural9Over7BankerData },
        //    new GameResult() { GameOutcome = Outcome.Banker, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Player, Tag = new GoldenFrogGameHistory() },
        //});

        //data.LoadGameData(new List<GameResult>()
        //{
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },
        //    new GameResult() { GameOutcome = Outcome.Tie, Tag = new GoldenFrogGameHistory() },

        //}); 

        //SetData(data);
    }

    public void AddRandomGame()
    {
        var gameResult = new GameResult();
        gameResult.GameOutcome = (Outcome)Random.Range(0, 3);
        gameResult.Tag = new GoldenFrogGameHistory();

        var newGames = new List<GameResult>(data.Games);
        newGames.Add(gameResult);


        data.LoadGameData(newGames);
        SetData(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(RoadmapData data)
    {
        this.data = data;

        bigRoad.LoadData(data);
        beadPlate.LoadData(data);
        bigEye.LoadData(data);
        smallRoad.LoadData(data);
        cockroachRoad.LoadData(data);
        gameHistogram.LoadData(data);

        // Need to update canvas so newly added rows to roadmap can be calculated in the scroll view
        // to scroll to the correct location.
        Canvas.ForceUpdateCanvases();

        SelectLastGame();
    }

    public void SetSelection(GameResult result)
    {
        selectedIndex = data.Games.IndexOf(result);

        if (result.GameOutcome != Outcome.Tie)
        {
            bigRoad.SetSelection(result, data);
            bigEye.SetSelection(result, data);
            smallRoad.SetSelection(result, data);
            cockroachRoad.SetSelection(result, data);
        }
        else
        {
            bigRoad.ClearSelection();
            bigEye.ClearSelection();
            smallRoad.ClearSelection();
            cockroachRoad.ClearSelection();
        }

        beadPlate.SetSelection(result, data);
        gameForecast.SetSelection(result, data);
        gamePreviewController.SetSelection(result, data);

        Doozy.Engine.Soundy.SoundyManager.Play("UI", "Bubble Pop");
    }

    public void IncrementSelection()
    {
        if (selectedIndex + 1 >= data.Games.Count)
            return;

        selectedIndex++;

        var gameResult = data.Games[selectedIndex];

        SetSelection(gameResult);
    }

    public void DecrementSelection()
    {
        if (selectedIndex - 1 < 0)
            return;

        selectedIndex--;

        var gameResult = data.Games[selectedIndex];

        SetSelection(gameResult);
    }

    public void SelectLastGame()
    {
        if (data.Games.Count > 0)
        {
            selectedIndex = data.Games.Count - 1;

            SetSelection(data.Games.Last());
        }
    }
}
