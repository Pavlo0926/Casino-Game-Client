using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using NativeWebSocket;
using UnityEngine;

public class GoldenFrogRoomClient : MonoBehaviour
{
    private Client client;
    public Room<GoldenFrogState> Room { get; private set; }

    private string lastSessionId;
    private string lastRoomId;

    private GoldenFrogTableController tableController;

    void Awake()
    {
        tableController = GetComponent<GoldenFrogTableController>();
        client = ColyseusManager.Instance.CreateClient(Player.Instance.GameUrl);
    }

    private void OnGoldenFrogRoomError(int code, string message)
    {
        Debug.Log("OnError: " + code + ", " + message);
        tableController.ShowConnectionLost();
    }

    private void OnGoldenFrogLeaveRoom(WebSocketCloseCode code)
    {
        if (code == WebSocketCloseCode.Abnormal)
        {
            tableController.LoadingView.Show();
            ReconnectToServer();
        }

        //GoldenFrogTableController.Instance.isAcceptingBets = false;
        Debug.Log("OnLeave: " + code.ToString());
    }

    private async void ReconnectToServer()
    {
        var isConnected = false;

        while (!isConnected)
        {
            try
            {
                await Task.Delay(1000);

                Room = await client.Reconnect<GoldenFrogState>(lastRoomId, lastSessionId);
                isConnected = true;
                lastRoomId = Room.Id;
                lastSessionId = Room.SessionId;

                Room.OnStateChange += OnGoldenFrogStateChanged;
                Room.OnMessage<GoldenFrogGameStateChanged>("GameStateChanged", HandleGameStateChangedMessage);
                Room.OnMessage<GoldenFrogClearBetAction>("ClearBetAction", HandleClearBetActionMessage);
                Room.OnMessage<GoldenFrogRebetAction>("RebetAction", HandleRebetActionMessage);
                Room.OnMessage<GoldenFrogBetAction>("PlayerBetAction", HandleBetActionMessage);
                Room.OnLeave += OnGoldenFrogLeaveRoom;
                Room.OnError += OnGoldenFrogRoomError;
            }
            catch (MatchMakeException ee)
            {
                tableController.ShowConnectionLost();
                break;
            }
            // catch (Exception e)
            // {
            //     Debug.LogError(e);
            // }
        }
    }

    public void CreateConnection()
    {
    }

    public async Task Disconnect()
    {
        if (Room != null && Room.Connection.IsOpen)
        {
            try
            {
                await Room.Leave();
            }
            finally
            {
                Room = null;
            }
        }
    }

    public async Task ConnectToServer(string roomId = null)
    {
        var isConnected = false;

        while (!isConnected)
        {
            try
            {
                var joinParams = new Dictionary<string, object>();
                joinParams.Add("gameSparksPlayerId", Player.Instance.UserId);
                Debug.Log("Connectin to: " + Player.Instance.GameUrl);
                if (roomId != null)
                    Room = await client.JoinById<GoldenFrogState>(roomId, joinParams);
                else
                    Room = await client.Join<GoldenFrogState>("goldenFrog", joinParams);
                isConnected = true;
                lastRoomId = Room.Id;
                lastSessionId = Room.SessionId;

                //GoldenFrogTableController.Instance.isAcceptingBets = true;

                Room.OnStateChange += OnGoldenFrogStateChanged;
                Room.OnMessage<GoldenFrogGameStateChanged>("GameStateChanged", HandleGameStateChangedMessage);
                Room.OnMessage<GoldenFrogClearBetAction>("ClearBetAction", HandleClearBetActionMessage);
                Room.OnMessage<GoldenFrogRebetAction>("RebetAction", HandleRebetActionMessage);
                Room.OnMessage<GoldenFrogBetAction>("PlayerBetAction", HandleBetActionMessage);
                Room.OnLeave += OnGoldenFrogLeaveRoom;
                Room.OnError += OnGoldenFrogRoomError;
                
                Debug.Log($"Connected to server with session id: {Room.SessionId}");
            }
            catch (MatchMakeException)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                await Task.Delay(1000);
            }
        }
    }

    private void HandleBetActionMessage(GoldenFrogBetAction obj)
    {
        tableController.HandleBetAction(Room.State, obj);
    }

    private void HandleRebetActionMessage(GoldenFrogRebetAction obj)
    {
        tableController.HandleRebetAction(Room.State, obj);
    }

    private void HandleClearBetActionMessage(GoldenFrogClearBetAction obj)
    {
        tableController.HandleClearBetAction(Room.State, obj);
    }

    private void HandleGameStateChangedMessage(GoldenFrogGameStateChanged message)
    {
        tableController.HandleGameStateChangedMessage(Room.State, message);
    }

    private void OnGoldenFrogStateChanged(GoldenFrogState state, bool isFirstState)
    {
        if (isFirstState)
        {
            tableController.LoadTable(state);
        }
    }

    public async void ClearBet()
    {
        await Room.Send("clearBet");
    }

    public async void Rebet(GoldenFrogWager wager)
    {
        await Room.Send("rebet", new
        {
            wager = new
            {
                bankerWager = wager.bankerWager,
                playerWager = wager.playerWager,
                tieWager = wager.tieWager,

                jinChan7Wager = wager.jinChan7Wager,
                koi8Wager = wager.koi8Wager,

                nineOverOneWager = wager.nineOverOneWager,
                natural9Over7Wager = wager.natural9Over7Wager,
                any8Over6Wager = wager.any8Over6Wager
            }
        });
    }

    public async void Bet(WagerType wType, long denom)
    {
        await Room.Send("bet", new
        {
            betName = wType.ToString(),
            denomination = denom
        });
    }

    public async void AdvanceState()
    {
        await Room.Send("advanceState");
    }
}
