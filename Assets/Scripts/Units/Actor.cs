using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Actor : MonoBehaviour
{
    [SerializeField]
    private BoxCollider wanderArea;

    [SerializeField]
    private NavMeshAgent agent;

    private float actingDistance = 0.5f;

    private bool isActing = false;
    private bool isFixing = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        EventManager.RegisterListener("OnTurnUpdate", WanderOnStage);
    }

    void Update()
    {
        if (agent.remainingDistance < actingDistance)
        {
            Act();
        }
    }

    public void WanderOnStage()
    {
        if (!wanderArea) return;

        Vector3 wanderPoint = new Vector3(
            Random.Range(wanderArea.bounds.min.x, wanderArea.bounds.max.x),
            Random.Range(wanderArea.bounds.min.y, wanderArea.bounds.max.y),
            Random.Range(wanderArea.bounds.min.z, wanderArea.bounds.max.z)
        );

        agent.SetDestination(wanderPoint);
    }

    private void Act()
    {

    }
}
