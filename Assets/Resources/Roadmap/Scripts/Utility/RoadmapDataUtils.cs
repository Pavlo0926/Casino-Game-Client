using Colyseus.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RoadmapDataUtils
{
    public static IList<GameResult> ServerGameHistoryToGameResults(IList<GoldenFrogGameHistory> gameHistories, MapSchema<GoldenFrogPayout> playerPayouts)
    {
        foreach (var gameHistory in gameHistories)
        {
            var keyInt = (int)gameHistory.gameNumber;
            var key = keyInt.ToString();
            if (playerPayouts.ContainsKey(key))
            {
                gameHistory.payout = playerPayouts[key];
            }
            else
            {
                gameHistory.payout = null;
            }
        }

        return gameHistories.Select(ServerGameHistoryToGameResult)
            .ToList();
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
