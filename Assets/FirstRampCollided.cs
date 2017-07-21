using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRampCollided : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent navMeshAgent;

    public void OnCollisionEnter(Collision collInfo)
    {

        RayCastToFirstRamp AltAmoredEnemyScript = collInfo.gameObject.GetComponent<RayCastToFirstRamp>();
        if ((AltAmoredEnemyScript != null))
        {
            Debug.Log("Collider hit");
        }


        if (collInfo.gameObject.name == "AltArmored Enemy")
        {
            navMeshAgent.updateRotation = false;
            Debug.Log("Collider hit");
        }
    }
}
