using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;
using System;
using agora_gaming_rtc;
using DG.Tweening;

public class TableAvatar : MonoBehaviour
{
    [SerializeField] Text displayNameText, creditsText, winText;
    [SerializeField] public Image profileImage;
    [SerializeField] Texture2D loadingTexture;
    public TableAvatarTimer Timer;
    public ChipStack BankerChips;
    public ChipStack PlayerChips;
    public ChipStack TieChips;
    public ChipStack JinChan7Chips;
    public ChipStack Koi8Chips;
    public ChipStack NineOverOneChips;
    public ChipStack Natural9Over7Chips;
    public ChipStack Any8Over6Chips;

    public ChipStack PayoutChipStack;

    public GameObject VideoStreamContainer;

    public string SessionId { get; private set; }
    public GoldenFrogPlayer LastPlayerState { get; private set; }

    public uint SeatNumber { get; set; }

    public uint VideoChatUid { get { return SeatNumber + 1; } }

    private CreditMeterController creditMeter;
    private UIView view;
    private uint creditsCount;

    private uint oldCredit =0;
    [SerializeField] Image[] winImage;

    private WagerType wagerType;

    private ChipStack chipToBetPlaceholder;
    
    void Awake()
    {
        Timer = GetComponentInChildren<TableAvatarTimer>();
        view = GetComponent<UIView>();
        creditMeter = GetComponentInChildren<CreditMeterController>();

        PlayerChips.owner = this;
        BankerChips.owner = this;
        TieChips.owner = this;
        JinChan7Chips.owner = this;
        Koi8Chips.owner = this;
        Any8Over6Chips.owner = this;
        NineOverOneChips.owner = this;
        Natural9Over7Chips.owner = this;
        PayoutChipStack.owner = this;
    }

    public void Reset()
    {
        BankerChips.Clear();
        PlayerChips.Clear();
        TieChips.Clear();

        Koi8Chips.Clear();
        JinChan7Chips.Clear();

        Any8Over6Chips.Clear();
        NineOverOneChips.Clear();
        Natural9Over7Chips.Clear();

        PayoutChipStack.Clear();
    }

    public void SeatPlayer(string sessionId, GoldenFrogPlayer player, bool mergePlayer = false)
    {
        SessionId = sessionId;

        string url = "http://graph.facebook.com/" + player.facebookId + "/picture?width=256&height=256";
        Davinci.get().load(url).setLoadingPlaceholder(loadingTexture).into(profileImage).start();

        UpdatePlayerState(player);

        if (!mergePlayer)
            view.Show();
    }

    public void OpenSeat()
    {
        Reset();
        SessionId = null;
        view.Hide();
    }

    public void UpdatePlayerState(GoldenFrogPlayer player)
    {
        LastPlayerState = player;

        displayNameText.text = player.playerName;
        UpdateCredits(player.credits);

        UpdateWager(player.wager);

        //UpdateConnected(player.isConnected);
    }

    public void ShowPlayerProfile()
    {
        if (Player.Instance.UserId == LastPlayerState.playerId)
        {

        }

        Debug.Log("Showing player profile for: " + LastPlayerState.playerId);

        ProfileScreen.ShowPlayerProfile(LastPlayerState.playerId);
    }

    public void UpdateCredits(uint credits)
    {
        // creditsCount = credits;
        // iTween.ValueTo(gameObject, iTween.Hash("from", (int)oldCredit, "to", (int)credits, "time", 1f, "onupdatetarget", this.gameObject, "onupdate", "CurrencyUpdate"));
        // oldCredit = credits;

        creditMeter.ShowTotalCredits(credits);
    }

    public void UpdateCreditDelta(int credits)
    {
        creditMeter.ShowDeltaCreditsView(credits);
    }

    public void CurrencyUpdate(int val)
    {
        creditsText.text = NumberUtils.ToFriendlyQuantityString(val);
    }

    public void UpdateWager(GoldenFrogWager wager)
    {
        BankerChips.SetValue(wager.bankerWager);
        PlayerChips.SetValue(wager.playerWager);
        TieChips.SetValue(wager.tieWager);

        JinChan7Chips.SetValue(wager.jinChan7Wager);
        Koi8Chips.SetValue(wager.koi8Wager);

        Any8Over6Chips.SetValue(wager.any8Over6Wager);
        Natural9Over7Chips.SetValue(wager.natural9Over7Wager);
        NineOverOneChips.SetValue(wager.nineOverOneWager);
    }

