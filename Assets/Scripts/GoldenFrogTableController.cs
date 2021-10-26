using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Doozy.Engine.UI;
using System;
using DG.Tweening;

public enum GameState
{
    WaitingForAction,
    AcceptingBets,
    InitialHandDealt,
    Player3rdDealt,
    Dealer3rdDealt,
    Finished
}

public enum CardType
{
    Player,
    Banker
}

public enum AspectRatio
{
    Width=0,
    Height=1
}

public enum WagerType
{
    Any8Over6,
    Banker,
    Player,
    JinChan7,
    Koi8,
    Natural9Over7,
    NineOverOne,
    Tie,
    None
}

public class GoldenFrogTableController : MonoBehaviour
{
    public TableAvatar PlayerAvatar;
    public TableAvatar[] Avatars;
    public OutcomeNotification OutcomeNotification;
    public PlayerAvatarControls PlayerAvatarControls;
    public ShoeController ShoeController;
    public UIView LoadingView;

    public GoldenFrogCardsController CardsController;
    public TableInformationController TableInformation;


    public TextMeshProUGUI bankerPointTotal;
    public TextMeshProUGUI playerPointTotal;

    public Transform DealerChipCollectPosition;

    private UIPopup roadmap;
    private AnnouncerSounds announcerSounds;
    private GoldenFrogRoomClient goldenFrogRoomClient;
    private AgoraVideoManager agoraVideoManager;
    private Dictionary<string, TableAvatar> sessionIdsToAvatars = new Dictionary<string, TableAvatar>();
    GoldenFrogWager playerWager = new GoldenFrogWager();

    GoldenFrogWager playerOldWager = new GoldenFrogWager();

    int playerTotal, bankerTotal, playerCardsCount, bankerCardsCount;
    public GameObject selectedChip;
    private static GoldenFrogTableController instance;

    public bool isAcceptingBets = true;
    [SerializeField] GameObject SelectorImage;


    [SerializeField] GameObject[] flaresImages;
    [SerializeField] GameObject playerAnimationImage, bankerAnimationImage;

    [SerializeField] float animationTimer = 3f;

    ChipTrayController chipTrayController;
    public static GoldenFrogTableController Instance
    {
        get {
            return instance;
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            //wager = new GoldenFrogWager();

            instance = this;

            goldenFrogRoomClient = GetComponent<GoldenFrogRoomClient>();
            agoraVideoManager = GetComponent<AgoraVideoManager>();
            roadmap = UIPopup.GetPopup("RoadmapPopup");
            chipTrayController = GetComponent<ChipTrayController>();
            announcerSounds = GetComponent<AnnouncerSounds>();
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }

    }

    public static void DestroyInstance()
    {
        DestroyImmediate(instance.gameObject);
        instance = null;
    }

    // Completely loads the table from state.  Used on Initial Load.
    public void LoadTable(GoldenFrogState state)
    {
        var gameState = (GameState)Enum.Parse(typeof(GameState), state.gameState, true);

        // Clear all avatars and table in case of reconnect before loading state.
        sessionIdsToAvatars.Clear();
        foreach (var avatar in Avatars)
            avatar.OpenSeat();

        ResetTable();

        var totalPlayers = Avatars.Length;
        var userPlayerSessionId = goldenFrogRoomClient.Room.SessionId;
        var userPlayerState = state.players[userPlayerSessionId];

        // No matter the players seat number, we always sit them in the middle of the table.
        // Other opponents seat will have to be rotated around to have the same relative representation
        // of the table.
        var playerSeatNumber = userPlayerState.seatNumber;
        var seatOffset = (int)(playerSeatNumber + 1) % totalPlayers;
        foreach (var avatar in Avatars)
        {
            avatar.SeatNumber = (uint)seatOffset;
            seatOffset = (seatOffset + 1) % totalPlayers;
        }

        state.players.ForEach((sessionId, playerState) => SeatPlayerFromState(sessionId, playerState));

        state.players.OnAdd += (playerState, sessionId) => SeatPlayerFromState(sessionId, playerState);
        state.players.OnRemove += (playerState, sessionId)  => RemovePlayerFromTable(sessionId);

        LoadAvatarsWager(state);
        LoadRoadmap(state);
        
        TableInformation.LoadTableInformation(state.tableInformation);
        LoadCardsLeftInShoe(state);

        switch (state.gameState)
        {
            case "WaitingForAction":
            LoadWaitingForActionState(state);
            break;
            case "AcceptingBets":
            LoadAcceptingBetsState(state, state.gameStateCountdown);
            break;
            case "InitialHandDealt":
            LoadInitialHandDealtStatePreDeal(state);
            LoadInitialHandDealtState(state);
            break;
            case "Player3rdDealt":
            LoadPlayer3rdCardDealtState(state);
            break;
            case "Dealer3rdDealt":
            LoadBanker3rdCardDealtState(state);
            break;
            case "Finished":
            LoadGameFinishedLateJoinState(state);
            break;
        }

        // Load Cards
        CardsController.LoadFromTableState(state, gameState);
        LoadingView.Hide();
    }

