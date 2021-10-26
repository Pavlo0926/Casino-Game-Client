using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public WagerType wagerType;
    public uint amount;

    //[SerializeField] GameObject chip;
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    Vector2 startPosition;

    bool isUsed = false;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        canvas = GameObject.Find("MasterCanvas").GetComponent<Canvas>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (!this.isUsed)
        {
            GameObject obj = Instantiate(this.gameObject);
            obj.transform.SetParent(transform.parent);
            obj.transform.position = Vector3.zero;
            obj.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
            obj.GetComponent<RectTransform>().localScale = Vector3.one;

            obj.name = transform.name;
            this.isUsed = true;
        }
        transform.SetAsLastSibling();
    }


    public void ResetPosition()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
        "from", rectTransform.anchoredPosition,
        "to", startPosition,
        "speed", 2000f,
        "onupdatetarget", this.gameObject,
        "onupdate", "PlayAnimation"));
    }

    
    public void PlayAnimation(Vector2 position)
    {
        this.rectTransform.anchoredPosition = position;
    }

    public void SetForOtherPlayer(Vector2 pos)
    {
        canvasGroup.blocksRaycasts = true;

        iTween.ValueTo(gameObject, iTween.Hash(
        "from", rectTransform.anchoredPosition,
        "to", pos,
        "speed", 2000f,
        "onupdatetarget", this.gameObject,
        "onupdate", "PlayAnimation"));
    }

    
}
