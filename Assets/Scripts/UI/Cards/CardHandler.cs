using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHandler : MonoBehaviour
{
    private bool isActivated = false;
    private bool hasEnougWorkers = true;
    private bool hasEnoughEnergy = true;

    [SerializeField]
    private CardSlot parentSlot;

    [SerializeField]
    private int energyCost = 1;

    [SerializeField]
    private int workerCost = 0;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        EventManager.RegisterListener("OnWorkerCountChange", ValidateWorkerUse);
        EventManager.RegisterListener("OnEnergyChange", ValidateEnergyUse);

        canvasGroup = GetComponent<CanvasGroup>();


    }

    public void ValidateWorkerUse()
    {
        if (isActivated) return;

        EventManager.DispatchEventWithCallback("CheckWorkerCount", CheckAvailableWorkerCount);
    }

    public void CheckAvailableWorkerCount(int workerCount)
    {
        if (workerCount < 1)
        {
            hasEnougWorkers = false;
        } else
        {
            hasEnougWorkers = true;
        }

        ValidateCardPlay();
    }

    public void ValidateEnergyUse()
    {
        if (isActivated) return;

        EventManager.DispatchEventWithCallback("CheckEnergy", CheckAvailableEnergy);
    }

    public void CheckAvailableEnergy(int energyCount)
    {
        if (energyCount < 1)
        {
            hasEnoughEnergy = false;
        }
        else
        {
            hasEnoughEnergy = true;
        }

        ValidateCardPlay();
    }

    private void ValidateCardPlay()
    {
        if (hasEnoughEnergy && hasEnougWorkers)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        } else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnSlot()
    {
        EventManager.RegisterListener("ApplyResources", ApplyResources);
        EventManager.RegisterListener("ResolveEvents", ResolveEvents);
        isActivated = true;

        ReserveResources();
    }

    public void OnUnslot()
    {
        EventManager.RegisterListener("ApplyResources", ApplyResources);
        EventManager.RegisterListener("ResolveEvents", ResolveEvents);
        isActivated = false;

        FreeResources();
    }

    public void ReserveResources()
    {
        EventManager.DispatchEvent("ReserveWorker");
        EventManager.DispatchEventWithNumber("ReserveEnergy", 1);
    }

    public void FreeResources()
    {
        EventManager.DispatchEvent("FreeWorker");
        EventManager.DispatchEventWithNumber("FreeEnergy", 1);
    }

    public void ApplyResources()
    {
        //EventManager.DispatchEvent("AddMaxEnergy");
    }

    public void ResolveEvents()
    {
        //EventManager.DispatchEvent("LowerPlayQuality");
    }
}
