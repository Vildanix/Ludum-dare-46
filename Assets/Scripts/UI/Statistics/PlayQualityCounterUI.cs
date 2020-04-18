using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayQualityCounterUI : MonoBehaviour
{
    [SerializeField]
    GameObject qualityIconPrefab = null;

    [SerializeField]
    GameObject iconContainer = null;
    
    void Awake()
    {
        // register component events
        EventManager.RegisterListener("Restart", Init);
        EventManager.RegisterListener("OnPlayQualityChange", UpdateWorkerCounter);
    }

    public void Init()
    {
        EventManager.DispatchEventWithCallback("CheckMaxPlayQuality", InitializeMaxPlayQualityCount);
    }

    public void UpdateWorkerCounter()
    {
        EventManager.DispatchEventWithCallback("CheckPlayQuality", SetQualityCount);
    }

    public void SetQualityCount(int qualityCount)
    {
        for (int i = 0; i < iconContainer.transform.childCount; i++)
        {
            Image qualityIcon = iconContainer.transform.GetChild(i).GetComponent<Image>();
            if (!qualityIcon) continue;

            if (i < qualityCount)
            {
                qualityIcon.CrossFadeAlpha(1, 0.3f, false);
            } else
            {
                qualityIcon.CrossFadeAlpha(0, 0.3f, false);
            }
        }
    }

    public void InitializeMaxPlayQualityCount(int qualityCount)
    {
        
        DestroyQualityIcons();
        for(int i = 0; i < qualityCount; i++)
        {
            CreateQualityIcon(i);
        }
    }

    private void DestroyQualityIcons()
    {
        while (iconContainer.transform.childCount > 0)
        {
            DestroyImmediate(iconContainer.transform.GetChild(0).gameObject);
        }
    }

    private void CreateQualityIcon(int orderNum)
    {
        var workerIcon = Instantiate(qualityIconPrefab, iconContainer.transform);
        workerIcon.name = "Quality " + orderNum;
    }
}
