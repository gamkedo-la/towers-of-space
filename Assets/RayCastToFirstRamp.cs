using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastToFirstRamp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit rayInfo;
        int UpRamp= LayerMask.GetMask("UpRamp");

        if(Physics.Raycast(transform.position, Vector3.forward, out rayInfo, 5.0f, UpRamp))
        {
            Debug.Log("ship close by 5");
        }
        
          //   if (other.gameObject.layer == LayerMask.NameToLayer("EndZone"))

        //transform.Rotate(Vector3.up, 30.0f * Time.deltaTime);
    }
}
