// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogPayout : Schema {
	[Type(0, "uint32")]
	public uint playerPayout = 0;

	[Type(1, "uint32")]
	public uint bankerPayout = 0;

	[Type(2, "uint32")]
	public uint tiePayout = 0;

	[Type(3, "uint32")]
	public uint koi8Payout = 0;

	[Type(4, "uint32")]
	public uint jinChan7Payout = 0;

	[Type(5, "uint32")]
	public uint nineOverOnePayout = 0;

	[Type(6, "uint32")]
	public uint natural9Over7Payout = 0;

	[Type(7, "uint32")]
	public uint any8Over6Payout = 0;

	[Type(8, "int32")]
	public int totalPayout = 0;

	[Type(9, "boolean")]
	public bool isPass = false;
}

