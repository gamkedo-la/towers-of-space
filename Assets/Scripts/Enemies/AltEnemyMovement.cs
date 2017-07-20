using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltEnemyMovement : MonoBehaviour
{

    //public Transform destination;

    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        GameObject baseGO = GameObject.Find("Base");

        Vector3 destination = baseGO.transform.position;
        //Quaternion destinationRot = baseGO.transform.rotation;

        //Transform destination = transform.position(6.4, -4.52, 13.48);
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        //agent.SetDestination(destination.position);
        agent.SetDestination(destination);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EndZone"))
        {
            //Debug.Log("made it to the end? I just walked into: " + other.gameObject.name);
               Destroy(gameObject);
                GameController.instance.LoseLife(); //instance is the Score Manager
                GameController.instance.CheckGameWon();
        }

    }
}
