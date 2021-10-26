using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BeadPlate : MonoBehaviour
{
    private RoadmapData data;

    private GridLayoutGroup grid;

    private Sprite banker;
    private Sprite player;
    private Sprite tie;

    private GameObject cellPrefab;

    private RoadmapCell selectedCell;

    private ScrollRectEnsureVisible ensureVisible;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        ensureVisible = GetComponentInParent<ScrollRectEnsureVisible>();
        GridLayoutUtils.AssignSpotsToGrid(grid.gameObject);

        banker = Resources.Load<Sprite>("Roadmap/Symbols/bead_plate_bank");
        player = Resources.Load<Sprite>("Roadmap/Symbols/bead_plate_player");
        tie = Resources.Load<Sprite>("Roadmap/Symbols/bead_plate_tie");

        cellPrefab = Resources.Load<GameObject>("Roadmap/Prefabs/Cell");
    }

    void Start()
    {
        GetComponent<GridResizer>().GridResized += () => GridLayoutUtils.FillRowsToEnd(grid.gameObject, cellPrefab);
    }

    public void LoadData(RoadmapData data)
    {
        Clear();

        var col = 0;
        var row = 0;
        foreach (var game in data.Games.ToList())
        {
            var cell = GridLayoutUtils.GetCell(grid.gameObject, col, row);

            if (cell == null)
            {
                GridLayoutUtils.AddNewRow(cellPrefab, grid.gameObject, 6);
                cell = GridLayoutUtils.GetCell(grid.gameObject, col, row);
            }

            switch (game.GameOutcome)
            {
                case Outcome.Banker:
                    cell.SetBaseImage(banker);
                    break;
                case Outcome.Player:
                    cell.SetBaseImage(player);
                    break;
                case Outcome.Tie:
                    cell.SetBaseImage(tie);
                    break;
            }

            cell.Game = game;

            row++;

            if (row == 6)
            {
                row = 0;
                col++;
            }
        }
    }

    private void Clear()
    {
        foreach (var cell in grid.transform.GetComponentsInChildren<RoadmapCell>())
        {
            cell.Clear();
        }
    }

    public void SetSelection(GameResult gameResult, RoadmapData data)
    {
        var index = data.Games.IndexOf(gameResult);

        if (index == -1)
            return;

        var col = index / 6;
        var row = index % 6;

        var cell = GridLayoutUtils.GetCell(grid.gameObject, col, row);

        if (selectedCell != null)
        {
            selectedCell.Deselect();
            selectedCell = null;
        }

        if (cell != null)
        {
            cell.Select();
            selectedCell = cell;
            ensureVisible.CenterOnItem(cell.GetComponent<RectTransform>());
        }
    }
}
