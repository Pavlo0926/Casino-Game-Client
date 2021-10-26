using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BigRoadItem
{
    public int Row { get; set; }

    public int Column { get; set; }

    public int LogicalColumn { get; set; }

    public int LowestRow { get; set; } = 11;

    public BigRoadOutcome Outcome { get; set; }

    /// <summary>
    /// The amount of ties that happened immediately after this game or if the outcome
    /// is none then the ties that happened before the first entry.
    /// </summary>
    public int Ties { get; set; }

    public PairResult PairResult { get; set; }

    public NaturalResult NaturalResult { get; set; }

    public GameResult Game { get; set; }
}
