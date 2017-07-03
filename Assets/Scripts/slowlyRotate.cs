using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowlyRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			// Slowly rotate the object arond its X axis at 1 degree/second.
			transform.Rotate(Time.deltaTime, 0, 0);		
	}
}
