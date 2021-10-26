using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BigRoadTrend
{
    /// <summary>
    /// All individual cells that exist in the big road.
    /// </summary>
    public IEnumerable<BigRoadItem> Items { get; set; }

    /// <summary>
    /// The logical column definitions of the big road.
    /// </summary>
    public IEnumerable<BigRoadColumn> ColumnDefinitions { get; set; }
}