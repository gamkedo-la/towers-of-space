using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltEnemyMovement : MonoBehaviour
{

    public Transform destination;

    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.SetDestination(destination.position);
    }

}