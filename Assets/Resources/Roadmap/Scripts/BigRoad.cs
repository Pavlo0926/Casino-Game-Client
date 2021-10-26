using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BigRoad : MonoBehaviour
{
    private GridLayoutGroup grid;

    private BigRoadService service;

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

    private GameObject cellPrefab;

    private RoadmapCell selectedCell;

    private ScrollRectEnsureVisible ensureVisible;

    // Should the big road data source be scrolled or should a user scroll handle it.
    public bool autoScrollBigRoadToVisible = false;

    public bool enableSelection = true;

    public int prepopulateRows = 0;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        ensureVisible = GetComponentInParent<ScrollRectEnsureVisible>();

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

        cellPrefab = Resources.Load<GameObject>("Roadmap/Prefabs/Cell");


        GridLayoutUtils.AssignSpotsToGrid(grid.gameObject);

        service = new BigRoadService();
    }

    void Start()
    {
        if (!autoScrollBigRoadToVisible)
        {
            GetComponent<GridResizer>().GridResized += () => GridLayoutUtils.FillRowsToEnd(grid.gameObject, cellPrefab, enableSelection, prepopulateRows);
        }
        else
        {
            for (var i = 0; i < prepopulateRows; i++)
            {
                GridLayoutUtils.AddNewRow(cellPrefab, grid.gameObject, 6, enableSelection);
            }
        }
    }

    private RoadmapData data;
    public void SaveData(RoadmapData data)
    {
        this.data = data;
    }

    public void LoadData() => LoadData(data);

    public void LoadData(RoadmapData data)
    {
        Clear();

        BigRoadTrend results = null;
        if (!autoScrollBigRoadToVisible)
            results = service.GenerateLatestBigRoadForTerminal(data);
        else
            results = service.GenerateLatestBigRoadForTerminal(data.Games, true, prepopulateRows - 1);

        foreach (var result in results.Items)
        {
            var cell = GridLayoutUtils.GetCell(grid.gameObject, result.Column, result.Row);

            if (cell == null)
            {
                GridLayoutUtils.AddNewRow(cellPrefab, grid.gameObject, 6, enableSelection);
                cell = GridLayoutUtils.GetCell(grid.gameObject, result.Column, result.Row);
            }

            var gameHistory = result.Game?.Tag as GoldenFrogGameHistory;

            switch (result.Outcome)
            {
                case BigRoadOutcome.Banker:
                    if (gameHistory.evaluation.isJinChan7)
                    {
                        cell.SetBaseImage(jinChan7);
                    }
                    else if (gameHistory.evaluation.isAny8Over6)
                    {
                        cell.SetBaseImage(any8Over6Banker);
                    }
                    else if (gameHistory.evaluation.isNatural9Over7)
                    {
                        cell.SetBaseImage(natural9Over7Banker);
                    }
                    else if (gameHistory.evaluation.isNineOverOne)
                    {
                        cell.SetBaseImage(nineOver1Banker);
                    }
                    else
                    {
                        cell.SetBaseImage(banker);
                    }
                    break;
                case BigRoadOutcome.Player:
                    if (gameHistory.evaluation.isKoi8)
                    {
                        if (gameHistory.evaluation.isAny8Over6)
                        {
                            cell.SetBaseImage(any8Over6WithKoi);
                        }
                        else
                        {
                            cell.SetBaseImage(koi8);
                        }
                    }
                    else if (gameHistory.evaluation.isAny8Over6)
                    {
                        cell.SetBaseImage(any8Over6Player);
                    }
                    else if (gameHistory.evaluation.isNatural9Over7)
                    {
                        cell.SetBaseImage(natural9Over7Player);
                    }
                    else if (gameHistory.evaluation.isNineOverOne)
                    {
                        cell.SetBaseImage(nineOver1Player);
                    }
                    else
                    {
                        cell.SetBaseImage(player);
                    }
                    break;
            }

            cell.Game = result.Game;

            if (result.Ties > 0)
            {
                cell.ShowTies(result.Ties);
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
        if (enableSelection)
        {
            var results = service.GenerateLatestBigRoadForTerminal(data);

            var result = results.Items.FirstOrDefault(i => i.Game == gameResult);

            var cell = GridLayoutUtils.GetCell(grid.gameObject, result.Column, result.Row);

            ClearSelection();

            if (cell != null)
            {
                cell.Select();
                selectedCell = cell;
                ensureVisible.CenterOnItem(cell.GetComponent<RectTransform>());
            }
        }

    }

    public void ClearSelection()
    {
        if (enableSelection)
        {
            if (selectedCell != null)
            {
                selectedCell.Deselect();
                selectedCell = null;
            }
        }
    }
}
