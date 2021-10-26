using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BigRoadService
{
    public BigRoadService()
    {
    }

    public BigRoadTrend GenerateLatestBigRoadForTerminal(IEnumerable<GameResult> games, bool flattenTies, bool AddExtraPlayerHand, bool AddExtraBankerHand)
    {
        var gameResults = games.ToList();
        if (AddExtraPlayerHand || AddExtraBankerHand)
        {
            GameResult result = new GameResult();
            if (AddExtraPlayerHand)
                result.GameOutcome = Outcome.Player;
            if (AddExtraBankerHand)
                result.GameOutcome = Outcome.Banker;

            gameResults.Add(result);
        }
        var bigRoadItemsList = GenerateItemsList(gameResults, flattenTies);

        var engine = new BigRoadDrawingEngine(6);
        var drawingItemsList = engine.AssignSpotsToBigRoadList(bigRoadItemsList);
        var columnDefinitionsList = CreateLogicalColumnDefinitions(drawingItemsList);

        return new BigRoadTrend()
        {
            ColumnDefinitions = columnDefinitionsList,
            Items = drawingItemsList
        };
    }

    public BigRoadTrend GenerateLatestBigRoadForTerminal(RoadmapData data, bool flattenTies, bool AddExtraPlayerHand, bool AddExtraBankerHand)
    {
        return GenerateLatestBigRoadForTerminal(data.Games, flattenTies, AddExtraPlayerHand, AddExtraBankerHand);
    }

    public BigRoadTrend GenerateLatestBigRoadForTerminal(IEnumerable<GameResult> games, bool flattenTies = true, int drawingColumns = -1)
    {
        var gameResults = games.ToList();
        var bigRoadItemsList = GenerateItemsList(gameResults, flattenTies);

        int rows = 6;

        var engine = new BigRoadDrawingEngine(rows);
        IEnumerable<BigRoadItem> drawingItemsList = new List<BigRoadItem>();
        if (drawingColumns == -1)
            drawingItemsList = engine.AssignSpotsToBigRoadList(bigRoadItemsList);
        else
            drawingItemsList = engine.AssignSpotsToBigRoadList(bigRoadItemsList, drawingColumns);

        var columnDefinitionsList = CreateLogicalColumnDefinitions(drawingItemsList);

        return new BigRoadTrend()
        {
            ColumnDefinitions = columnDefinitionsList,
            Items = drawingItemsList
        };
    }


    public BigRoadTrend GenerateLatestBigRoadForTerminal(RoadmapData data, bool flattenTies = true)
    {
        return GenerateLatestBigRoadForTerminal(data.Games, flattenTies);
    }

    public BigRoadTrend GenerateLatestBigRoadForTerminal(RoadmapData data, int drawingColumns, bool flattenTies = true)
    {
        return GenerateLatestBigRoadForTerminal(data.Games, flattenTies, drawingColumns);
    }

    private IEnumerable<BigRoadItem> GenerateItemsList(IEnumerable<GameResult> gameResults, bool flattenTies = true)
    {
        IEnumerable<BigRoadItem> bigRoadItemList = null;
        var consecutiveItemsList = new List<List<GameResult>>();

        var currentSequence = gameResults.Skip(0);
        var currentItem = gameResults.FirstOrDefault();

        // Create a list of lists containing consecutive elements to aid creation.
        // Ex: [[Tie, Tie, Tie], [Banker], [Player, Player]] etc...
        while (currentItem != null)
        {
            var sameOutcomes = currentSequence.TakeWhile(g => g.GameOutcome == currentItem.GameOutcome).ToList();

            if (currentItem.GameOutcome != Outcome.Unknown)
            {
                consecutiveItemsList.Add(sameOutcomes);
            }

            currentSequence = currentSequence.Skip(sameOutcomes.Count);

            currentItem = currentSequence.FirstOrDefault();
        }

        if (flattenTies)
        {
            bigRoadItemList = FlattenTies(consecutiveItemsList);
        }
        else
        {
            bigRoadItemList = new List<BigRoadItem>(consecutiveItemsList.SelectMany(gameResultList => gameResultList.Select(gameResult =>
            {
                var outcome = BigRoadOutcome.None;

                if (gameResult.GameOutcome == Outcome.Player)
                    outcome = BigRoadOutcome.Player;
                else if (gameResult.GameOutcome == Outcome.Banker)
                    outcome = BigRoadOutcome.Banker;

                return new BigRoadItem()
                {
                    Outcome = outcome,
                    PairResult = gameResult.PairResult,
                    NaturalResult = gameResult.NaturalResult,
                    Ties = 0,
                    Game = gameResult
                };
            })));
        }

        return bigRoadItemList;
    }

    private IEnumerable<BigRoadItem> FlattenTies(List<List<GameResult>> consecutiveItemsList)
    {
        var bigRoadItemList = new List<BigRoadItem>();
        var firstItemWasTie = false;

        foreach (var consecutiveList in consecutiveItemsList)
        {
            var head = consecutiveList.First();
            var lastHandInBigRoad = bigRoadItemList.LastOrDefault();

            // If this is a run of ties then we need to mark the last game with the tie count.
            // If no last game exists then these ties must go on the next game result of a banker/player win.
            if (head.GameOutcome == Outcome.Tie)
            {
                var tieCount = consecutiveList.Count;

                // The road began with a tie, must wait for next hand to add.
                if (lastHandInBigRoad == null)
                {
                    bigRoadItemList.Add(new BigRoadItem()
                    {
                        Outcome = BigRoadOutcome.None,
                        Ties = tieCount,
                    });

                    firstItemWasTie = true;
                }
                else
                {
                    // Don't add this tie.  Add it to the previous game.
                    lastHandInBigRoad.Ties = tieCount;
                }
            }
            else
            {
                // Just add each item since it's not a tie and requires special processing.
                foreach (var game in consecutiveList)
                {
                    bigRoadItemList.Add(new BigRoadItem()
                    {
                        Ties = firstItemWasTie ? lastHandInBigRoad.Ties : 0,
                        PairResult = game.PairResult,
                        NaturalResult = game.NaturalResult,
                        Outcome = game.GameOutcome == Outcome.Player ? BigRoadOutcome.Player : BigRoadOutcome.Banker,
                        Game = game
                    });

                    if (firstItemWasTie)
                    {
                        bigRoadItemList.Remove(lastHandInBigRoad);
                        firstItemWasTie = false;
                    }
                }
            }
        }

        return bigRoadItemList;
    }

    private IEnumerable<BigRoadColumn> CreateLogicalColumnDefinitions(IEnumerable<BigRoadItem> items)
    {
        var columnDictionary = new SortedDictionary<int, BigRoadColumn>();

        foreach (var item in items)
        {
            if (!columnDictionary.ContainsKey(item.LogicalColumn))
            {
                var bigRoadColumn = new BigRoadColumn()
                {
                    LogicalColumn = item.LogicalColumn,
                    LogicalColumnDepth = 1,
                    Outcome = item.Outcome
                };

                columnDictionary.Add(item.LogicalColumn, bigRoadColumn);
            }
            else
            {
                columnDictionary[item.LogicalColumn].LogicalColumnDepth++;
            }
        }

        return columnDictionary.Values;
    }
}