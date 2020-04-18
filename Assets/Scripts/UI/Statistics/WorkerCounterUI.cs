using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WorkerCounterUI : MonoBehaviour
{
    [SerializeField]
    GameObject workerIconPrefab = null;

    [SerializeField]
    GameObject iconContainer = null;
    
    void Awake()
    {
        // register component events
        EventManager.RegisterListener("Restart", Init);
        EventManager.RegisterListener("OnWorkerCountChange", UpdateWorkerCounter);
    }

    public void Init()
    {
        EventManager.DispatchEventWithCallback("CheckMaxWorkerCount", InitializeMaxWorkerCount);
    }

    public void UpdateWorkerCounter()
    {
        EventManager.DispatchEventWithCallback("CheckWorkerCount", SetWorkerCount);
    }

    public void SetWorkerCount(int workerCount)
    {
        for (int i = 0; i < iconContainer.transform.childCount; i++)
        {
            Image workerIcon = iconContainer.transform.GetChild(i).GetComponent<Image>();
            if (!workerIcon) continue;

            if (i < workerCount)
            {
                workerIcon.CrossFadeAlpha(1, 0.3f, false);
            } else
            {
                workerIcon.CrossFadeAlpha(0, 0.3f, false);
            }
        }
    }

    public void InitializeMaxWorkerCount(int workerCount)
    {
        
        DestroyWorkerIcons();
        for(int i = 0; i < workerCount; i++)
        {
            CreateWorkerIcon(i);
        }
    }

    private void DestroyWorkerIcons()
    {
        while (iconContainer.transform.childCount > 0)
        {
            DestroyImmediate(iconContainer.transform.GetChild(0).gameObject);
        }
    }

    private void CreateWorkerIcon(int orderNum)
    {
        var workerIcon = Instantiate(workerIconPrefab, iconContainer.transform);
        workerIcon.name = "Worker " + orderNum;
    }
}
