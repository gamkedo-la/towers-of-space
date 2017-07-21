using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastToFirstRamp : MonoBehaviour {

    Animator animator;
    UnityEngine.AI.NavMeshAgent navMeshAgent;

    private bool upTheFirstRamp = false;
    private bool downTheFirstRamp = false;

    GameObject EndFirstRampGO;
    Collider EndFirstRampCol;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        EndFirstRampGO = GameObject.Find("EndFirstRampPoint");
    }
	
	// Update is called once per frame
	void Update () {

        RaycastHit rayInfo;
        int FirstRamp = LayerMask.GetMask("FirstRamp");
        int SecondRamp = LayerMask.GetMask("SecondRamp");

        //First Ramp Up
        //Debug.Log(Physics.Raycast(transform.position, Vector3.right, out rayInfo, 10f, FirstRamp));
        if (Physics.Raycast(transform.position, Vector3.right, out rayInfo, 10f, FirstRamp))
        {
            upTheFirstRamp = true;
        } else
        {
            upTheFirstRamp = false;
        }
        animator.SetBool("upTheFirstRamp", upTheFirstRamp);

    }    
}
