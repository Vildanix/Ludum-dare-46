using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayManager : MonoBehaviour
{

    [SerializeField]
    private int totalRequiredTurns = 10;

    [SerializeField]
    private int currentTurn = 1;

    [SerializeField]
    private int maxPlayQuality = 10;

    [SerializeField]
    private int currentPlayQuality = 10;

    [SerializeField]
    private int maxWorkersCount = 8;

    [SerializeField]
    private int availableWorkersCount = 8;

    [SerializeField]
    private int maxBossEnergy = 3;

    [SerializeField]
    private int currentBossEnergy = 3;

    private bool isPlayActive = true;
    private bool isPlayPaused = false;

    private void Awake()
    {
        // register play events
        EventManager.RegisterListener("Restart", PrepareGame);
        EventManager.RegisterListener("EndTurn", EndTurn);
        EventManager.RegisterListener("LowerPlayQuality", LowerPlayQuality);
        EventManager.RegisterListener("ReserveWorker", ReserveWorker);
        EventManager.RegisterListener("FreeWorker", FreeWorker);
        EventManager.RegisterListenerNumber("ReserveEnergy", ReserveEnergy);
        EventManager.RegisterListenerNumber("FreeEnergy", FreeEnergy);
        EventManager.RegisterListener("AddMaxEnergy", AddMaxEnergy);
        EventManager.RegisterListener("WinGame", EndGame);
        EventManager.RegisterListener("LostGame", EndGame);
        EventManager.RegisterListener("PausePlay", PausePlayForOneTurn);
        EventManager.RegisterListenerCallback("CheckWorkerCount", (UnityAction<int> action) => { action.Invoke(GetWorkerCount()); });
        EventManager.RegisterListenerCallback("CheckTurnCount", (UnityAction<int> action) => { action.Invoke(GetTurn()); });
        EventManager.RegisterListenerCallback("CheckPlayQuality", (UnityAction<int> action) => { action.Invoke(GetPlayQuality()); });
        EventManager.RegisterListenerCallback("CheckEnergy", (UnityAction<int> action) => { action.Invoke(GetCurrentEnergy()); });

        EventManager.RegisterListenerCallback("CheckMaxWorkerCount", (UnityAction<int> action) => { action.Invoke(GetMaxWorkerCount()); });
        EventManager.RegisterListenerCallback("CheckMaxTurnCount", (UnityAction<int> action) => { action.Invoke(GetMaxTurnCount()); });
        EventManager.RegisterListenerCallback("CheckMaxPlayQuality", (UnityAction<int> action) => { action.Invoke(GetMaxQuality()); });
        EventManager.RegisterListenerCallback("CheckMaxEnergy", (UnityAction<int> action) => { action.Invoke(GetMaxEnergy()); });
    }

    private void Start()
    {
        EventManager.DispatchEvent("Restart");
    }

    public int GetTurn()
    {
        return currentTurn;
    }

    public int GetPlayQuality()
    {
        return currentPlayQuality;
    }

    public int GetWorkerCount()
    {
        return availableWorkersCount;
    }

    public int GetMaxTurnCount()
    {
        return totalRequiredTurns;
    }

    public int GetMaxQuality()
    {
        return maxPlayQuality;
    }

    public int GetMaxWorkerCount()
    {
        return maxWorkersCount;
    }

    public int GetCurrentEnergy()
    {
        return currentBossEnergy;
    }

    public int GetMaxEnergy()
    {
        return maxBossEnergy;
    }

    /**
     * Set all play progress to default values
     */
    void PrepareGame()
    {
        currentTurn = 1;
        currentPlayQuality = maxPlayQuality;
        availableWorkersCount = maxWorkersCount;
        currentBossEnergy = maxBossEnergy;
        isPlayActive = true;
    }

    /**
     * Process end turn and notify if 
     */
    public void EndTurn()
    {
        EventManager.DispatchEvent("ApplyResources");
        EventManager.DispatchEvent("ResolveEvents");

        if (isPlayActive)
        {
            if (currentTurn > totalRequiredTurns)
            {
                EventManager.DispatchEvent("WinGame");
            } else
            {
                EventManager.DispatchEvent("NewRound"); 
            }
        }

        // restore energy for next turn
        currentBossEnergy = maxBossEnergy;
        EventManager.DispatchEvent("OnEnergyChange");

        if (!isPlayPaused)
        {
            currentTurn++;
            EventManager.DispatchEvent("OnTurnUpdate");
        } else
        {
            isPlayPaused = false;
        }
    }

    public void PausePlayForOneTurn()
    {
        isPlayPaused = true;
    }

    /**
     * Decrease play quality
     */
    public void LowerPlayQuality()
    {
        currentPlayQuality--;

        EventManager.DispatchEvent("OnPlayQualityChange");

        if (currentPlayQuality == 0)
        {
            EventManager.DispatchEvent("LostGame");
        }
    }

    /**
     * Set end game status for current play
     */
    public void EndGame()
    {
        isPlayActive = false;
    }

    /**
     * Reserve one worker for activity
     */
    public void ReserveWorker() {
        if (availableWorkersCount > 0)
        {
            availableWorkersCount--;
        } else
        {
            EventManager.DispatchEventWithText("Error", "There are no free workers left!");
        }

        EventManager.DispatchEvent("OnWorkerCountChange");
    }

    /**
     * Return reserved worker
     */
    public void FreeWorker()
    {
        if (availableWorkersCount < maxWorkersCount)
        {
            availableWorkersCount++;
        } else
        {
            EventManager.DispatchEventWithText("Error", "Teather already have maximum worker count!");
        }

        EventManager.DispatchEvent("OnWorkerCountChange");
    }

    /**
     * Reserve enrgy for action selected this turn
     */
    public void ReserveEnergy(int energyCount)
    {
        if (currentBossEnergy >= energyCount)
        {
            currentBossEnergy -= energyCount;
        }
        else
        {
            EventManager.DispatchEventWithText("Error", "Boss doesn!t have enough energy this turn!");
        }

        EventManager.DispatchEvent("OnEnergyChange");
    }

    /**
     * Return energy for other actions
     */
    public void FreeEnergy(int energyCount)
    {
        currentBossEnergy += energyCount;
        if (currentBossEnergy > maxBossEnergy)
        {
            currentBossEnergy = maxBossEnergy;
        }

        EventManager.DispatchEvent("OnEnergyChange");
    }

    public void AddMaxEnergy()
    {
        maxBossEnergy++;

        EventManager.DispatchEventWithNumber("OnEnergyAdd", maxBossEnergy);
    }
}
