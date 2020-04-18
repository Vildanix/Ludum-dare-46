using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayProgressUI : MonoBehaviour
{
    [SerializeField]
    GameObject progressIconPrefab = null;

    [SerializeField]
    GameObject iconContainer = null;
    
    void Awake()
    {
        // register component events
        EventManager.RegisterListener("Restart", Init);
        EventManager.RegisterListener("OnTurnUpdate", UpdateTurnCounter);
    }

    public void Init()
    {
        EventManager.DispatchEventWithCallback("CheckMaxTurnCount", InitializeMaxTurnCount);
        EventManager.DispatchEventWithCallback("CheckTurnCount", SetTurnCount);
    }

    public void UpdateTurnCounter()
    {
        EventManager.DispatchEventWithCallback("CheckTurnCount", SetTurnCount);
    }

    public void SetTurnCount(int turnCount)
    {
        for (int i = 0; i < iconContainer.transform.childCount; i++)
        {
            Image turnIcon = iconContainer.transform.GetChild(i).GetComponent<Image>();
            if (!turnIcon) continue;

            if (i < turnCount)
            {
                turnIcon.CrossFadeAlpha(1, 0.3f, false);
            } else
            {
                turnIcon.CrossFadeAlpha(0, 0.3f, false);
            }
        }
    }

    public void InitializeMaxTurnCount(int turnCount)
    {
        
        DestroyTurnIcons();
        for(int i = 0; i < turnCount; i++)
        {
            CreateTurnIcon(i);
        }
    }

    private void DestroyTurnIcons()
    {
        while (iconContainer.transform.childCount > 0)
        {
            DestroyImmediate(iconContainer.transform.GetChild(0).gameObject);
        }
    }

    private void CreateTurnIcon(int orderNum)
    {
        var turnIcon = Instantiate(progressIconPrefab, iconContainer.transform);
        turnIcon.name = "Turn " + orderNum;
    }
}
