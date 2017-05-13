using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisplay : MonoBehaviour {

    public Camera PlayerCamera; //aka the main camera
    public Camera OverheadCamera; //duplicate of minimap camera with no Minimap Target Texture
    private Camera playerCamera;  //These two are the camera components of the above cameras (not sure if they could be combined tbh)
    private Camera overheadCamera;

    public GameObject GUILayout; //aka the Canvas GameObject
    private Canvas canvas; //The Canvas component of the above

    public Transform rotationPoint; //The transform around which the camera rotates
    private float targetRotation;
    public float rotationSpeed = 1.5f; //Multiplies camera rotation
    public bool invert = false; //Switches left/right in camera rotation (the inverted feels weird, but you can do it :P)

    public float translationSpeed = 0.15f; //Moving the camera forwards and backwards
    private float movementX;
    private float movementY;
    private float targetMovement;

    // Use this for initialization
    void Start () {

        playerCamera = PlayerCamera.GetComponent<Camera>();
        overheadCamera = OverheadCamera.GetComponent<Camera>();

        playerCamera.enabled = true;  //Start at default position
        overheadCamera.enabled = false;
        canvas = GUILayout.GetComponent<Canvas>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Space))
        {
            playerCamera.enabled = false;
            overheadCamera.enabled = true;
            canvas.worldCamera = overheadCamera;  //Moves the UI with the buttons (worldCamera switches the camera to which it's attached)
        }
        else
        {
            playerCamera.enabled = true;  //return to main camera
            overheadCamera.enabled = false;
            canvas.worldCamera = playerCamera;
        }

        if (playerCamera.enabled) //Only triggers if not in overhead view
        {
            targetRotation = Input.GetAxis("Horizontal") * rotationSpeed * (invert ? 1 : -1);          //Rotates the camera around by pressing left/right (or a,d).
            playerCamera.transform.RotateAround(rotationPoint.position, Vector3.up, targetRotation);   //Thought about adding smoothing but it actually feels fine like this

            targetMovement = Input.GetAxis("Vertical"); //The forwards direction is relative to the rotation. Will make it so that holding a button allows for translation only.
            Vector3 angle = playerCamera.transform.rotation.eulerAngles * Mathf.Deg2Rad;
            movementX = Mathf.Cos(angle.x) * targetMovement * translationSpeed; //Which is actually the camera's local Z axis
            movementY = Mathf.Sin(angle.x) * targetMovement * translationSpeed; 
            playerCamera.transform.Translate(0, movementY, movementX);
        }

	}
}
