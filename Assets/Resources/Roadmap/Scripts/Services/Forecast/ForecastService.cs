using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ForecastService : DerivedRoad
{
    private BigRoadService bigRoadService = new BigRoadService();

    public ForecastTrend GenerateForecastForBanker(IEnumerable<GameResult> data)
    {
        var bigRoad = bigRoadService.GenerateLatestBigRoadForTerminal(data, true, false, true);

        return GenerateForecast(bigRoad);
    }

    public ForecastTrend GenerateForecastForPlayer(IEnumerable<GameResult> data)
    {
        var bigRoad = bigRoadService.GenerateLatestBigRoadForTerminal(data, true, true, false);

        return GenerateForecast(bigRoad);
    }

    public ForecastTrend GenerateForecast(BigRoadTrend bigRoadTrend)
    {
        var forcastItems = new List<DerivedRoadItem>();
        var itemsBigEye = GenerateCycleResultsForBigRoad(bigRoadTrend, 1);
        var itemsSmall = GenerateCycleResultsForBigRoad(bigRoadTrend, 2);
        var itemsCockroach = GenerateCycleResultsForBigRoad(bigRoadTrend, 3);
        //var itemsDealerBigEye = GenerateCycleResultsForBigRoad(bigRoadTrend, 1);
        //var itemsDealerSmall = GenerateCycleResultsForBigRoad(bigRoadTrend, 2);
        //var itemsDealerCockroach = GenerateCycleResultsForBigRoad(bigRoadTrend, 3);

        //var EmptyItems = new List<DerivedRoadItem>();
        //EmptyItems.Add()
        var emptyItem = new DerivedRoadItem();
        emptyItem.Outcome = DerivedOutcome.Empty;

        if (itemsBigEye.Count() > 0)
        {
            forcastItems.Add(itemsBigEye.Last());
        }
        else
        {
            forcastItems.Add(emptyItem);
        }

        if (itemsSmall.Count() > 0)
        {
            forcastItems.Add(itemsSmall.Last());
        }
        else
        {
            forcastItems.Add(emptyItem);
        }

        if (itemsCockroach.Count() > 0)
        {
            forcastItems.Add(itemsCockroach.Last());
        }
        else
        {
            forcastItems.Add(emptyItem);
        }
        //forcastItems.Add(itemsDealerBigEye.Last());
        //forcastItems.Add(itemsDealerSmall.Last());
        //forcastItems.Add(itemsDealerCockroach.Last());
        return new ForecastTrend()
        {
            Items = forcastItems
        };
    }
}