// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogState : Schema {
	[Type(0, "string")]
	public string gameState = "";

	[Type(1, "int32")]
	public int gameStateCountdown = 0;

	[Type(2, "ref", typeof(GoldenFrogTable))]
	public GoldenFrogTable table = new GoldenFrogTable();

	[Type(3, "ref", typeof(GoldenFrogTableInformation))]
	public GoldenFrogTableInformation tableInformation = new GoldenFrogTableInformation();

	[Type(4, "ref", typeof(GoldenFrogEvaluation))]
	public GoldenFrogEvaluation evaluation = new GoldenFrogEvaluation();

	[Type(5, "array", typeof(ArraySchema<GoldenFrogGameHistory>))]
	public ArraySchema<GoldenFrogGameHistory> gameHistory = new ArraySchema<GoldenFrogGameHistory>();

	[Type(6, "map", typeof(MapSchema<GoldenFrogPlayer>))]
	public MapSchema<GoldenFrogPlayer> players = new MapSchema<GoldenFrogPlayer>();
}

