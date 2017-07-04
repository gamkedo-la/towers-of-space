using UnityEngine;
using System.Collections;

// $CTK this works only if the xform isn't touched by any other components
public class Wobble : MonoBehaviour {

	// just in case we don't want starting pos
	public float offsetX=0.0f; // $CTK
	public float offsetY=0.0f; // $CTK
	public float offsetZ=0.0f; // $CTK

	public float speedX=3.5f;
	public float speedY=3.5f;
	public float speedZ=3.5f;

	// how big is the wobble?
	public float magnitudeX=1;
	public float magnitudeY=1;
	public float magnitudeZ=1;

	public float rotspeedX=3.5f;
	public float rotspeedY=3.5f;
	public float rotspeedZ=3.5f;
	public float rotmagnitudeX=1;
	public float rotmagnitudeY=1;
	public float rotmagnitudeZ=1;
	private float rotstartingX=0;
	private float rotstartingY=0;
	private float rotstartingZ=0;

	// so every object starts at a diff phase
	private float timeoffsetX=0; 
	private float timeoffsetY=0; 
	private float timeoffsetZ=0; 

	// we orbit the starting pos
	private float startingX=0;
	private float startingY=0;
	private float startingZ=0;
	
	// Use this for initialization
	void Start () {
		timeoffsetX=0;//Random.Range(-5f, 5f);
		timeoffsetY=0;//Random.Range(-5f, 5f);
		timeoffsetZ=8;//Random.Range(-5f, 5f);

		startingX=transform.localPosition.x;
		startingY=transform.localPosition.y;
		startingZ=transform.localPosition.z;

		rotstartingX=transform.rotation.x;
		rotstartingY=transform.rotation.y;
		rotstartingZ=transform.rotation.z;
	}
	
	// Update is called once per frame
	void Update () {
		float x=offsetX+startingX+magnitudeX*Mathf.Sin(speedX*Time.time+timeoffsetX); // $CTK
		float y=offsetY+startingY+magnitudeY*Mathf.Sin(speedY*Time.time+timeoffsetY); // $CTK
		float z=offsetZ+startingZ+magnitudeZ*Mathf.Sin(speedZ*Time.time+timeoffsetZ); // $CTK
		transform.localPosition=new Vector3(x, y, z);

		float rx=rotstartingX+rotmagnitudeX*Mathf.Sin(rotspeedX*Time.time+timeoffsetX); // $CTK
		float ry=rotstartingY+rotmagnitudeY*Mathf.Sin(rotspeedY*Time.time+timeoffsetY); // $CTK
		float rz=rotstartingZ+rotmagnitudeZ*Mathf.Sin(rotspeedZ*Time.time+timeoffsetZ); // $CTK
		transform.rotation = Quaternion.Euler(rx,ry-180,rz);

	}
}
