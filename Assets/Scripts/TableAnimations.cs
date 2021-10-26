using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class TableAnimations
{
    public static Sequence DealInitialHand(GoldenFrogTable table, GoldenFrogCardsController cardsController, Transform shoeLocation)
    {
        var seq = DOTween.Sequence();


        var card1 = CardFactory.CreateCard(table.playerCard1
            , false, shoeLocation.position, cardsController.PlayerCard1.transform);
        var card2 = CardFactory.CreateCard(table.bankerCard1
            , false, shoeLocation.position, cardsController.BankerCard1.transform);
        var card3 = CardFactory.CreateCard(table.playerCard2
            , false, shoeLocation.position, cardsController.PlayerCard2.transform);
        var card4 = CardFactory.CreateCard(table.bankerCard2
            , false, shoeLocation.position, cardsController.BankerCard2.transform);

        seq.Insert(0.0f, CreateShoeToTableSequence(card1));
        seq.Insert(0.2f, CreateShoeToTableSequence(card2));
        seq.Insert(0.4f, CreateShoeToTableSequence(card3));
        seq.Insert(0.6f, CreateShoeToTableSequence(card4));

        var flips = DOTween.Sequence()
            .Insert(0, card1.GetComponent<Card>().FlipFront())
            .Insert(0, card2.GetComponent<Card>().FlipFront())
            .Insert(0, card3.GetComponent<Card>().FlipFront())
            .Insert(0, card4.GetComponent<Card>().FlipFront());
        seq.Append(flips.SetDelay(.5f));

        return seq;
    }

    public static Sequence Deal3rdBankerCard(string card, GoldenFrogCardsController cardsController, Transform shoeLocation)
    {
        var newCard = CardFactory.CreateCard(card
            , false, shoeLocation.position, cardsController.BankerCard3.transform);

        var seq = DOTween.Sequence();

        seq.Insert(0.0f, Create3rdCardShoeToTableSequence(newCard));
        seq.Append(newCard.GetComponent<Card>().FlipFrontHorizontal().SetDelay(0.5f));

        return seq;
    }

    public static Sequence Deal3rdPlayerCard(string card, GoldenFrogCardsController cardsController, Transform shoeLocation)
    {
        var newCard = CardFactory.CreateCard(card
            , false, shoeLocation.position, cardsController.PlayerCard3.transform);

        var seq = DOTween.Sequence();

        seq.Insert(0.0f, Create3rdCardShoeToTableSequence(newCard));
        seq.Append(newCard.GetComponent<Card>().FlipFrontHorizontal().SetDelay(0.5f));

        return seq;
    }

    public static Sequence NudgeHand(IList<GameObject> cards)
    {
        var seq = DOTween.Sequence();
        var shines = DOTween.Sequence();

        foreach (var card in cards)
        {
            seq.Insert(0, card.transform.DOLocalMoveY(-30, 0.5f).SetEase(Ease.OutQuad));

            // Add shine if there is a card in the container.
            var cardScript = card.GetComponentInChildren<Card>();
            if (cardScript != null)
                shines.Insert(0, cardScript.ShowShineAnimation());
        }

        seq.Append(shines);

        return seq;
    }

    private static Sequence CreateShoeToTableSequence(GameObject card)
    {
        return DOTween.Sequence()
            .Append(card.GetComponent<CanvasGroup>().DOFade(1, .33f).From(0, true))
            .Insert(0, card.transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(Ease.OutQuad))
            .Insert(0, card.transform.DOLocalRotate(Vector3.zero, 0.5f, RotateMode.FastBeyond360).From(new Vector3(0, 0, 270), true).SetEase(Ease.OutQuad))
            .OnStart(() =>
            {
                Doozy.Engine.Soundy.SoundyManager.Play("Cards", "cardDeal");
            });
    }

    private static Sequence Create3rdCardShoeToTableSequence(GameObject card)
    {
        return DOTween.Sequence()
            .Append(card.GetComponent<CanvasGroup>().DOFade(1, .33f).From(0, true))
            .Insert(0, card.transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(Ease.OutQuad))
            .Insert(0, card.transform.DOLocalRotate(new Vector3(0, 0, 90), 0.5f, RotateMode.FastBeyond360).From(new Vector3(0, 0, 270), true).SetEase(Ease.OutQuad))
            .OnStart(() =>
            {
                Doozy.Engine.Soundy.SoundyManager.Play("Cards", "cardDeal");
            });
    }

    public static Sequence CreateFlareAnimSequence(GameObject flare)
    {
        return DOTween.Sequence()
            .Append(flare.GetComponent<Image>().DOFade(1f, 0.5f).From(0, true)).SetLoops(2)
            .Insert(0, flare.transform.DOLocalRotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).From(new Vector3(0, 0, 0), true).SetEase(Ease.OutQuad)).SetLoops(2)
            .OnStart(() =>
            {
                //Doozy.Engine.Soundy.SoundyManager.Play("Cards", "cardDeal");
            })
            .OnComplete(()=>{
                flare.GetComponent<Image>().DOFade(0, 0).From(0, true);
                Debug.Log("OnComoelte");
            });
    }

    public static Sequence CreateCircleAnimSequence(GameObject circle)
    {
        return DOTween.Sequence()
            .Append(circle.GetComponent<Image>().DOFade(1f, .33f).From(0, true)).SetLoops(2)
            .Insert(0, circle.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).From(new Vector3(1, 1, 1), true).SetEase(Ease.OutQuad)).SetLoops(2)
            .OnStart(() =>
            {
                //Doozy.Engine.Soundy.SoundyManager.Play("Cards", "cardDeal");
            })
            .OnComplete(() => {
                circle.GetComponent<Image>().DOFade(0, 0).From(0, true);
                Debug.Log("OnComoelte");
            });
    }
}