    public void ShowConnectionLost()
    {
        Alertbox.ShowMessage("Your placed wagers and game results are saved.", "Table connection lost");
        UIManager.Instance.UnloadGamePlayScene(() =>
        {
            UIManager.Instance.ShowHomeScreen();
        });
    }

    private void LoadGameFinishedLateJoinState(GoldenFrogState state)
    {
         LoadRoadmap(state);

        var wagerType = WagerType.None;

        if (state.evaluation.outcome == "banker") 
        {
            wagerType = WagerType.Banker;
        }
        else if (state.evaluation.outcome == "player") 
        {
            wagerType = WagerType.Player;
        }
        else if (state.evaluation.outcome == "tie") 
        {
            wagerType = WagerType.Tie;
        }

        if (state.evaluation.isJinChan7)
        {
            wagerType = WagerType.JinChan7;
        }
        else if (state.evaluation.isKoi8)
        {
            wagerType = WagerType.Koi8;
        }

        OutcomeNotification.ShowOutcomeNotification(wagerType, true);
    }

    public void EnableVideoChattingAvatars()
    {
        var userPlayerSessionId = goldenFrogRoomClient.Room.SessionId;
        // Join the user into the video chat live group.
        var userAvatar = sessionIdsToAvatars[userPlayerSessionId];
        agoraVideoManager.join(goldenFrogRoomClient.Room.Id, userAvatar.VideoChatUid);

        EnablePlayerAvatarVideo();
    }

    public void DisableVideoChattingAvatars()
    {
        agoraVideoManager.leave();

        DisablePlayerAvatarVideo();

        foreach (var avatar in Avatars)
            avatar.HideVideoStream();
    }

    public void EnablePlayerAvatarVideo()
    {
        var userPlayerSessionId = goldenFrogRoomClient.Room.SessionId;
        var userAvatar = sessionIdsToAvatars[userPlayerSessionId];

        userAvatar.ShowVideoStream();
    }

    public void DisablePlayerAvatarVideo()
    {
        var userPlayerSessionId = goldenFrogRoomClient.Room.SessionId;
        var userAvatar = sessionIdsToAvatars[userPlayerSessionId];

        userAvatar.HideVideoStream();
    }

    public void AttachVideoStreamToAvatarByVideoStreamUid(uint videoStreamUid)
    {
        var avatarBySeat = Avatars.FirstOrDefault(a => a.VideoChatUid == videoStreamUid);

        if (avatarBySeat == null)
        {
            Debug.LogWarning("Could not find avatar at video Stream id: " + videoStreamUid);
            return;
        }

        avatarBySeat.AttachVideoStream();
        avatarBySeat.ShowVideoStream();
    }

    public void DetachVideoStreamToAvatarByVideoStreamUid(uint videoStreamUid)
    {
        var avatarBySeat = Avatars.FirstOrDefault(a => a.VideoChatUid == videoStreamUid);

        if (avatarBySeat == null)
        {
            Debug.LogWarning("Could not find avatar at video Stream id: " + videoStreamUid);
            return;
        }

        avatarBySeat.HideVideoStream();
    }

