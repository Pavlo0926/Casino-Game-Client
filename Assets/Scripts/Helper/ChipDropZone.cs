using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChipDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] WagerType WagerType;
    private ChipStack chipStack;

    public void Awake()
    {
        chipStack = GetComponentInChildren<ChipStack>();
        if(!(WagerType == WagerType.Player || WagerType == WagerType.Banker))
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var chipDraggable = eventData.pointerDrag.GetComponent<ChipDraggable>();
            if (chipDraggable == false)
                return;

            GoldenFrogTableController.Instance.AddPlayerWagerFromDraggable(WagerType, chipDraggable);
        }
    }

    public void OnClick()
    {
        if(GoldenFrogTableController.Instance.selectedChip != null && GoldenFrogTableController.Instance.isAcceptingBets)
        {
            var draggable = GoldenFrogTableController.Instance.selectedChip.GetComponent<ChipDraggable>();
            draggable.PlaceChipOnBet(chipStack, () =>
            {
                Doozy.Engine.Soundy.SoundyManager.Play("Chips", "chipDrop");
                chipStack.AddValue(draggable.amount);
                GoldenFrogTableController.Instance.AddPlayerWagerFromDraggable(WagerType, draggable);
            });
        }
    }
}
