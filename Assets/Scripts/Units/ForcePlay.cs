using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePlay : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.RegisterListener("FreeResources", ReturnPlayWork);
        EventManager.RegisterListener("NewTurn", ForcePlayWork);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("FreeResources", ReturnPlayWork);
        EventManager.RemoveListener("NewTurn", ForcePlayWork);
    }
    public void ReturnPlayWork()
    {
        EventManager.DispatchEvent("FreeWorker");
    }

    public void ForcePlayWork()
    {
        EventManager.DispatchEvent("ReserveWorker");
    }
}
