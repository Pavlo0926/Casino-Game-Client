using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameForecast : MonoBehaviour
{
    public Image bankerBigEyeRoad;
    public Image bankerSmallRoad;
    public Image bankerCockroachRoad;

    public Image playerBigEyeRoad;
    public Image playerSmallRoad;
    public Image playerCockroachRoad;

    private Sprite blueBigEye;
    private Sprite redBigEye;

    private Sprite blueSmallRoad;
    private Sprite redSmallRoad;

    private Sprite blueCockroach;
    private Sprite redCockroach;

    private ForecastService forecastService = new ForecastService();

    void Awake()
    {
        blueBigEye = Resources.Load<Sprite>("Roadmap/Symbols/big_eye_boy_blue");
        redBigEye = Resources.Load<Sprite>("Roadmap/Symbols/big_eye_boy_red");

        blueSmallRoad = Resources.Load<Sprite>("Roadmap/Symbols/small_road_blue");
        redSmallRoad = Resources.Load<Sprite>("Roadmap/Symbols/small_road_red");

        blueCockroach = Resources.Load<Sprite>("Roadmap/Symbols/cockroach_blue");
        redCockroach = Resources.Load<Sprite>("Roadmap/Symbols/cockroach_red");
    }

    public void SetSelection(GameResult gameResult, RoadmapData data)
    {
        var indexOfItem = data.Games.IndexOf(gameResult);
        if (indexOfItem == -1)
            return;

        var gamesData = data.Games.Take(indexOfItem + 1);

        var playerForecast = forecastService.GenerateForecastForPlayer(gamesData).Items.ToList();
        var bankerForecast = forecastService.GenerateForecastForBanker(gamesData).Items.ToList();

        SetForecastCell(playerBigEyeRoad, playerForecast[0], blueBigEye, redBigEye);
        SetForecastCell(playerSmallRoad, playerForecast[1], blueSmallRoad, redSmallRoad);
        SetForecastCell(playerCockroachRoad, playerForecast[2], blueCockroach, redCockroach);

        SetForecastCell(bankerBigEyeRoad, bankerForecast[0], blueBigEye, redBigEye);
        SetForecastCell(bankerSmallRoad, bankerForecast[1], blueSmallRoad, redSmallRoad);
        SetForecastCell(bankerCockroachRoad, bankerForecast[2], blueCockroach, redCockroach);
    }

    private void SetForecastCellEmpty(Image image)
    {
        image.enabled = false;
        image.sprite = null;
    }

    private void SetForecastCell(Image image, DerivedRoadItem item, Sprite blue, Sprite red)
    {
        switch (item.Outcome)
        {
            case DerivedOutcome.Blue:
                image.sprite = blue;
                image.enabled = true;
                break;
            case DerivedOutcome.Red:
                image.sprite = red;
                image.enabled = true;
                break;
            case DerivedOutcome.Empty:
                image.enabled = false;
                image.sprite = null;
                break;

        }
    }
}
