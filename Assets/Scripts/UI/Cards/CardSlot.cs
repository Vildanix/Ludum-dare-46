using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public RectTransform dropParent = null;
    public CardHandler slottedCard = null;

    [SerializeField]
    private bool isActivator = false;

    public void OnDrop(PointerEventData eventData)
    {
        // check if something usefull dropped
        if (eventData.pointerDrag == null) return;
        /*
        // check if something usefull is a card
        DragDropCard card = eventData.pointerDrag.GetComponent<DragDropCard>();
        if (card == null) return;

        // we have successfull card drop now. Place it to the slot
        card.SetOriginalParent(dropParent);
        card.GetComponent<RectTransform>().SetParent(dropParent);
        card.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

        // notify slot about new card handler
        CardHandler handler = eventData.pointerDrag.GetComponent<CardHandler>();
        if (handler == null) return;

        PlaceCardInSlot(handler);*/
    }

    public void PlaceCardInSlot(CardHandler card)
    {
        if (isActivator)
        {
            Debug.Log("Slotted card " + card.name);
        }
            
    }
}
