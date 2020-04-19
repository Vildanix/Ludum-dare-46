using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardHandler))]
public class DragDropCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Transform dragParent;
    private CardHandler cardHandler;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        cardHandler = GetComponent<CardHandler>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetParent(dragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        /*
        // check if something usefull dropped
        if (eventData.pointerDrag == null) return;

        // check drop on another card, if yes, switch their slots
        CardHandler card = eventData.pointerDrag.GetComponent<CardHandler>();
        if (card != null) {
            card.
        }

        // drop directly onto slot

        // Check if card is dropped directly to card slot
        CardSlot slot = eventData.pointerDrag.GetComponent<CardSlot>();
        if (slot != null)
        {
            // check existing card for replacement in slot
        }

        // check if something usefull is a card
        

        // switch card position
        Transform newOriginalPerent = card.GetOriginalParent();
        
        card.SetOriginalParent(originalParrent);
        card.GetComponent<RectTransform>().SetParent(originalParrent);
        card.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

        rectTransform.SetParent(newOriginalPerent);
        SetOriginalParent(newOriginalPerent);

        // get CardSlot after switch for new slotted card
        //CardSlot slot = newOriginalPerent.GetComponent<CardSlot>();
        if (!slot) return;

        // notify slot about new card handler
        CardHandler handler = GetComponent<CardHandler>();
        if (handler == null) return;

        slot.SlotCard(handler);*/
    }
}
