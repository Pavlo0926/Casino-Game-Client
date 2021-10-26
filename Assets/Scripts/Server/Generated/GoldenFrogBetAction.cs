// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.34
// 

using Colyseus.Schema;

public class GoldenFrogBetAction : Schema {
	[Type(0, "string")]
	public string playerSessionId = "";

	[Type(1, "string")]
	public string betName = "";

	[Type(2, "int32")]
	public int denomination = 0;

	[Type(3, "boolean")]
	public bool isClear = false;
}

