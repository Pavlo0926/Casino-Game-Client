// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogRebetAction : Schema {
	[Type(0, "string")]
	public string playerSessionId = "";

	[Type(1, "ref", typeof(GoldenFrogWager))]
	public GoldenFrogWager wager = new GoldenFrogWager();
}

