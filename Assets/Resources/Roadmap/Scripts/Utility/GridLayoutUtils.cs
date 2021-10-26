using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

static class GridLayoutUtils
{
    public static void FillRowsToEnd(GameObject grid, GameObject cellPrefab, bool enableSelection = true, int prepopulateRows = 0)
    {
        var parent = grid.transform.parent.gameObject;
        var height = parent.GetComponent<RectTransform>().rect.height;
        var width = parent.GetComponent<RectTransform>().rect.width;
        var rows = 6;

        if (height > 0 && width > 0)
        {
            var cellSize = height / rows;

            for (var i = 0; i < width; i += (int)cellSize)
            {
                GridLayoutUtils.AddNewRow(cellPrefab, grid.gameObject, 6, enableSelection);
            }
        }
        else
        {
            for (var i = 0; i < prepopulateRows; i++)
            {
                GridLayoutUtils.AddNewRow(cellPrefab, grid.gameObject, 6, enableSelection);
            }
        }
    }

    public static void AssignSpotsToGrid(GameObject grid)
    {
        var col = 0;
        var row = 0;

        foreach (var cell in grid.transform.GetComponentsInChildren<RoadmapCell>())
        {
            cell.Row = row;
            cell.Column = col;
            cell.gameObject.name = $"{col}x{row}";

            row++;
            if (row > 5)
            {
                col++;
                row = 0;
            }
        }
    }

    public static void AssignDerivedRoadSpotsToGrid(GameObject grid)
    {
        // Derived road "group cells" (parent to blocks of 4 cells)
        var col = 0;
        var row = 0;

        foreach (Transform child in grid.transform)
        {
            var derivedCells = child.GetComponentsInChildren<DerivedRoadmapCell>().ToList();
            var startCol = col * 2;
            var startRow = row * 2;

            if (derivedCells.Count == 4)
            {
                // Top Left
                derivedCells[0].Column = startCol;
                derivedCells[0].Row = startRow;
                derivedCells[0].name = $"{startCol}x{startRow}";

                // Bottom Right
                derivedCells[1].Column = startCol;
                derivedCells[1].Row = startRow + 1;
                derivedCells[1].name = $"{startCol}x{startRow + 1}";


                // Top Right
                derivedCells[2].Column = startCol + 1;
                derivedCells[2].Row = startRow;
                derivedCells[2].name = $"{startCol + 1}x{startRow}";


                // Bottom Right
                derivedCells[3].Column = startCol + 1;
                derivedCells[3].Row = startRow + 1;
                derivedCells[3].name = $"{startCol + 1}x{startRow + 1}";
            }

            child.name = $"{col}x{row}";

            row++;
            if (row > 2)
            {
                col++;
                row = 0;
            }
        }
    }

    public static RoadmapCell GetCell(GameObject grid, int col, int row)
    {
        return grid.transform.GetComponentsInChildren<RoadmapCell>().FirstOrDefault(c => c.Row == row && c.Column == col);
    }

    public static DerivedRoadmapCell GetDerivedCell(GameObject grid, int col, int row)
    {
        return grid.transform.GetComponentsInChildren<DerivedRoadmapCell>()
            .FirstOrDefault(c => c.Row == row && c.Column == col);
    }

    public static void AddNewRow(UnityEngine.Object prefab, GameObject parent, int count, bool isSelectable = true)
    {
        for (var i = 0; i < count; i++)
        {
            var newCell = (GameObject)GameObject.Instantiate(prefab, parent.transform);
            newCell.GetComponent<Button>().enabled = isSelectable;
            
        }

        AssignSpotsToGrid(parent);
    }

    public static void AddNewDerivedRow(UnityEngine.Object prefab, GameObject parent, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var newCell = GameObject.Instantiate(prefab, parent.transform);
        }

        AssignDerivedRoadSpotsToGrid(parent);
    }
}
