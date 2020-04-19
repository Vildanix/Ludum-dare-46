using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spectator : MonoBehaviour
{
    [SerializeField]
    private BoxCollider wanderArea;

    [SerializeField]
    private NavMeshAgent agent;

    private float actingDistance = 0.5f;

    [SerializeField]
    private int expectedPlayQuality = 1;

    private Vector3 startingPosition = Vector3.zero;
    private Quaternion startingRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Awake()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        startingPosition = transform.position;
        startingRotation = transform.rotation;

        EventManager.RegisterListener("OnPlayQualityChange", OnPlayQualityChange);
        EventManager.RegisterListener("Restart", ReturnToSeat);
    }

    void Update()
    {
        if (agent.remainingDistance < actingDistance)
        {
            Disapear();
        }
    }

    public void OnPlayQualityChange()
    {
        EventManager.DispatchEventWithCallback("CheckPlayQuality", CheckMyExpectations);
    }

    public void CheckMyExpectations(int playQuality)
    {
        if (playQuality < expectedPlayQuality)
        {
            StepOut();
        }
    }

    public void StepOut()
    {
        if (!wanderArea) return;

        Vector3 wanderPoint = new Vector3(
            Random.Range(wanderArea.bounds.min.x, wanderArea.bounds.max.x),
            Random.Range(wanderArea.bounds.min.y, wanderArea.bounds.max.y),
            Random.Range(wanderArea.bounds.min.z, wanderArea.bounds.max.z)
        );

        agent.SetDestination(wanderPoint);
    }

    private void Disapear()
    {
        // set invisible, disable agent collision
    }

    private void ReturnToSeat()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        // set visible, enable agent collision
    }
}
