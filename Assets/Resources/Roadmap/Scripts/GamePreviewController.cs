using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePreviewController : MonoBehaviour
{
    public Text payoutAmount;

    public GamePreview playerPreview;
    public GamePreview bankerPreview;

    public Color payoutAmountColorNegative;
    public Color payoutAmountColorPositive;

    public void SetSelection(GameResult game, RoadmapData data)
    {
        var gameHistory = game.Tag as GoldenFrogGameHistory;

        var playerCards = new List<string>() { gameHistory.table.playerCard1, gameHistory.table.playerCard2, gameHistory.table.playerCard3 };
        var bankerCards = new List<string>() { gameHistory.table.bankerCard1, gameHistory.table.bankerCard2, gameHistory.table.bankerCard3 };

        playerPreview.SetCards(playerCards);
        bankerPreview.SetCards(bankerCards);

        SetPayout(gameHistory.payout);
    }

    private void SetPayout(GoldenFrogPayout payout)
    {
        if (payout == null)
        {
            payoutAmount.text = "-";
            payoutAmount.color = Color.gray;
        }
        else
        {
            if (payout.isPass)
            {
                payoutAmount.text = "PASS";
                payoutAmount.color = Color.black;
            }
            else
            {
                if (payout.totalPayout < 0)
                {
                    payoutAmount.color = payoutAmountColorNegative;
                    payoutAmount.text = string.Format($"{payout.totalPayout.ToString("n0")}");
                }
                else if (payout.totalPayout == 0)
                {
                    payoutAmount.color = Color.black;
                    payoutAmount.text = "0";
                }
                else
                {
                    payoutAmount.color = payoutAmountColorPositive;
                    payoutAmount.text = string.Format($"+{payout.totalPayout.ToString("n0")}");
                }
            }
        }
    }
}
