using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameResult
{
    public Outcome GameOutcome { get; set; }
    public PairResult PairResult { get; set; }
    public NaturalResult NaturalResult { get; set; }
    public NaturalValue NaturalValue { get; set; }
    public object Tag { get; set; }

    public GoldenFrogPayout Payout { get; set; }

    public bool HasPayoutForGame { get { return Payout != null; } }

}

public enum Outcome
{
    Player,
    Banker,
    Tie,
    Unknown
}

public enum PairResult
{
    Player,
    Both,
    Banker,
    None,
}

public enum NaturalResult
{
    Player,
    Both,
    Banker,
    None,
}

public enum NaturalValue
{
    None,
    Natural8,
    Natural9,
}
