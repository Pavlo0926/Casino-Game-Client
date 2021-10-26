using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ChipDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public uint amount;

    private GameObject placeHolder = null;
    Canvas canvas;
    CanvasGroup canvasGroup;
    Vector2 startPosition;
    RectTransform rectTransform;


    public void Awake()
    {
        canvas = GameObject.Find("MasterCanvas").GetComponent<Canvas>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;

        this.GetComponent<Button>().onClick.AddListener(OnChipSelect);
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        placeHolder = Instantiate(this.gameObject);
        placeHolder.transform.SetParent(transform.parent);
        placeHolder.transform.position = Vector3.zero;
        placeHolder.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        placeHolder.GetComponent<RectTransform>().localScale = Vector3.one;

        placeHolder.name = transform.name;

        placeHolder.GetComponent<CanvasGroup>().alpha = 0.5f;
        placeHolder.GetComponent<CanvasGroup>().blocksRaycasts = false;
        placeHolder.GetComponent<Button>().onClick.RemoveAllListeners();
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) 
    {
        if (GoldenFrogTableController.Instance.isAcceptingBets)
            placeHolder.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
        else
            ResetPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(placeHolder);
    }

    public void ResetPosition()
    {
        if(placeHolder != null)
        {
        var rectTransform = placeHolder.GetComponent<RectTransform>();
        iTween.ValueTo(placeHolder, iTween.Hash(
        "from", rectTransform.anchoredPosition,
        "to", startPosition,
        "speed", 2000f,
        "onupdatetarget", this.placeHolder,
        "onupdate", "PlayAnimation"));
        }
    }

    public void PlayAnimation(Vector2 position)
    {
        placeHolder.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void PlayAnimationOnChip(Vector2 position)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void OnChipSelect()
    {
        GoldenFrogTableController.Instance.SetSelector(gameObject);
    }

    public Sequence PlaceChipOnBet(ChipStack chipStack, Action onFinishedCallback)
    {
        var targetRect = chipStack.GetComponent<RectTransform>();
        var placeHolder = Instantiate(this.gameObject);
        placeHolder.GetComponent<CanvasGroup>().blocksRaycasts = false;
        placeHolder.transform.SetParent(chipStack.transform, true);

        placeHolder.transform.position = transform.position;
        placeHolder.transform.localRotation = Quaternion.identity;
        placeHolder.transform.localScale = Vector3.one;
        
        placeHolder.name = transform.name + " (DropAnimation)";

        placeHolder.GetComponent<Button>().onClick.RemoveAllListeners();

        var seq =  placeHolder.transform.DOLocalJump(Vector3.zero, 1, 1, 0.33f).SetEase(Ease.OutQuad);
        seq.Insert(0, placeHolder.GetComponent<RectTransform>().DOSizeDelta(targetRect.sizeDelta, 0.33f));

        seq.onComplete += () =>
        {
            Destroy(placeHolder);
            onFinishedCallback?.Invoke();
        };

        return seq;
    }

    public void UnSelectChip()
    {
        GoldenFrogTableController.Instance.selectedChip = null;
    }
}
