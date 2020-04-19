using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private string obstacleEventName = "";

    [SerializeField]
    private string obstacleAction = "";

    [SerializeField]
    private GameObject objectToEnable = null;

    private void Awake()
    {
        EventManager.RegisterListener("Restart", PrepareObstacle);
    }

    public void PrepareObstacle()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }

        Disable();
    }

    private void OnEnable()
    {
        EventManager.RegisterListener("ResolveEvents", ResolveObstacle);
        EventManager.RegisterListenerText("SolveObstacle", SolveObstacle);

        EventManager.DispatchEventWithText("ActivatedObstacle", obstacleEventName);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("ResolveEvents", ResolveObstacle);
        EventManager.RemoveListenerText("SolveObstacle", SolveObstacle);

        EventManager.DispatchEventWithText("SolvedObstacle", obstacleEventName);
    }

    public void ActivateObstacle()
    {
        gameObject.SetActive(true);

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }

    public string GetObstacleEventName()
    {
        return obstacleEventName;
    }

    public void SolveObstacle(string obstacleName)
    {
        if (obstacleName != obstacleEventName) return;

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }

        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void ResolveObstacle()
    {
        EventManager.DispatchEvent(obstacleAction);
    }
}
