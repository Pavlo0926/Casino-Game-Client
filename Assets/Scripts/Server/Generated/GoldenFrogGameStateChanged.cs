// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogGameStateChanged : Schema {
	[Type(0, "string")]
	public string gameState = "";

	[Type(1, "int32")]
	public int gameStateCountdown = 0;
}

