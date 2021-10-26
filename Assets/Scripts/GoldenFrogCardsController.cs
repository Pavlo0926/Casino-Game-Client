using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldenFrogCardsController : MonoBehaviour
{
    public GameObject PlayerCard1;
    public GameObject PlayerCard2;
    public GameObject PlayerCard3;

    public GameObject BankerCard1;
    public GameObject BankerCard2;
    public GameObject BankerCard3;

    public Transform ShoeLocation;

    private float startingY;

    // Start is called before the first frame update
    void Start()
    {
        startingY = PlayerCard1.GetComponent<RectTransform>().anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        DestroyChildren(PlayerCard1);
        DestroyChildren(PlayerCard2);
        DestroyChildren(PlayerCard3);

        DestroyChildren(BankerCard1);
        DestroyChildren(BankerCard2);
        DestroyChildren(BankerCard3);
    }

    private void DestroyChildren(GameObject obj)
    {
        foreach (Transform transform in obj.transform)
        {
            DestroyImmediate(transform.gameObject);
        }

        var vec = obj.GetComponent<RectTransform>().anchoredPosition;
        vec.y = startingY;
        obj.GetComponent<RectTransform>().anchoredPosition = vec;
    }

    public Sequence NudgePlayerHand()
    {
        return TableAnimations.NudgeHand(new[] { PlayerCard1, PlayerCard2, PlayerCard3 });
    }

    public Sequence NudgeBankerHand()
    {
        return TableAnimations.NudgeHand(new[] { BankerCard1, BankerCard2, BankerCard3 });
    }

    public Sequence DealInitialHand(GoldenFrogTable table)
    {
        return TableAnimations.DealInitialHand(table, this, ShoeLocation);
    }

    public Sequence Deal3rdBankerCard(GoldenFrogTable table)
    {
        return TableAnimations.Deal3rdBankerCard(table.bankerCard3, this, ShoeLocation);
    }

    public Sequence Deal3rdPlayerCard(GoldenFrogTable table)
    {
        return TableAnimations.Deal3rdPlayerCard(table.playerCard3, this, ShoeLocation);
    }

    public void LoadFromTableState(GoldenFrogState state, GameState gameState)
    {
        var table = state.table;

        // Must make sure the appropriate game state has passed to show the card
        // Also must make sure the server actually sent 3rd card.  IE: reached finished state
        // all cards should be shown that were dealt.

        if (gameState >= GameState.InitialHandDealt)
        {
            CardFactory.CreateCard(table.playerCard1, true, PlayerCard1.transform);
            CardFactory.CreateCard(table.playerCard2, true, PlayerCard2.transform);
            CardFactory.CreateCard(table.bankerCard1, true, BankerCard1.transform);
            CardFactory.CreateCard(table.bankerCard2, true, BankerCard2.transform);

            if (gameState >= GameState.Player3rdDealt && !string.IsNullOrEmpty(table.playerCard3))
            {
                var card = CardFactory.CreateCard(table.playerCard3, true, PlayerCard3.transform);
                card.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            if (gameState >= GameState.Dealer3rdDealt && !string.IsNullOrEmpty(table.bankerCard3))
            {
                var card = CardFactory.CreateCard(table.bankerCard3, true, BankerCard3.transform);
                card.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
        }
    }
}
