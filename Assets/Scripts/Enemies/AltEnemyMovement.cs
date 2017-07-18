using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltEnemyMovement : MonoBehaviour
{

    //public Transform destination;

    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        Vector3 myVector = new Vector3(6.4f, -4.52f, 13.48f);

        //Transform destination = transform.position(6.4, -4.52, 13.48);
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        //agent.SetDestination(destination.position);
        agent.SetDestination(myVector);

    }
    //void Update()
    //    {
    //        if (!hasNextWaypoint)
    //        {
    //            SetNextNode();
    //
    //            if (!hasNextWaypoint)
    //            {
    //                // No more path nodes!
    //                ReachedGoal();
    //            }
    //        }




            //if (agent.transform.position = myVector)
            //{
            //    Destroy(gameObject);
            //    GameController.instance.LoseLife(); //instance is the Score Manager
            //    GameController.instance.CheckGameWon();
            //}
      //  }

}