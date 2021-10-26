using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using NativeWebSocket;
using UnityEngine;

public class RoomListingMetadata
{
    public int minimumBet;
    public GoldenFrogTableInformation tableInformation;

    public GoldenFrogGameHistory[] gameHistory;

    public PlayerMetadataItem[] players;

    public static RoomListingMetadata FromIndexedDictionary(IndexedDictionary<string, object> data)
    {
        var metaData = new RoomListingMetadata();
        var jsonData = Json.SerializeToString(data);

        return Json.Deserialize<RoomListingMetadata>(jsonData);
    }
}

public class PlayerMetadataItem
{
    public string playerId;
    public string playerName;
    //public string facebookId;
    public string seatNumber;
    public string credits;
}

public class RoomListingData
{
  public int clients;
  public bool locked;
  //public bool private;
  public int maxClients;
  public RoomListingMetadata metadata;
  public string name;
  public string processId;
  public string roomId;
  public bool unlisted;

  public static RoomListingData FromIndexedDictionary(IndexedDictionary<string, object> data)
  {
      var roomData = new RoomListingData();
      var json = Json.SerializeToString(data);

      return Json.Deserialize<RoomListingData>(json);
    //   roomData.clients = (byte)data["clients"];
    //   roomData.maxClients = (byte)data["maxClients"];

    //   var metadata = data["metadata"] as IndexedDictionary<string, object>;

    //   roomData.metadata = new RoomListingMetadata();
      
    //   var tableInformation = metadata["tableInformation"] as IndexedDictionary<string, object>;
    //   roomData.metadata = RoomListingMetadata.FromIndexedDictionary(metadata);


    //   return roomData;
  }
}

public class RoomChangedData
{
    public string roomId;
    public RoomListingData data;
}

public class GoldenFrogLobbyClient : MonoBehaviour
{
    private Client client;
    public Room<IndexedDictionary<string, object>> Room { get; private set; }

    private string lastSessionId;
    private string lastRoomId;

    private HomeController homeController;

    public int minimumBetFilter = 100;


    void Awake()
    {
        homeController = GetComponent<HomeController>();
        client = ColyseusManager.Instance.CreateClient(Player.Instance.GameUrl);
    }

    private void OnGoldenFrogRoomError(int code, string message)
    {
        Debug.Log("GoldenFrog Lobby Room Error: " + code + ", " + message);
    }

    private void OnGoldenFrogLeaveRoom(WebSocketCloseCode code)
    {
        if (code == WebSocketCloseCode.Abnormal)
        {
            homeController.loadingView.Show();
            ReconnectToServer();
        }
        //GoldenFrogTableController.Instance.isAcceptingBets = false;
        Debug.Log("Lobby OnLeave: " + code.ToString());
    }

    private async void ReconnectToServer()
    {
        var isConnected = false;

        while (!isConnected)
        {
            try
            {
                await Task.Delay(1000);

                await JoinLobbyRoom();

                isConnected = true;
                lastRoomId = Room.Id;
            }
            catch (MatchMakeException ee)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    private void OnRoomsMessage(RoomListingData[] data)
    {
        homeController.LoadTables(data);
    }

    private void OnRoomChangedMessage(List<object> data)
    {
        var roomId = data[0] as string;
        var changes = data[1] as IndexedDictionary<string, object>;

        var roomListingData = RoomListingData.FromIndexedDictionary(changes);

        homeController.UpdateTable(roomId, roomListingData);
    }

    public async Task LeaveLobbyRoom()
    {
        if (Room != null)
        {
            await Room.Leave();
            Room = null;
        }
    }

    public async Task JoinLobbyRoom()
    {
        Debug.Log("Joining golden frog lobby room: " + Player.Instance.GameUrl);
        var filter = new Dictionary<string, object>();
        filter.Add("filter", new Dictionary<string, object>() 
        {
            {"name", "goldenFrog"},
            {"metadata", new Dictionary<string, object>()
            {
                {"minimumBet", minimumBetFilter}
            }}
        });
        Room = await client.JoinOrCreate<IndexedDictionary<string, object>>("lobby", filter);
        lastRoomId = Room.Id;
        lastSessionId = Room.SessionId;
        Room.OnMessage<RoomListingData[]>("rooms", OnRoomsMessage);
        Room.OnMessage<List<object>>("+", OnRoomChangedMessage);
        Room.OnLeave += OnGoldenFrogLeaveRoom;
        Room.OnError += OnGoldenFrogRoomError;
        
        Debug.Log($"Connected to lobby with session id: {Room.SessionId}");
    }

    private void OnGoldenFrogStateChanged(RoomAvailable state, bool isFirstState)
    {
        if (isFirstState)
        {
        }
    }

    public Task FilterTables(int minBet)
    {
        minimumBetFilter = minBet;

        return Room.Send("filter", new {
            name = "goldenFrog",
            metadata = new {
                minimumBet = minimumBetFilter
            }
        });
    }
}
