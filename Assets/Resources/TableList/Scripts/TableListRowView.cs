using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class TableListRowView : EnhancedScrollerCellView
{
    public TableListItem[] tables;

    public void LoadData()
    {
        for (var i = 0; i < tables.Length; i++)
        {
            tables[i].bigRoad.LoadData();
        }
    }

    private SmallList<RoomListingData> data;
    private int startingIndex = 0;

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(ref SmallList<RoomListingData> data, int startingIndex)
    {
        this.data = data;
        this.startingIndex = startingIndex;

        // loop through the sub cells to display their data (or disable them if they are outside the bounds of the data)
        for (var i = 0; i < tables.Length; i++)
        {
            if (startingIndex + i < data.Count)
            {
                var dataItem = data[startingIndex + i];

                tables[i].LoadRoomListingData(dataItem);
                tables[i].GetComponent<UIView>().InstantShow();

            }
            else
            {
                tables[i].GetComponent<UIView>().InstantHide();
            }

            // if the sub cell is outside the bounds of the data, we pass null to the sub cell
            //tables[i].SetData(startingIndex + i < data.Count ? data[startingIndex + i] : null);
        }
    }


    public override void RefreshCellView()
    {
        Debug.Log("Refreshing cell view");
        SetData(ref data, startingIndex);
    }
}