    public void AddPlayerWagerFromDraggable(WagerType wagerType, ChipDraggable chip)
    {
        if (wagerType == WagerType.Banker)
        {
            this.playerWager.bankerWager += chip.amount;
        }
        else if (wagerType == WagerType.Player)
        {
            this.playerWager.playerWager += chip.amount;
        }
        else if (wagerType == WagerType.Tie)
        {
            this.playerWager.tieWager += chip.amount;
        }
        else if (wagerType == WagerType.JinChan7)
        {
            this.playerWager.jinChan7Wager += chip.amount;
        }
        else if (wagerType == WagerType.Koi8)
        {
            this.playerWager.koi8Wager += chip.amount;
        }
        else if (wagerType == WagerType.Natural9Over7)
        {
            this.playerWager.natural9Over7Wager += chip.amount;
        }
        else if (wagerType == WagerType.NineOverOne)
        {
            this.playerWager.nineOverOneWager += chip.amount;
        }
        else if (wagerType == WagerType.Any8Over6)
        {
            this.playerWager.any8Over6Wager += chip.amount;
        }

        StopTableAnimation();
        goldenFrogRoomClient.Bet(wagerType, chip.amount);

    }

    public void SetSelector(GameObject selected)
    {
        selectedChip = selected;
        SelectorImage.SetActive(true);
        if (SelectorImage.GetComponent<iTween>() == null)
        {
            iTween.ScaleTo(SelectorImage, iTween.Hash("x", 1.1f, "y", 1.1f, "speed", 1f, "loopType", iTween.LoopType.pingPong,"easyType", iTween.EaseType.linear));
        }

        SelectorImage.GetComponent<RectTransform>().anchoredPosition = selectedChip.GetComponent<RectTransform>().anchoredPosition;

        Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipPickup");
    }

    public void ResetSelectedChip()
    {
        selectedChip = null;
        SelectorImage.SetActive(false);
    }

    #region Server Message Handlers
    public void HandleGameStateChangedMessage(GoldenFrogState state, GoldenFrogGameStateChanged message)
    {
        Debug.Log($"HandleGameStateChangedMessage -> {message.gameState}");

        LoadAvatarsCredits(state);

        switch (message.gameState)
        {
            case "WaitingForAction":
            LoadAvatarsWager(state);
            ResetTable();
            LoadWaitingForActionState(state);
            break;
            case "AcceptingBets":
            LoadAcceptingBetsState(state, message.gameStateCountdown);
            break;
            case "InitialHandDealt":
                StopTableAnimation();
            LoadAvatarsWager(state);
            HideRoadmap();
            LoadInitialHandDealtStatePreDeal(state);
            CardsController.DealInitialHand(state.table).onComplete += () =>
            {
                LoadInitialHandDealtState(state);
            };
            break;
            case "Player3rdDealt":
            CardsController.Deal3rdPlayerCard(state.table).onComplete += () =>
            {
                LoadPlayer3rdCardDealtState(state);
            };
            break;
            case "Dealer3rdDealt":
            CardsController.Deal3rdBankerCard(state.table).onComplete += () =>
            {
                LoadBanker3rdCardDealtState(state);
            };
            break;
            case "Finished":
            LoadGameFinishedState(state);
            break;
        }
    }

    public void HandleRebetAction(GoldenFrogState state, GoldenFrogRebetAction message)
    {
        var wagerData = state.players[message.playerSessionId].wager;
        
        LoadAvatarsCredits(state);

        if (message.playerSessionId != goldenFrogRoomClient.Room.SessionId)
        {
            LoadAvatarWager(message.playerSessionId, wagerData);
        }

    }

    public void HandleClearBetAction(GoldenFrogState state, GoldenFrogClearBetAction message)
    {
        var wagerData = state.players[message.playerSessionId].wager;
        
        LoadAvatarsCredits(state);

        if (message.playerSessionId != goldenFrogRoomClient.Room.SessionId)
        {
            LoadAvatarWager(message.playerSessionId, new GoldenFrogWager());
        }
    }

    public void HandleBetAction(GoldenFrogState state, GoldenFrogBetAction message)
    {
        var avatar = sessionIdsToAvatars[message.playerSessionId];

        var wagerType = (WagerType)Enum.Parse(typeof(WagerType), message.betName);

        avatar.UpdateBetAction(wagerType, message.denomination);
    }
#endregion
    private void LoadWaitingForActionState(GoldenFrogState state)
    {
        LoadRoadmap(state);
        ShowRoadmap();
        chipTrayController.Show();

        StartTableAnimation();

        Doozy.Engine.Soundy.SoundyManager.Play("Announcer", "Place-Your-Bets");
    }

    private void LoadAcceptingBetsState(GoldenFrogState state, int countdown)
    {
        isAcceptingBets = true;
        PlayerAvatar.Timer.StartCountdown(countdown);

        chipTrayController.Show();

        
    }

