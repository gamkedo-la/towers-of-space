using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisplay : MonoBehaviour {

    public Camera playerCamera; //aka the main camera
    public Camera overheadCamera; //duplicate of minimap camera with no Minimap Target Texture
    public GameObject GUILayout; //The Canvas GameObject

    private Canvas canvas; //The Canvas component

    // Use this for initialization
    void Start () {

        playerCamera.GetComponent<Camera>().enabled = false;  //Start at default position
        overheadCamera.GetComponent<Camera>().enabled = true;
        canvas = GUILayout.GetComponent<Canvas>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Space))
        {
            playerCamera.GetComponent<Camera>().enabled = false;
            overheadCamera.GetComponent<Camera>().enabled = true;
            canvas.worldCamera = overheadCamera;  //Moves the UI
        }
        else
        {
            playerCamera.GetComponent<Camera>().enabled = true;  //return to main camera
            overheadCamera.GetComponent<Camera>().enabled = false;
            canvas.worldCamera = playerCamera;
        }
		
	}
}
