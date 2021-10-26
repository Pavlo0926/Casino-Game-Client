using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DerivedRoadDrawingEngine
{
    public const int Rows = 6;

    /// <summary>
    /// What (x,y) tuple pairs are occupied already.
    /// </summary>
    private Dictionary<Tuple<int, int>, DerivedRoadItem> assignmentMap;

    /// <summary>
    /// The logical spot that will contain the next "differing" outcome.
    /// The spot this result would occupy in a perfect world with no dragon tails.
    /// </summary>
    private int logicalColumn = 0;

    private int highestDrawingColumn = 0;

    private DerivedRoadItem lastEntry;

    public DerivedRoadDrawingEngine()
    {
        Clear();
    }

    private void AssignSpotToItem(DerivedRoadItem item)
    {
        if (lastEntry != null &&
            lastEntry.Outcome != item.Outcome)
        {
            logicalColumn++;
        }


        var probeColumn = logicalColumn;
        var probeRow = 0;
        var done = false;

        while (!done)
        {
            if (IsPositionAvailable(probeRow, probeColumn))
            {
                PlaceItem(item, probeRow, probeColumn, logicalColumn);
                done = true;
            }
            else if (SpotBelowIsEndOfTable(probeRow))
            {
                probeColumn++;
            }
            else if (SpotBelowIsEmpty(probeRow, probeColumn))
            {
                probeRow++;
            }
            else if (SpotBelowIsSameColor(item, probeRow, probeColumn))
            {
                probeRow++;
            }
            else
            {
                probeColumn++;
            }
        }

        highestDrawingColumn = Math.Max(highestDrawingColumn, probeColumn);
    }

    public IEnumerable<DerivedRoadItem> AssignSpotsToRoadmapList(IEnumerable<DerivedRoadItem> items)
    {
        Clear();

        var results = new List<DerivedRoadItem>(items);

        foreach (var item in results)
        {
            AssignSpotToItem(item);
        }

        return results.ToList();
    }

    public IEnumerable<DerivedRoadItem> AssignSpotsToRoadmapList(IEnumerable<DerivedRoadItem> items, int drawingColumns)
    {
        Clear();

        var results = new List<DerivedRoadItem>(items);

        foreach (var item in results)
        {
            AssignSpotToItem(item);
        }

        // Only draw the most recent columns that fit into drawingColumns.
        return ScrollItems(results, drawingColumns);
    }

    private IEnumerable<DerivedRoadItem> ScrollItems(IEnumerable<DerivedRoadItem> items, int drawingColumns)
    {
        // We index columns from 0
        var highestDrawableIndex = drawingColumns - 1;
        var offset = Math.Max(0, highestDrawingColumn - highestDrawableIndex);

        // Get items within the range.
        var validItems = items.Where(item => (item.Column - offset) >= 0).ToList();

        // Reassign their drawing columns to start at 0.
        foreach (var item in validItems)
        {
            item.Column -= offset;
        }

        return validItems;
    }

    private void Clear()
    {
        assignmentMap = new Dictionary<Tuple<int, int>, DerivedRoadItem>();

        logicalColumn = 0;
        highestDrawingColumn = 0;

        lastEntry = null;
    }

    private void PlaceItem(DerivedRoadItem item, int row, int col, int logicalColumn)
    {
        item.Row = row;
        item.Column = col;
        item.LogicalColumn = logicalColumn;

        var tuple = new Tuple<int, int>(col, row);
        assignmentMap.Add(tuple, item);

        lastEntry = item;
    }

    private bool IsPositionAvailable(int row, int col)
    {
        var tuple = new Tuple<int, int>(col, row);

        return !assignmentMap.ContainsKey(tuple);

        //return assignmentMap[col, row] == null;
    }

    private bool SpotBelowIsEndOfTable(int row)
    {
        return (row + 1) >= Rows;
    }

    private bool SpotBelowIsEmpty(int row, int col)
    {
        if (SpotBelowIsEndOfTable(row))
            return false;

        var tuple = new Tuple<int, int>(col, row + 1);

        return !assignmentMap.ContainsKey(tuple);

        //return assignmentMap[col, row + 1] == null;
    }

    private bool SpotBelowIsSameColor(DerivedRoadItem currentTiem, int row, int col)
    {
        if (SpotBelowIsEmpty(row, col))
            return false;

        var tuple = new Tuple<int, int>(col, row + 1);

        var spotBelow = assignmentMap[tuple];

        return spotBelow != null && spotBelow.Outcome == currentTiem.Outcome;
    }
}