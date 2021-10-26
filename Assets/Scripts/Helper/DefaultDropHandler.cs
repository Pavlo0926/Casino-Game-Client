using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var chipDraggable = eventData.pointerDrag.GetComponent<ChipDraggable>();
            if (chipDraggable == false)
                return;

            Debug.Log("RESET");
            chipDraggable.ResetPosition();
        }
    }
}