    private void LoadInitialHandDealtStatePreDeal(GoldenFrogState state)
    {
        LoadCardsLeftInShoe(state);
        PlayerAvatar.Timer.StopCountdown();
        OutcomeNotification.HideTableAnimations();

        isAcceptingBets = false;
        chipTrayController.Hide();

        Doozy.Engine.Soundy.SoundyManager.Play("Announcer", "Good-Luck");
    }

    private void LoadInitialHandDealtState(GoldenFrogState state)
    {
        playerCardsCount = bankerCardsCount = 2;

        LoadPointTotals(state.table, playerCardsCount, bankerCardsCount);
    }

    private void LoadPlayer3rdCardDealtState(GoldenFrogState state)
    {
        LoadCardsLeftInShoe(state);
        playerCardsCount = 3;
        LoadPointTotals(state.table, playerCardsCount, bankerCardsCount);
    }

    private void LoadBanker3rdCardDealtState(GoldenFrogState state)
    {
        LoadCardsLeftInShoe(state);

        bankerCardsCount = 3;
        LoadPointTotals(state.table, playerCardsCount, bankerCardsCount);
    }

    private void LoadCardsLeftInShoe(GoldenFrogState state)
    {
        ShoeController.LoadFromState(state.table);
    }

    private void LoadGameFinishedState(GoldenFrogState state)
    {
        LoadRoadmap(state);

        var wagerType = WagerType.None;

        if (state.evaluation.outcome == "banker") 
        {
            wagerType = WagerType.Banker;
        }
        else if (state.evaluation.outcome == "player") 
        {
            wagerType = WagerType.Player;
        }
        else if (state.evaluation.outcome == "tie") 
        {
            wagerType = WagerType.Tie;
        }

        if (state.evaluation.isJinChan7)
        {
            wagerType = WagerType.JinChan7;
        }
        else if (state.evaluation.isKoi8)
        {
            wagerType = WagerType.Koi8;
        }

        announcerSounds.PlayGameFinishedSounds(state.evaluation, playerTotal, bankerTotal, () =>
        {
            var losingTopBets = DOTween.Sequence();
            var losingMiddleBets = DOTween.Sequence();
            var losingOutcomeBets = DOTween.Sequence();

            var winningTopBets = DOTween.Sequence();
            var winningMiddleBets = DOTween.Sequence();
            var winningOrPushingOutcomeBets = DOTween.Sequence();

            var dealerPayoutSequence  = DOTween.Sequence();

            var stackToPlayerSequence = DOTween.Sequence();


            state.players.ForEach((sessionId, playerState) =>
            {
                var avatar = sessionIdsToAvatars[sessionId];

                losingTopBets.Insert(0, avatar.DealerCollectLosingTopSideBets(state.evaluation));
                losingMiddleBets.Insert(0, avatar.DealerCollectLosingMiddleBets(state.evaluation));
                losingOutcomeBets.Insert(0, avatar.DealerCollectLosingOutcomeBets(state.evaluation));

                winningTopBets.Insert(0, avatar.DealerPayoutWinningTopSideBets(state.evaluation, playerState.payout, playerState.wager));
                winningMiddleBets.Insert(0, avatar.DealerPayoutWinningMiddleSideBets(state.evaluation, playerState.payout, playerState.wager));
                winningOrPushingOutcomeBets.Insert(0, avatar.DealerPayoutWinningOrPushedOutcomeBets(state.evaluation, playerState.payout, playerState.wager));

                var moveToSeq = avatar.WinningChipsCollectToPlayer(playerState.payout);
                moveToSeq.onComplete += () =>
                {
                    if (!playerState.payout.isPass)
                        avatar.UpdateCreditDelta(playerState.payout.totalPayout);
                };
                stackToPlayerSequence.Insert(0, moveToSeq);
            });

            // Collect losers row by row
            var dealerCollectionSequence = DOTween.Sequence();
            dealerCollectionSequence.Append(losingTopBets);
            dealerCollectionSequence.Append(losingMiddleBets.SetDelay(0.5f));
            dealerCollectionSequence.Append(losingOutcomeBets.SetDelay(0.5f));

            // Pay winners row by row
            dealerPayoutSequence.Append(winningTopBets.SetDelay(0.5f));
            dealerPayoutSequence.Append(winningMiddleBets.SetDelay(0.5f));
            dealerPayoutSequence.Append(winningOrPushingOutcomeBets.SetDelay(0.5f));
            dealerCollectionSequence.Append(dealerPayoutSequence);

            // Move chips to player avatar
            dealerCollectionSequence.Append(stackToPlayerSequence.SetDelay(1f));


            dealerCollectionSequence.onComplete += () =>
            {
                // Notify server our animations are complete
                goldenFrogRoomClient.AdvanceState();
            };

            ShowOutcomeAnimation(wagerType, state.evaluation);

            if (state.evaluation.outcome == "banker")
            {
                CardsController.NudgeBankerHand();
            }
            else if (state.evaluation.outcome == "player")
            {
                CardsController.NudgePlayerHand();
            }
            else if (state.evaluation.outcome == "tie")
            {
                CardsController.NudgeBankerHand();
                CardsController.NudgePlayerHand();
            }
        });
    }

