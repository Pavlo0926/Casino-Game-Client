using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DerivedRoad
{
    public IEnumerable<DerivedRoadItem> GenerateCycleResultsForBigRoad(BigRoadTrend bigRoadTrend, int cycleLength)
    {
        var derivedRoadItemsWithoutPositions = GenerateOutcomeList(bigRoadTrend, cycleLength)
            .Select(outcome => new DerivedRoadItem()
            {
                Outcome = outcome
            });

        var derivedDrawingEngine = new DerivedRoadDrawingEngine();
        var assignedSpots = derivedDrawingEngine.AssignSpotsToRoadmapList(derivedRoadItemsWithoutPositions);

        return assignedSpots;
    }

    private IEnumerable<DerivedOutcome> GenerateOutcomeList(BigRoadTrend bigRoadTrend, int cycleLength)
    {
        /*
            1.    Let k be the Cycle of the roadmap.  k = 1 for big eye road.
            2.    Assume that the last icon added to the 大路 (Big Road) is on row m of column n.
            2.a.    If m >= 2, we compare with column (n-k).
            2.a.1.    If there is no such column (i.e. before the 1st column) …  No need to add any icon.
            2.a.2.    If there is such a column, and the column has p icons.
            2.a.2.1.    If m <= p  …  The answer is red.
            2.a.2.2.    If m = p + 1  …  The answer is blue.
            2.a.2.3.    If m > p + 1  … The answer is red.
            2.b.    If m = 1, reverse the result (Banker to Player, and vice versa), determine the result as in rule 2.a above, and reverse the answer (Red to blue, and vice versa) to get the real answer.
         */
        var k = cycleLength;
        var outcomes = new List<DerivedOutcome>();

        foreach (var bigRoadColumn in bigRoadTrend.ColumnDefinitions)
        {
            var outcome = DerivedOutcome.Blue;
            var n = bigRoadColumn.LogicalColumn;

            // Nothing can be computed until enough big road columns exist to go past the cycle amount.
            //if (n <= k)
            //    continue;

            for (var m = 0; m < bigRoadColumn.LogicalColumnDepth; m++)
            {
                var rowMDepth = m + 1;

                if (rowMDepth >= 2)
                {
                    var compareColumn = n - k;

                    // Step 2.a.1 - No column exists here.
                    if (compareColumn <= 0)
                        continue;

                    // Step 2.a.1
                    var pColumn = bigRoadTrend.ColumnDefinitions.FirstOrDefault(c => c.LogicalColumn == compareColumn);
                    if (pColumn == null)
                        continue;

                    var p = pColumn.LogicalColumnDepth;
                    if (rowMDepth <= p)
                    {
                        outcome = DerivedOutcome.Red;
                    }
                    else if (rowMDepth == (p + 1))
                    {
                        outcome = DerivedOutcome.Blue;
                    }
                    else if (rowMDepth > (p + 1))
                    {
                        outcome = DerivedOutcome.Red;
                    }

                    outcomes.Add(outcome);

                }
                else
                {
                    var kDistanceColumn = n - (k + 1);
                    var leftColumn = n - 1;

                    var kDistanceColumnDefinition = bigRoadTrend.ColumnDefinitions.FirstOrDefault(c => c.LogicalColumn == kDistanceColumn);
                    var leftColumnDefinition = bigRoadTrend.ColumnDefinitions.FirstOrDefault(c => c.LogicalColumn == leftColumn);

                    if (kDistanceColumnDefinition != null &&
                        leftColumnDefinition != null)
                    {
                        if (kDistanceColumnDefinition.LogicalColumnDepth == leftColumnDefinition.LogicalColumnDepth)
                            outcome = DerivedOutcome.Red;
                        else
                            outcome = DerivedOutcome.Blue;

                        outcomes.Add(outcome);
                    }
                }
            }
        }

        return outcomes;
    }
}