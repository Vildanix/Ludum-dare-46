using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    [SerializeField]
    private bool canPlay = false;

    [SerializeField]
    private bool canFixStage = false;

    [SerializeField]
    private bool canRepairEnergy = false;

    [SerializeField]
    private bool isBadActor = false;

    public bool CanPlay()
    {
        return canPlay;
    }

    public bool CanFixStage()
    {
        return canFixStage;
    }

    public bool CanRepairEnergy()
    {
        return canRepairEnergy;
    }

    public bool IsBadActor()
    {
        return isBadActor;
    }
}
