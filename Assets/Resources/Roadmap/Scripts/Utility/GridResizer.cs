using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// [ExecuteInEditMode]
public class GridResizer : MonoBehaviour
{
    public GameObject container;
    public int rows;
    private GridLayoutGroup grid;

    public event Action GridResized;


    // Start is called before the first frame update
    void Start()
    {


    }

    bool firstTime = false;

    // Update is called once per frame
    void Update()
    {
        if (!firstTime)
        {
            grid = GetComponent<GridLayoutGroup>();

            var height = container.GetComponent<RectTransform>().rect.height;

            if (height > 0)
            {
                var cellSize = height / rows;
                grid.cellSize = new Vector2(cellSize, cellSize);
                firstTime = true;

                GridResized?.Invoke();
            }
        }
    }
}
