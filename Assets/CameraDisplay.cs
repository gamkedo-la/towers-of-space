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
    private float movementZ;
    private float targetMovement;
    private float targetMovementX; //These two are for when the player is holding translation lock
    private float targetMovementY;

    private float fov; //Field of view, in degrees
    private float minFov = 15f;
    private float maxFov = 90f;
    public float sensitivity = 15f; //Mouse scroll sensitivity
    private float zoomSmoothTime = 0.3f;
    private float currentZoomVelocity = 25;

    private float resetMove;

    // Use this for initialization
    void Start () {

        playerCamera = PlayerCamera.GetComponent<Camera>();
        overheadCamera = OverheadCamera.GetComponent<Camera>();

        playerCamera.enabled = true;  //Start at default position
        overheadCamera.enabled = false;
        canvas = GUILayout.GetComponent<Canvas>();

        fov = playerCamera.fieldOfView;

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


        Vector3 angle = playerCamera.transform.rotation.eulerAngles * Mathf.Deg2Rad; //Calculates the proportions of Y and Z movements necessary to move forwards

        if (playerCamera.enabled && !Input.GetKey(KeyCode.LeftShift)) //Only triggers if not in overhead view
        {

            targetRotation = Input.GetAxis("Horizontal") * rotationSpeed * resetMove * (invert ? 1 : -1) * Time.deltaTime;          //Rotates the camera around by pressing left/right (or a,d).
            playerCamera.transform.RotateAround(rotationPoint.position, Vector3.up, targetRotation);   //Thought about adding smoothing but it actually feels fine like this

            targetMovement = Input.GetAxis("Vertical"); //The forwards direction is relative to the rotation. Will make it so that holding a button allows for translation only.

            movementY = Mathf.Sin(angle.x) * targetMovement * translationSpeed * Time.deltaTime;
            movementZ = Mathf.Cos(angle.x) * targetMovement * translationSpeed * Time.deltaTime;
            playerCamera.transform.Translate(0, movementY, movementZ);
        }

        else if (playerCamera.enabled && Input.GetKey(KeyCode.LeftShift))
        {
            targetMovementX = Input.GetAxis("Horizontal") * resetMove;
            targetMovementY = Input.GetAxis("Vertical") * resetMove;

            movementX = targetMovementX * translationSpeed * Time.deltaTime;
            movementY = Mathf.Sin(angle.x) * targetMovementY * translationSpeed * Time.deltaTime;
            movementZ = Mathf.Cos(angle.x) * targetMovementY * translationSpeed * Time.deltaTime; //Which is actually the camera's local Z axis

            playerCamera.transform.Translate(movementX, movementY, movementZ);
            
        }

        if (resetMove != 1)    //This section will be polished later. Basically the previous inputs for one mode are kept for the other, resulting in camera sliding when switching.
        {                      //To counter this, I tried multiplying by an incremental float (resetMove) that could both nullify the slide and make the translation seamless. Will tweak the numbers to make it better
            resetMove += 0.05f;
            resetMove = Mathf.Clamp(resetMove, 0, 1);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift)) //Resets the resetMove on pressing/releasing Shift
        {
            resetMove = 0.05f;
        }

        //Enables zooming with the scroll wheel
        float targetFov = fov - (Input.GetAxis("Mouse ScrollWheel") * sensitivity);
        fov = Mathf.SmoothDamp(fov, targetFov, ref currentZoomVelocity, zoomSmoothTime);
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;

    }
}
