// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogGameHistory : Schema {
	[Type(0, "number")]
	public float gameNumber = 0;

	[Type(1, "int32")]
	public int gameTimeUtc = 0;

	[Type(2, "ref", typeof(GoldenFrogEvaluation))]
	public GoldenFrogEvaluation evaluation = new GoldenFrogEvaluation();

	[Type(3, "ref", typeof(GoldenFrogTable))]
	public GoldenFrogTable table = new GoldenFrogTable();

	[Type(4, "ref", typeof(GoldenFrogPayout))]
	public GoldenFrogPayout payout = new GoldenFrogPayout();
}

