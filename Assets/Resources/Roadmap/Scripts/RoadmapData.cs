using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RoadmapData
{
    public event Action GamesRefreshed;
    public IReadOnlyList<GameResult> Games { get; private set; } = new List<GameResult>();

    public RoadmapData()
    {
    }

    public void LoadGameData(IEnumerable<GameResult> games)
    {
        Games = games.ToList();

        NotifyGamesRefreshed();
    }

    protected virtual void NotifyGamesRefreshed() => GamesRefreshed?.Invoke();
}