    private void ResetTable()
    {

        if(this.playerWager != null)
        {
        this.playerOldWager.playerWager = this.playerWager.playerWager;
        this.playerOldWager.bankerWager = this.playerWager.bankerWager;
        this.playerOldWager.tieWager = this.playerWager.tieWager;
        this.playerOldWager.jinChan7Wager = this.playerWager.jinChan7Wager;
        this.playerOldWager.koi8Wager = this.playerWager.koi8Wager;
        this.playerOldWager.any8Over6Wager = this.playerWager.any8Over6Wager;
        this.playerOldWager.nineOverOneWager = this.playerWager.nineOverOneWager;
        this.playerOldWager.natural9Over7Wager = this.playerWager.natural9Over7Wager;
        }

        OutcomeNotification.HideTableAnimations();
        playerWager = new GoldenFrogWager();

        playerPointTotal.text = bankerPointTotal.text = "";

        foreach (var avatar in Avatars)
            avatar.Reset();

        CardsController.Clear();

        isAcceptingBets = true;
        
        OutcomeNotification.HideOutcomeNotification();
    }

    public void ClearBet()
    {
        this.playerWager.bankerWager = this.playerWager.playerWager = this.playerWager.tieWager = this.playerWager.nineOverOneWager = this.playerWager.natural9Over7Wager = this.playerWager.koi8Wager = this.playerWager.jinChan7Wager = this.playerWager.any8Over6Wager = 0;
        this.PlayerAvatar.Reset();
        goldenFrogRoomClient.ClearBet();
    }

    public void PlaceSameBetAgain()
    {
        if(playerOldWager != null)
        {
            this.playerWager.playerWager = this.playerOldWager.playerWager;
            this.playerWager.bankerWager = this.playerOldWager.bankerWager;
            this.playerWager.tieWager = this.playerOldWager.tieWager;
            this.playerWager.jinChan7Wager = this.playerOldWager.jinChan7Wager;
            this.playerWager.koi8Wager = this.playerOldWager.koi8Wager;
            this.playerWager.any8Over6Wager = this.playerOldWager.any8Over6Wager;
            this.playerWager.nineOverOneWager = this.playerOldWager.nineOverOneWager;
            this.playerWager.natural9Over7Wager = this.playerOldWager.natural9Over7Wager;
        this.PlayerAvatar.UpdateWager(playerWager);
        goldenFrogRoomClient.Rebet(this.playerWager);
        }
    }
    

    private void LoadPointTotals(GoldenFrogTable table, int playerCards, int bankerCards)
    {
        this.playerTotal = 0;
        this.bankerTotal = 0;

        if (playerCards >= 2)
        {
            this.playerTotal += CardValue(table.playerCard1) + CardValue(table.playerCard2);
            if (playerCards == 3)
            {
                if (!string.IsNullOrEmpty(table.playerCard3) && !string.IsNullOrWhiteSpace(table.playerCard3))
                    this.playerTotal += CardValue(table.playerCard3);
                
            }
        }

        if (bankerCards >= 2)
        {
            this.bankerTotal += CardValue(table.bankerCard1) + CardValue(table.bankerCard2);
            if (bankerCards == 3)
            {
                if (!string.IsNullOrEmpty(table.bankerCard3) && !string.IsNullOrWhiteSpace(table.bankerCard3))
                    this.bankerTotal += CardValue(table.bankerCard3);

            }
        }

        this.playerTotal %= 10;
        this.bankerTotal %= 10;

        SetPointText();
    }

    void SetPointText()
    {
        playerPointTotal.text = playerTotal.ToString();
        bankerPointTotal.text = bankerTotal.ToString();
    }

