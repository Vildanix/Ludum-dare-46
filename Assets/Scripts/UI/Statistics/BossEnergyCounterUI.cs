using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossEnergyCounterUI : MonoBehaviour
{
    [SerializeField]
    GameObject energyIconPrefab = null;

    [SerializeField]
    GameObject iconContainer = null;
    
    void Awake()
    {
        // register component events
        EventManager.RegisterListener("Restart", Init);
        EventManager.RegisterListener("OnEnergyChange", UpdateEnergyCounter);
        EventManager.RegisterListenerNumber("OnEnergyAdd", AddNewEnergy);
    }

    public void Init()
    {
        EventManager.DispatchEventWithCallback("CheckMaxEnergy", InitializeMaxEnergyCount);
    }

    public void UpdateEnergyCounter()
    {
        EventManager.DispatchEventWithCallback("CheckEnergy", SetEnergy);
    }

    public void SetEnergy(int energyCount)
    {
        for (int i = 0; i < iconContainer.transform.childCount; i++)
        {
            Image energyIcon = iconContainer.transform.GetChild(i).GetComponent<Image>();
            if (!energyIcon) continue;

            if (i < energyCount)
            {
                energyIcon.CrossFadeAlpha(1, 0.3f, false);
            } else
            {
                energyIcon.CrossFadeAlpha(0, 0.3f, false);
            }
        }
    }

    public void InitializeMaxEnergyCount(int energyCount)
    {
        
        DestroyEnergyIcons();
        for(int i = 0; i < energyCount; i++)
        {
            CreateEnergyIcon(i);
        }
    }

    private void DestroyEnergyIcons()
    {
        while (iconContainer.transform.childCount > 0)
        {
            DestroyImmediate(iconContainer.transform.GetChild(0).gameObject);
        }
    }

    private void CreateEnergyIcon(int orderNum)
    {
        var energyIcon = Instantiate(energyIconPrefab, iconContainer.transform);
        energyIcon.name = "Energy " + orderNum;
    }

    public void AddNewEnergy(int energyIndex)
    {
        var energyIcon = Instantiate(energyIconPrefab, iconContainer.transform);
        energyIcon.name = "Energy " + energyIndex;
    }
}
