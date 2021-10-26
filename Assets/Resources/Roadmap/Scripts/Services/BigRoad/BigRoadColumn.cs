using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Big road column definition.  Contains helpers to determine column assignment without the difficulty of 
/// dragon tail drawings.
/// </summary>
public class BigRoadColumn
{
    /// <summary>
    /// The logical column number of this column.  This is the column number that the column would occupy if 
    /// no drawing items (such as dragon tails) forced it to another location.
    /// </summary>
    public int LogicalColumn { get; set; }

    /// <summary>
    /// The depth of the logical column while also considering wrapping and dragon tails.
    /// </summary>
    public int LogicalColumnDepth { get; set; }

    /// <summary>
    /// All columns contain the same outcome (logically).  There may be more than one outcome drawn in a column 
    /// but that is due to wrapping and dragon tails.  This only considers the column that has an unlimited number of 
    /// rows below it to store its outcomes.
    /// </summary>
    public BigRoadOutcome Outcome { get; set; }
}
