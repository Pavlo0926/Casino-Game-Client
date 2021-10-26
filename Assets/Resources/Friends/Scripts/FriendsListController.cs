using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using Doozy.Engine.UI;
using GameSparks.Api.Responses;

public class FriendsListController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private SmallList<ListGameFriendsResponse._Player> data = new SmallList<ListGameFriendsResponse._Player>();

        /// <summary>
        /// This is our scroller we will be a delegate for
        /// </summary>
        public EnhancedScroller scroller;

        /// <summary>
        /// This will be the prefab of each cell in our scroller. The cell view will
        /// hold references to each row sub cell
        /// </summary>
        public EnhancedScrollerCellView cellViewPrefab;

        void Start()
        {
            scroller.Delegate = this;
        }

    public void LoadData(ListGameFriendsResponse._Player[] data)
        {
            
            this.data = new SmallList<ListGameFriendsResponse._Player>();
            for (var i = 0; i < data.Length; i++)
                this.data.Add(data[i]);

            scroller.ReloadData();
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
            return data.Count;
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
            return 120f;
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
            FriendsListView cellView = scroller.GetCellView(cellViewPrefab) as FriendsListView;

            cellView.name = "Cell " + dataIndex.ToString();

            // in this example, we just pass the data to our cell's view which will update its UI
            cellView.SetData(data[dataIndex]);

            // return the cell to the scroller
            return cellView;
        }

        #endregion
    }
