﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSetter : MonoBehaviour
{
    [SerializeField]
    GameObject actionPanel = null;
    [SerializeField]
    GameObject unitButton = null;
    [SerializeField]
    UnitAction unitActions = null;

    private List<string> activeObstacles;

    [SerializeField]
    private string activeAction = "";

    [SerializeField]
    private Sprite defaultIcon;

    [SerializeField]
    private Color defaultColor;

    [SerializeField]
    private Color fixColor;

    private bool isEnergyTaken = false;
    private bool isPlayActive = false;
    private bool borrowedWorkPlayForGhost = false;
    private int availableEnergy = 0;

    private void Awake()
    {
        activeObstacles = new List<string>();
        EventManager.RegisterListenerText("ActivatedObstacle", RegisterActiveObstacle);
        EventManager.RegisterListenerText("SolvedObstacle", UnregisterActiveObstacle);

        EventManager.RegisterListener("ApplyActions", ApplyAction);
        EventManager.RegisterListener("NewRound", ReleaseBorrowedResources);
        EventManager.RegisterListener("OnEnergyChange", ReevaluateActionIcons);
    }

    public void TogglePanel()
    {
        if (actionPanel.activeSelf)
        {
            actionPanel.SetActive(false);
        } else {
            actionPanel.SetActive(true);
            ReevaluateActionIcons();
        }
        
    }

    private void ReevaluateActionIcons()
    {
        EventManager.DispatchEventWithCallback("CheckEnergy", (int energy) => { availableEnergy = energy; });

        if (availableEnergy < 1)
        {
            // disable all energy actions
            for (int i = 0; i < actionPanel.transform.childCount - 1; i++)
            {
                Button actionButton = actionPanel.transform.GetChild(i).GetComponent<Button>();
                actionButton.interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < actionPanel.transform.childCount - 1; i++)
            {
                Button actionButton = actionPanel.transform.GetChild(i).GetComponent<Button>();
                actionButton.interactable = true;
            }
        }
    }

    public void ReleaseBorrowedResources()
    {
        FreeEnergy();
        FreePlay();
    }

    public void RegisterActiveObstacle(string obstacleName)
    {
        if (activeObstacles.Contains(obstacleName)) return;
        activeObstacles.Add(obstacleName);
        ReevaluateActionIcons();
    }

    public void UnregisterActiveObstacle(string obstacleName)
    {
        if (activeObstacles.Contains(obstacleName))
        {
            activeObstacles.Remove(obstacleName);
        }

        ReevaluateActionIcons();
    }

    public void SetActiveAction(string actionName)
    {
        if (activeAction.Equals(actionName)) return;

        activeAction = actionName;

        if (activeAction.Length == 0)
        {
            ResetActionIcon();
            FreePlay();
            // free taken enrgy
            if (isEnergyTaken)
            {
                EventManager.DispatchEventWithNumber("FreeEnergy", 1);
                isEnergyTaken = false;
            }
            
        } else if (actionName.Equals("Play"))
        {
            ReservePlay();
            SetActionColor(defaultColor);
            ReserveEnergy();
            
        } else 
        {
            // reserve enrgy for action
            ReserveEnergy();
            FreePlay();
            SetActionColor(fixColor);
        }

        HideActionPanel();
    }

    private void ReserveEnergy()
    {
        if (!isEnergyTaken)
        {
            EventManager.DispatchEventWithNumber("ReserveEnergy", 1);
            isEnergyTaken = true;
        }
    }

    private void FreeEnergy()
    {
        if (isEnergyTaken)
        {
            EventManager.DispatchEventWithNumber("FreeEnergy", 1);
            isEnergyTaken = false;
        }
    }

    private void ReservePlay()
    {
        if (!isPlayActive)
        {
            EventManager.DispatchEvent("ReserveWorker");
            isPlayActive = true;
        }
    }

    private void FreePlay()
    {
        if (isPlayActive)
        {
            EventManager.DispatchEvent("FreeWorker");
            isPlayActive = false;
        }
    }

    public void ApplyAction()
    {
        switch (activeAction)
        {
            case "Play":
                // play already reserved
                SetActionColor(defaultColor);
                break;
            case "FixObject":
                EventManager.DispatchEventWithText("SolveObstacle", "BrokenObject");
                if (unitActions.IsBadActor())
                    EventManager.DispatchEvent("LowerPlayQuality");
                break;
            case "FixCurtain":
                EventManager.DispatchEventWithText("SolveObstacle", "BrokenCurtain");
                if (unitActions.IsBadActor())
                    EventManager.DispatchEvent("LowerPlayQuality");
                break;
            case "FixHelper":
                EventManager.DispatchEventWithText("SolveObstacle", "BrokenHelper");
                if (unitActions.IsBadActor())
                    EventManager.DispatchEvent("LowerPlayQuality");
                break;
            case "FixGhost":
                EventManager.DispatchEventWithText("SolveObstacle", "Ghost");
                if (unitActions.IsBadActor())
                    EventManager.DispatchEvent("LowerPlayQuality");
                // borrow playwork for missing ghost
                ReservePlay();
                break;
            case "FixLight":
                EventManager.DispatchEventWithText("SolveObstacle", "LightDown");
                break;
        }

        activeAction = "";

        HideActionPanel();
        ResetActionIcon();
    }

    public void SetActionIcon(Sprite icon)
    {
        unitButton.GetComponent<Image>().sprite = icon;
    }

    public void SetActionColor(Color iconColor)
    {
        GetComponent<Image>().color = iconColor;
    }

    private void ResetActionIcon()
    {
        unitButton.GetComponent<Image>().sprite = defaultIcon;
        GetComponent<Image>().color = defaultColor;
    }

    private void HideActionPanel()
    {
        actionPanel.SetActive(false);
    }
}