    private int CardValue(string card)
    {
        Debug.Log("Value : "+ card + card[0]);
        switch (card[0])
        {
            case 'A':
            return 1;
            case 'T':
            case 'K':
            case 'Q':
            case 'J':
            return 10;
            default:
            return int.Parse(card[0].ToString());
        }
    }

    private void SeatPlayerFromState(string sessionId, GoldenFrogPlayer playerState)
    {
        var seatNumber = playerState.seatNumber;

        var avatar = Avatars.FirstOrDefault(a => a.SeatNumber == seatNumber);

        // This user has been merged to a new player session
        if (avatar.SessionId != null && avatar.SessionId != sessionId)
        {
            sessionIdsToAvatars.Remove(avatar.SessionId);

            Debug.Log("Player Merged Successfuly");

            sessionIdsToAvatars.Add(sessionId, avatar);

            avatar.SeatPlayer(sessionId, playerState, mergePlayer: true);
        }
        else
        {
            sessionIdsToAvatars.Add(sessionId, avatar);

            avatar.SeatPlayer(sessionId, playerState);
        }
    }

    //     private void SeatPlayerFromState(string sessionId, GoldenFrogPlayer playerState)
    // {
    //     var seatNumber = playerState.seatNumber;

    //     var avatar = Avatars.FirstOrDefault(a => a.SeatNumber == seatNumber);

    //     // This user has been merged to a new player session
    //     if (avatar != null && avatar.SessionId != sessionId)
    //     {
    //         avatar.SeatPlayer(sessionId, playerState, mergePlayer: true);
    //     }
    //     else
    //     {
    //         sessionIdsToAvatars.Add(sessionId, avatar);

    //         avatar.SeatPlayer(sessionId, playerState);
    //     }
    // }

    private void RemovePlayerFromTable(string sessionId)
    {
        if (sessionIdsToAvatars.ContainsKey(sessionId))
        {
            sessionIdsToAvatars[sessionId].OpenSeat();
            sessionIdsToAvatars.Remove(sessionId);
        }
    }

    private void LoadAvatarWager(string sessionId, GoldenFrogWager wager)
    {
        if (!sessionIdsToAvatars.ContainsKey(sessionId))
            return;

        Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipDown");
        sessionIdsToAvatars[sessionId].UpdateWager(wager);
    }

    private void LoadAvatarsWager(GoldenFrogState state)
    {
        state.players.ForEach((sessionId, playerState) =>
        {
            var avatar = sessionIdsToAvatars[sessionId];
            avatar.UpdateWager(playerState.wager);
        });
    }

    private void LoadAvatarsCredits(GoldenFrogState state)
    {
        state.players.ForEach((sessionId, playerState) =>
        {
            var avatar = sessionIdsToAvatars[sessionId];
            avatar.UpdateCredits(playerState.credits);
        });
    }

    public void LoadRoadmap(GoldenFrogState state)
    {
        var userPlayerSessionId = goldenFrogRoomClient.Room.SessionId;
        var userPlayerState = state.players[userPlayerSessionId];

        var gameResults = RoadmapDataUtils.ServerGameHistoryToGameResults(state.gameHistory.Items.Values.ToList(), userPlayerState.payoutHistory);

        var roadmapData = new RoadmapData();
        roadmapData.LoadGameData(gameResults);

        var roadmapController = roadmap.GetComponentInChildren<RoadmapController>();

        roadmapController.SetData(roadmapData);
    }

    public void ShowRoadmap()
    {
        roadmap.Show();
    }

    public void HideRoadmap()
    {
        roadmap.Hide();
    }

    public void ShowOutcomeAnimation(WagerType wagerType, GoldenFrogEvaluation evaluation)
    {
        OutcomeNotification.Show(wagerType, evaluation);
    }

    public void StartTableAnimation()
    {
        InvokeRepeating("PlayTableFlareAnimations", 1f, animationTimer);
    }

    public void StopTableAnimation()
    {
        CancelInvoke("PlayTableFlareAnimations");
    }

    public void PlayTableFlareAnimations()
    {
            for(int i=0;i<flaresImages.Length;i++)
            {
                TableAnimations.CreateFlareAnimSequence(flaresImages[i]).Play();
            }

            TableAnimations.CreateCircleAnimSequence(playerAnimationImage).Play();
        TableAnimations.CreateCircleAnimSequence(bankerAnimationImage).Play();
    }
}