    public void UpdateBetAction(WagerType type, int denomination)
    {
        ChipStack targetChipStack = null;

        switch (type)
        {
            case WagerType.Player:
            targetChipStack = PlayerChips;
            break;
            case WagerType.Banker:
            targetChipStack = BankerChips;
            break;
            case WagerType.Tie:
            targetChipStack = TieChips;
            break;
            case WagerType.JinChan7:
            targetChipStack = JinChan7Chips;
            break;
            case WagerType.Koi8:
            targetChipStack = Koi8Chips;
            break;
            case WagerType.Any8Over6:
            targetChipStack = Any8Over6Chips;
            break;
            case WagerType.Natural9Over7:
            targetChipStack = Natural9Over7Chips;
            break;
            case WagerType.NineOverOne:
            targetChipStack = NineOverOneChips;
            break;
        }

        if (chipToBetPlaceholder != null)
        {
            Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipDrop");
            chipToBetPlaceholder.AddValue((uint)denomination);
        }
        else
        {
            PlaceChipOnBet(targetChipStack, denomination, () =>
            {
                targetChipStack.AddValue((uint)chipToBetPlaceholder.value);
                Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipDrop");
                chipToBetPlaceholder = null;
            });
        }
    }


    public Sequence PlaceChipOnBet(ChipStack chipStack, int denomination, Action onFinishedCallback)
    {
        var targetRect = chipStack.GetComponent<RectTransform>();
        var placeHolder = Instantiate(chipStack.gameObject);
        chipToBetPlaceholder = placeHolder.GetComponent<ChipStack>();
        chipToBetPlaceholder.SetValue((uint)denomination);

        placeHolder.GetComponent<CanvasGroup>().blocksRaycasts = false;
        placeHolder.transform.SetParent(chipStack.transform, true);
        placeHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(48, 48);

        placeHolder.transform.position = transform.position;
        placeHolder.transform.localRotation = Quaternion.identity;
        placeHolder.transform.localScale = Vector3.one;
        
        placeHolder.name = chipStack.name + " (DropAnimation)";

        var seq = DOTween.Sequence();
        seq.Append(placeHolder.transform.DOLocalMove(Vector3.zero, 0.33f).SetEase(Ease.OutQuad));
        seq.Insert(0, placeHolder.GetComponent<RectTransform>().DOSizeDelta(targetRect.sizeDelta, 0.33f));

        seq.OnStart(() =>
        {
            //Doozy.Engine.Soundy.SoundyManager.Play("UI", "Slide");
        });

        seq.onComplete += () =>
        {
            Destroy(placeHolder);
            onFinishedCallback?.Invoke();
        };

        return seq;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckWinLoose(GoldenFrogPlayer state, WagerType wager)
    {
        this.wagerType = wager;

        if (state.payout.totalPayout > 0)
        {
            creditsText.gameObject.SetActive(false);
            winText.text = "+" + state.payout.totalPayout;

            //PayoutChipStack.SetValue((uint)state.payout.totalPayout);
            //PayoutChipStack.MoveToPlayer(this.GetComponent<RectTransform>().anchoredPosition);

            ShowWinImage();
        }
    }

    private void ShowWinImage()
    {
        foreach (Image image in winImage)
            image.gameObject.SetActive(true);


        Invoke("HideWinImage", 4f);
    }

    private void HideWinImage()
    {
        foreach (Image image in winImage)
            image.gameObject.SetActive(false);

        winText.text = "";
        creditsText.gameObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", (int)creditsCount -1000 /*LastPlayerState.payout.totalPayout*/, "to", (int)creditsCount, "time", 1f, "onupdatetarget", this.gameObject, "onupdate", "CurrencyUpdate"));
    }

    public Sequence WinningChipsCollectToPlayer(GoldenFrogPayout payout)
    {
        var seq = DOTween.Sequence();

        if (payout.bankerPayout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(BankerChips));
        }
        if (payout.playerPayout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(PlayerChips));
        }
        if (payout.tiePayout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(TieChips));
        }

        if (payout.jinChan7Payout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(JinChan7Chips));
        }
        if (payout.koi8Payout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(Koi8Chips));
        }

        if (payout.any8Over6Payout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(Any8Over6Chips));
        }
        if (payout.natural9Over7Payout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(Natural9Over7Chips));
        }
        if (payout.nineOverOnePayout > 0)
        {
            seq.Insert(0, ChipStackToPlayer(NineOverOneChips));
        }

        return seq;
    }

    private Sequence ChipStackToPlayer(ChipStack stack)
    {
        var seq = DOTween.Sequence();

        seq.Append(stack.transform.DOJump(creditMeter.transform.position, 1, 1, 0.5f));
        seq.onComplete += () =>
        {
            stack.Clear();
        };

        return seq;
    }

    public Sequence DealerPayoutWinningTopSideBets(GoldenFrogEvaluation evaluation, GoldenFrogPayout payout, GoldenFrogWager wager)
    {
        var seq = DOTween.Sequence();

        // Top line side bets are mutually exclusive to each other.  Only 1 can win.
        if (evaluation.isNatural9Over7 && payout.natural9Over7Payout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.natural9Over7Payout - wager.natural9Over7Wager, Natural9Over7Chips));
        }
        else if (evaluation.isAny8Over6 && payout.any8Over6Payout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.any8Over6Payout - wager.any8Over6Wager, Any8Over6Chips));
        }
        else if (evaluation.isNineOverOne && payout.nineOverOnePayout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.nineOverOnePayout - wager.nineOverOneWager, NineOverOneChips));
        }

        return seq;
    }

    public Sequence DealerPayoutWinningMiddleSideBets(GoldenFrogEvaluation evaluation, GoldenFrogPayout payout, GoldenFrogWager wager)
    {
        var seq = DOTween.Sequence();

        // Middle line side bets are mutually exclusive to each other.  Only 1 can win.
        if (evaluation.isKoi8 && payout.koi8Payout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.koi8Payout - wager.koi8Wager, Koi8Chips));
        }
        else if (evaluation.isJinChan7 && payout.jinChan7Payout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.jinChan7Payout - wager.jinChan7Wager, JinChan7Chips));
        }
        else if (evaluation.outcome == "tie" && payout.tiePayout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.tiePayout - wager.tieWager, TieChips));
        }

        return seq;
    }

    public Sequence DealerPayoutWinningOrPushedOutcomeBets(GoldenFrogEvaluation evaluation, GoldenFrogPayout payout, GoldenFrogWager wager)
    {
        var seq = DOTween.Sequence();

        if (evaluation.outcome == "banker" && payout.bankerPayout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.bankerPayout - wager.bankerWager, BankerChips));
        }
        else if (evaluation.outcome == "player" && payout.playerPayout > 0)
        {
            seq.Append(PayoutChipStack.MoveToOtherChipStack(payout.playerPayout - wager.playerWager, PlayerChips));
        }

        return seq;
    }

    public Sequence DealerCollectLosingTopSideBets(GoldenFrogEvaluation evaluation)
    {
        var seq = DOTween.Sequence();

        // Natural 9/7 Losing Bet Collect
        if (Natural9Over7Chips.value > 0 && !evaluation.isNatural9Over7)
        {
            seq.Insert(0, Natural9Over7Chips.MoveToDealer());
        }
        // Any 8/6 Losing Bet Collect
        if (Any8Over6Chips.value > 0 && !evaluation.isAny8Over6)
        {
            seq.Insert(0, Any8Over6Chips.MoveToDealer());
        }
        // 9/1 Losing BetCollect
        if (NineOverOneChips.value > 0 && !evaluation.isNineOverOne)
        {
            seq.Insert(0, NineOverOneChips.MoveToDealer());
        }

        return seq;
    }

    public Sequence DealerCollectLosingMiddleBets(GoldenFrogEvaluation evaluation)
    {
        var seq = DOTween.Sequence();

        // Koi 8 Losing Bet Collect
        if (Koi8Chips.value > 0 && !evaluation.isKoi8)
        {
            seq.Insert(0, Koi8Chips.MoveToDealer());
        }
        // JinChan 7
        if (JinChan7Chips.value > 0 && !evaluation.isJinChan7)
        {
            seq.Insert(0, JinChan7Chips.MoveToDealer());
        }
        // Tie
        if (TieChips.value > 0 && evaluation.outcome != "tie")
        {
            seq.Insert(0, TieChips.MoveToDealer());
        }

        return seq;
    }

    public Sequence DealerCollectLosingOutcomeBets(GoldenFrogEvaluation evaluation)
    {
        var seq = DOTween.Sequence();

        if (evaluation.outcome == "tie")
        {
            return seq;
        }
        else if (BankerChips.value > 0 && evaluation.outcome != "banker")
        {
            seq.Insert(0, BankerChips.MoveToDealer());
        }
        else if (PlayerChips.value > 0 && evaluation.outcome != "player")
        {
            seq.Insert(0, PlayerChips.MoveToDealer());
        }

        return seq;
    }

    public void AttachVideoStream()
    {
        var surface = VideoStreamContainer.GetComponent<VideoSurface>();
        if (surface == null)
            surface = VideoStreamContainer.AddComponent<VideoSurface>();

        surface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);

        surface.SetForUser(VideoChatUid);
        //o.mAdjustTransfrom += onTransformDelegate;
        //o.transform.Rotate(-90.0f, 0.0f, 0.0f);
        //float r = Random.Range(-5.0f, 5.0f);
        //o.transform.position = new Vector3(0f, r, 0f);
        //o.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
    }

    public void ShowVideoStream()
    {
        Debug.Log("SHOWING VIDEO STREAM FOR USER: " + VideoChatUid);
        VideoStreamContainer.SetActive(true);
        VideoSurface o = VideoStreamContainer.GetComponent<VideoSurface>();

        o.SetEnable(true);
    }

    public void HideVideoStream()
    {
        VideoStreamContainer.SetActive(false);
        VideoSurface o = VideoStreamContainer.GetComponent<VideoSurface>();
        o.SetEnable(false);
    }
}
