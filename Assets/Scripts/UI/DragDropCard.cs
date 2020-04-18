using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Transform dragParent;
    private Transform originalParrent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParrent = rectTransform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetParent(dragParent);
    }

    public Transform GetOriginalParent()
    {
        return originalParrent;
    }

    public void SetOriginalParent(Transform newParent)
    {
        originalParrent = newParent;
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
        // check if something usefull dropped
        if (eventData.pointerDrag == null) return;

        // check if something usefull is a card
        DragDropCard card = eventData.pointerDrag.GetComponent<DragDropCard>();
        if (card == null) return;

        // switch card position
        Transform newOriginalPerent = card.GetOriginalParent();
        
        card.SetOriginalParent(originalParrent);
        card.GetComponent<RectTransform>().SetParent(originalParrent);
        card.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

        rectTransform.SetParent(newOriginalPerent);
        SetOriginalParent(newOriginalPerent);

        // get CardSlot after switch for new slotted card
        CardSlot slot = newOriginalPerent.GetComponent<CardSlot>();
        if (!slot) return;

        // notify slot about new card handler
        CardHandler handler = GetComponent<CardHandler>();
        if (handler == null) return;

        slot.SlotCard(handler);
    }
}
