using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCreator : MonoBehaviour
{
    public Obstacle[] allAvailableObstacles;

    public List<Obstacle> obstaclePool;

    public List<Obstacle> solvedObstacles;

    [SerializeField]
    private int reservedWorkPlay = 0;

    private void Awake()
    {
        EventManager.RegisterListener("NewRound", CreateTurnSet);
        EventManager.RegisterListener("Restart", PreparePlay);
        EventManager.RegisterListenerText("SolveObstacle", FixObstacle);
        obstaclePool = new List<Obstacle>();
        solvedObstacles = new List<Obstacle>();
    }

    public void PreparePlay() {
        obstaclePool.Clear();

        foreach (Obstacle obstacle in allAvailableObstacles)
        {
            obstaclePool.Add(obstacle);
        }
        reservedWorkPlay = 0;
    }

    public void FixObstacle(string obstacleName)
    {
        foreach (Obstacle obstacle in allAvailableObstacles)
        {
            if (obstacle.GetObstacleEventName().Equals(obstacleName))
            {
                solvedObstacles.Add(obstacle);
                break;
            }
        }
    }

    public void CreateTurnSet()
    {
        // free workplay from previous turn
        for (int i = 0; i < reservedWorkPlay; i++)
        {
            EventManager.DispatchEvent("FreeWorker");
        }
        
        // chose how much work will this turn require
        int requiredEnergy = Random.Range(1, 4);
        int reducedWorkplay = Random.Range(0, Mathf.Min(requiredEnergy, 3)); // reserve play energy

        for (int i = 0; i < reducedWorkplay; i++)
        {
            EventManager.DispatchEvent("ReserveWorker");
        }
        
        reservedWorkPlay = reducedWorkplay;

        // activate inactive events 
        for (int i = 0; i < requiredEnergy; i++)
        {
            ActivateRandomObstacleFromPool();
        }

        // copy solved obstacles to pool for next turn
        foreach(Obstacle obstacle in solvedObstacles)
        {
            obstaclePool.Add(obstacle);
        }
        solvedObstacles.Clear();
    }

    public void ActivateRandomObstacleFromPool()
    {
        if (obstaclePool.Count == 0) return;

        int obstacleIndex = Random.Range(0, obstaclePool.Count - 1);
        obstaclePool[obstacleIndex].ActivateObstacle();
        obstaclePool.RemoveAt(obstacleIndex);
    }
}
