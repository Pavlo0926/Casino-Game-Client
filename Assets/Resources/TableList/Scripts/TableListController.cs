using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using Doozy.Engine.UI;

public class TableListController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        /// <summary>
        /// Internal representation of our data. Note that the scroller will never see
        /// this, so it separates the data from the layout using MVC principles.
        /// </summary>
        private SmallList<RoomListingData> data = new SmallList<RoomListingData>();

        /// <summary>
        /// This is our scroller we will be a delegate for
        /// </summary>
        public EnhancedScroller scroller;

        /// <summary>
        /// This will be the prefab of each cell in our scroller. The cell view will
        /// hold references to each row sub cell
        /// </summary>
        public EnhancedScrollerCellView cellViewPrefab;

        public int numberOfCellsPerRow = 3;

        /// <summary>
        /// Be sure to set up your references to the scroller after the Awake function. The 
        /// scroller does some internal configuration in its own Awake function. If you need to
        /// do this in the Awake function, you can set up the script order through the Unity editor.
        /// In this case, be sure to set the EnhancedScroller's script before your delegate.
        /// 
        /// In this example, we are calling our initializations in the delegate's Start function,
        /// but it could have been done later, perhaps in the Update function.
        /// </summary>
        void Start()
        {
            // tell the scroller that this script will be its delegate
            scroller.Delegate = this;

            scroller.cellViewVisibilityChanged += onCellViewVisibilityChanged;
        }

    private void onCellViewVisibilityChanged(EnhancedScrollerCellView cellView)
    {
        //((TableListRowView)cellView).LoadData();
        // if (!cellView.active)
        //     return;

        // var rowCellView = (TableListRowView)cellView;
        // Debug.Log("New visible row");

        // foreach (var table in rowCellView.tables)
        // {
        //     table.GetComponentInChildren<BigRoad>().FillRowsToEnd();
        // }
    }

    public void LoadData(RoomListingData[] data)
        {
            this.data = new SmallList<RoomListingData>();
            for (var i = 0; i < data.Length; i++)
                this.data.Add(data[i]);

            // tell the scroller to reload now that we have the data
            scroller.ReloadData();
        }

        public void UpdateListItem(string roomId, RoomListingData data)
        {
            var i = 0;
            for (i = 0; i < this.data.Count; i++)
            {
                if (this.data[i].roomId == roomId)
                {
                    this.data[i] = data;

                    //var row = (TableListRowView)scroller.GetCellViewAtDataIndex(i / numberOfCellsPerRow);
                    //Debug.Log($"Loading Data ID: {i} Table {i % numberOfCellsPerRow}");
                    //row.tables[i % numberOfCellsPerRow].LoadRoomListingData(this.data[i]);
                    //row.SetData(ref this.data, i * numberOfCellsPerRow);
                    break;
                }
            }
            scroller.RefreshActiveCellViews();
            //scroller.ReloadData(scroller.ScrollPosition / scroller.ScrollSize);
        }

        #region EnhancedScroller Handlers

        /// <summary>
        /// This tells the scroller the number of cells that should have room allocated.
        /// For this example, the count is the number of data elements divided by the number of cells per row (rounded up using Mathf.CeilToInt)
        /// </summary>
        /// <param name="scroller">The scroller that is requesting the data size</param>
        /// <returns>The number of cells</returns>
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)data.Count / (float)numberOfCellsPerRow);
        }

        /// <summary>
        /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
        /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
        /// cell size will be the width.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell size</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <returns>The size of the cell</returns>
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 400f;
        }

        /// <summary>
        /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
        /// Some examples of this would be headers, footers, and other grouping cells.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
        /// <returns>The cell for the scroller to use</returns>
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // first, we get a cell from the scroller by passing a prefab.
            // if the scroller finds one it can recycle it will do so, otherwise
            // it will create a new cell.
            TableListRowView cellView = scroller.GetCellView(cellViewPrefab) as TableListRowView;
            

            cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

            // pass in a reference to our data set with the offset for this cell
            cellView.SetData(ref data, dataIndex * numberOfCellsPerRow);

            // return the cell to the scroller
            return cellView;
        }

        #endregion
    }