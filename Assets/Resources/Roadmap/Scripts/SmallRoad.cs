using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SmallRoad : MonoBehaviour
{
    private GridLayoutGroup grid;

    private RoadmapData data;
    private DerivedRoad service;
    private BigRoadService bigRoadService;

    private Sprite blueMarker;
    private Sprite redMarker;

    private Object cellPrefab;

    private DerivedRoadmapCell selectedCell;
    private ScrollRectEnsureVisible ensureVisible;

    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        ensureVisible = GetComponentInParent<ScrollRectEnsureVisible>();

        blueMarker = Resources.Load<Sprite>("Roadmap/Symbols/small_road_blue");
        redMarker = Resources.Load<Sprite>("Roadmap/Symbols/small_road_red");

        cellPrefab = Resources.Load("Roadmap/Prefabs/DerivedCell");

        GridLayoutUtils.AssignDerivedRoadSpotsToGrid(grid.gameObject);
        service = new DerivedRoad();
        bigRoadService = new BigRoadService();
    }

    public void LoadData(RoadmapData data)
    {
        Clear();

        var bigRoad = bigRoadService.GenerateLatestBigRoadForTerminal(data);
        var results = service.GenerateCycleResultsForBigRoad(bigRoad, 2);

        foreach (var result in results)
        {
            var cell = GridLayoutUtils.GetDerivedCell(grid.gameObject, result.Column, result.Row);

            if (cell == null)
            {
                GridLayoutUtils.AddNewDerivedRow(cellPrefab, grid.gameObject, 3);
                cell = GridLayoutUtils.GetDerivedCell(grid.gameObject, result.Column, result.Row);
            }

            switch (result.Outcome)
            {
                case DerivedOutcome.Blue:
                    cell.SetBaseImage(blueMarker);
                    break;
                case DerivedOutcome.Red:
                    cell.SetBaseImage(redMarker);
                    break;
            }
        }
    }

    public void Clear()
    {
        foreach (var cell in grid.transform.GetComponentsInChildren<DerivedRoadmapCell>())
        {
            cell.Clear();
        }
    }

    public void SetSelection(GameResult gameResult, RoadmapData data)
    {
        var indexOfItem = data.Games.IndexOf(gameResult);
        if (indexOfItem == -1)
            return;

        var bigRoad = bigRoadService.GenerateLatestBigRoadForTerminal(data.Games.Take(indexOfItem + 1));

        var results = service.GenerateCycleResultsForBigRoad(bigRoad, 2);

        var lastResult = results.LastOrDefault();

        if (lastResult != null)
        {
            var cell = GridLayoutUtils.GetDerivedCell(grid.gameObject, lastResult.Column, lastResult.Row);

            ClearSelection();

            cell.Select();
            selectedCell = cell;
            ensureVisible.CenterOnItem(cell.GetComponent<RectTransform>());
        }
        else
        {
            ClearSelection();
        }
    }

    public void ClearSelection()
    {
        if (selectedCell != null)
        {
            selectedCell.Deselect();
            selectedCell = null;
        }
    }
}
