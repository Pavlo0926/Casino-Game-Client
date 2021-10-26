// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogPlayer : Schema {
	[Type(0, "string")]
	public string sessionId = "";

	[Type(1, "string")]
	public string playerId = "";

	[Type(2, "string")]
	public string playerName = "";

	[Type(3, "string")]
	public string facebookId = "";

	[Type(4, "uint32")]
	public uint seatNumber = 0;

	[Type(5, "uint32")]
	public uint credits = 0;

	[Type(6, "boolean")]
	public bool isConnected = false;

	[Type(7, "int32")]
	public int playerShoeStartTime = 0;

	[Type(8, "boolean")]
	public bool isObserving = false;

	[Type(9, "ref", typeof(GoldenFrogWager))]
	public GoldenFrogWager wager = new GoldenFrogWager();

	[Type(10, "ref", typeof(GoldenFrogPayout))]
	public GoldenFrogPayout payout = new GoldenFrogPayout();

	[Type(11, "ref", typeof(GoldenFrogPlayerShoeStats))]
	public GoldenFrogPlayerShoeStats shoeStats = new GoldenFrogPlayerShoeStats();

	[Type(12, "map", typeof(MapSchema<GoldenFrogPayout>))]
	public MapSchema<GoldenFrogPayout> payoutHistory = new MapSchema<GoldenFrogPayout>();
}

