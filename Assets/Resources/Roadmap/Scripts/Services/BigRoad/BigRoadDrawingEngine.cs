using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BigRoadDrawingEngine
{
    public int MaxRows = 5;

    /// <summary>
    /// What (x,y) tuple pairs are occupied already.
    /// </summary>
    private Dictionary<Tuple<int, int>, BigRoadItem> assignmentMap;

    /// <summary>
    /// The logical spot that will contain the next "differing" outcome.
    /// The spot this result would occupy in a perfect world with no dragon tails.
    /// </summary>
    private int logicalColumn = 0;
    private BigRoadOutcome logicalColumnOutcome = BigRoadOutcome.None;

    private int highestDrawingColumn = 0;

    private BigRoadItem lastEntry;

    public BigRoadDrawingEngine(int maxRows)
    {
        MaxRows = maxRows;
        Clear();
    }

    private void AssignSpotToItem(BigRoadItem item, BigRoadItem previousItem)
    {
        /*
         This is the simplest "greedy" method I could come up with for drawing the board.
         The general idea is to go to each logical column where the outcome would be marked if nothing
         was in the way and then keep moving until we find a free spot based on the following steps:

         If currentOutcome != lastOutcome
              logicalColumn++
         ProbeColumn = logicalColumn
         ProbeRow = 0
         If position available:
              PlaceEntryHere
         If spot below is end of table
              ProbeColumn++
         If spot below is empty
              ProbeRow++
         If spot below is the same color
              ProbeRow++
         If position below is different color
              ProbeColumn++

        The only time we turn right is when there is no room below due to a different color
        or there is no more space by height.
         */


        if (lastEntry != null &&
                //lastEntry.Outcome != item.Outcome 
                (item.Outcome != BigRoadOutcome.None && item.Outcome != logicalColumnOutcome))
        {
            logicalColumn++;
            logicalColumnOutcome = item.Outcome;
        }
        //else if (lastEntry != null &&
        //    (previousItem.LowestRow == 0))
        //{
        //    logicalColumn++;
        //    logicalColumnOutcome = item.Outcome;
        //}
        else
        {
            if (lastEntry == null || logicalColumnOutcome == BigRoadOutcome.None)
            {
                logicalColumnOutcome = item.Outcome;
            }
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
                item.LowestRow = probeRow;
                probeColumn++;
            }
            else if (SpotBelowIsEmpty(probeRow, probeColumn) && (previousItem == null || probeRow < previousItem.LowestRow))
            {
                probeRow++;
            }
            else if (SpotBelowIsSameLogicalColumn(item, probeRow, probeColumn, logicalColumn) && (previousItem == null || probeRow < previousItem.LowestRow))
            {
                probeRow++;
            }
            //else if (item.Outcome != previousItem.Outcome && previousItem.LowestRow == 0)
            else if (previousItem.LowestRow == 0)
            {
                probeRow = 0;
                //item.LowestRow = MaxRows;
                item.LowestRow = 0;
                probeColumn++;
                logicalColumn++;
            }
            else
            {
                item.LowestRow = probeRow;
                probeColumn++;
            }
        }

        //item.LowestRow = previousItem == null ? 5 : previousItem.LowestRow;

        highestDrawingColumn = Math.Max(highestDrawingColumn, probeColumn);
    }

    /// <summary>
    /// Assigns a list of big road items their drawing spots on the big road.  The number of drawing columns will 
    /// constrain the resulting list to the most recent hands that fit in the amount of drawing columns.
    /// </summary>
    /// <param name="drawingColumns">The amount of columns that should be drawn.  If there are more items than drawing columns the most recent items that fit in 
    /// the specified drawing columns will be returned.</param>
    public IEnumerable<BigRoadItem> AssignSpotsToBigRoadList(IEnumerable<BigRoadItem> items, int drawingColumns = 36)
    {
        Clear();

        BigRoadItem previousItem = null;
        foreach (var item in items)
        {
            AssignSpotToItem(item, previousItem);
            previousItem = item;
        }

        return ScrollItems(items, drawingColumns);
    }

    public IEnumerable<BigRoadItem> AssignSpotsToBigRoadList(IEnumerable<BigRoadItem> items)
    {
        Clear();

        BigRoadItem previousItem = null;
        foreach (var item in items)
        {
            AssignSpotToItem(item, previousItem);
            previousItem = item;
        }

        return items.ToList();
    }

    private IEnumerable<BigRoadItem> ScrollItems(IEnumerable<BigRoadItem> items, int drawingColumns)
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
        assignmentMap = new Dictionary<Tuple<int, int>, BigRoadItem>();

        logicalColumn = 0;
        highestDrawingColumn = 0;

        lastEntry = null;
    }

    private void PlaceItem(BigRoadItem item, int row, int col, int logicalColumn)
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
    }

    private bool SpotBelowIsEndOfTable(int row)
    {
        return (row + 1) >= MaxRows;
    }

    private bool SpotBelowIsEmpty(int row, int col)
    {
        if (SpotBelowIsEndOfTable(row))
            return false;

        var tuple = new Tuple<int, int>(col, row + 1);

        return !assignmentMap.ContainsKey(tuple);
    }

    private bool SpotBelowIsSameLogicalColumn(BigRoadItem currentItem, int row, int col, int logicalCol)
    {
        if (SpotBelowIsEmpty(row, col))
            return false;

        var tuple = new Tuple<int, int>(col, row + 1);

        var spotBelow = assignmentMap[tuple];

        return spotBelow != null && spotBelow.LogicalColumn == logicalCol;
        //return spotBelow != null && spotBelow.Outcome == currentItem.Outcome;
    }
}