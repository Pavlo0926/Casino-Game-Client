using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollBarHelper : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    UIView scrollBar;
    ScrollRect scrollRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollBar.Show();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollBar.Hide();
    }

    void Start()
    {
        scrollRect = this.GetComponent<ScrollRect>();
        scrollBar = scrollRect.verticalScrollbar.gameObject.GetComponent<UIView>();

    }

    



    
}
