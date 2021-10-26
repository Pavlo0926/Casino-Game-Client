using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class GameListRowController : MonoBehaviour
{
    public Image GameIcon;

    [Header("Cards")]
    public Image PlayerCard1;
    public Image PlayerCard2;
    public Image PlayerCard3;
    public Image BankerCard1;
    public Image BankerCard2;
    public Image BankerCard3;

    public TextMeshProUGUI PlayerPointTotal;
    public TextMeshProUGUI BankerPointTotal;

    public TextMeshProUGUI GameNumber;

    public TextMeshProUGUI GameTime;

    public TextMeshProUGUI Profit;

    #region Games List Outcome Icons
    private Sprite banker;
    private Sprite player;

    private Sprite jinChan7;
    private Sprite koi8;

    private Sprite any8Over6Player;
    private Sprite any8Over6Banker;
    private Sprite any8Over6WithKoi;

    private Sprite nineOver1Player;
    private Sprite nineOver1Banker;

    private Sprite natural9Over7Player;
    private Sprite natural9Over7Banker;
    #endregion

    void Awake()
    {
        banker = Resources.Load<Sprite>("Roadmap/Symbols/banker");
        player = Resources.Load<Sprite>("Roadmap/Symbols/player");

        jinChan7 = Resources.Load<Sprite>("Roadmap/Symbols/Jinchan-icon");
        koi8 = Resources.Load<Sprite>("Roadmap/Symbols/koi-icon");

        any8Over6Player = Resources.Load<Sprite>("Roadmap/Symbols/gb_8-6_player");
        any8Over6Banker = Resources.Load<Sprite>("Roadmap/Symbols/gb_8-6_banker");
        any8Over6WithKoi = Resources.Load<Sprite>("Roadmap/Symbols/gb_8-6_koi");

        nineOver1Player = Resources.Load<Sprite>("Roadmap/Symbols/gb_9-1_player");
        nineOver1Banker = Resources.Load<Sprite>("Roadmap/Symbols/gb_9-1_banker");

        natural9Over7Player = Resources.Load<Sprite>("Roadmap/Symbols/gb_9-7_player");
        natural9Over7Banker = Resources.Load<Sprite>("Roadmap/Symbols/gb_9-7_banker");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameResult(GameResult gameResult)
    {
        var gameHistory = gameResult.Tag as GoldenFrogGameHistory;
        LoadGameIcon(gameResult);

        SetCard(gameHistory.table.playerCard1, PlayerCard1);
        SetCard(gameHistory.table.playerCard2, PlayerCard2);
        SetCard(gameHistory.table.playerCard3, PlayerCard3);

        SetCard(gameHistory.table.bankerCard1, BankerCard1);
        SetCard(gameHistory.table.bankerCard2, BankerCard2);
        SetCard(gameHistory.table.bankerCard3, BankerCard3);

        SetPoints(new[] { gameHistory.table.playerCard1, gameHistory.table.playerCard2, gameHistory.table.playerCard3 }, PlayerPointTotal);
        SetPoints(new[] { gameHistory.table.bankerCard1, gameHistory.table.bankerCard2, gameHistory.table.bankerCard3 }, BankerPointTotal);

        GameTime.text = DateTimeUtils.UnixTimeStampToDateTime((long)gameHistory.gameTimeUtc).ToString();
        Profit.text = StringFormatUtils.PayoutString(gameHistory.payout.totalPayout);
        GameNumber.text = (gameHistory.gameNumber + 1).ToString();
    }

    public void LoadGameIcon(GameResult gameResult)
    {
        var gameHistory = gameResult.Tag as GoldenFrogGameHistory;

        switch (gameResult.GameOutcome)
        {
            case Outcome.Banker:
                if (gameHistory.evaluation.isJinChan7)
                {
                    GameIcon.sprite = jinChan7;
                }
                else if (gameHistory.evaluation.isAny8Over6)
                {
                    GameIcon.sprite = any8Over6Banker;
                }
                else if (gameHistory.evaluation.isNatural9Over7)
                {
                    GameIcon.sprite = natural9Over7Banker;
                }
                else if (gameHistory.evaluation.isNineOverOne)
                {
                    GameIcon.sprite = nineOver1Banker;
                }
                else
                {
                    GameIcon.sprite = banker;
                }
                break;
            case Outcome.Player:
                if (gameHistory.evaluation.isKoi8)
                {
                    if (gameHistory.evaluation.isAny8Over6)
                    {
                        GameIcon.sprite = any8Over6WithKoi;
                    }
                    else
                    {
                        GameIcon.sprite = koi8;
                    }
                }
                else if (gameHistory.evaluation.isAny8Over6)
                {
                    GameIcon.sprite = any8Over6Player;
                }
                else if (gameHistory.evaluation.isNatural9Over7)
                {
                    GameIcon.sprite = natural9Over7Player;
                }
                else if (gameHistory.evaluation.isNineOverOne)
                {
                    GameIcon.sprite = nineOver1Player;
                }
                else
                {
                    GameIcon.sprite = player;
                }
                break;
        }
    }

    private void SetCard(string card, Image image)
    {
        if (String.IsNullOrEmpty(card))
        {
            image.gameObject.SetActive(false);
        }
        else
        {
            var c = LoadCard(card);
            image.sprite = LoadCard(card);
            image.gameObject.SetActive(true);
        }
    }

    private void SetPoints(IEnumerable<string> cards, TextMeshProUGUI label)
    {
        var pointsTotal = cards.Where(c => !String.IsNullOrEmpty(c)).Sum(CardValue) % 10;
        label.text = pointsTotal.ToString();
    }

    private int CardValue(string card)
    {
        switch (card[0])
        {
            case 'A':
                return 1;
            case 'T':
            case 'K':
            case 'Q':
            case 'J':
                return 10;
            default:
                return int.Parse(card[0].ToString());
        }
    }

    private Sprite LoadCard(string card) => Resources.Load<Sprite>($"Roadmap/Cards/{card[0]}-{card[1].ToString().ToLower()}");
}
