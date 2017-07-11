using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisplay : MonoBehaviour
{

    public Camera PlayerCamera; //aka the main camera
    public Camera OverheadCamera; //duplicate of minimap camera with no Minimap Target Texture
    private Camera playerCamera;  //These two are the camera components of the above cameras (not sure if they could be combined tbh)
    private Camera overheadCamera;
    //private Camera activeCamera;

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
    private float momentumTranslateVertical = 0f;
    private float momentumTranslateHorizontal = 0f;
    private float momentumRotateVertical = 0f;
    private float momentumRotateHorizontal = 0f;
    public float momentumRampTime = 3f;

    private float fov; //Field of view, in degrees
    private float minFov = 15f;
    private float maxFov = 90f;
    float targetFov = 63f;
    public float sensitivity = 15f; //Mouse scroll sensitivity
    private float zoomSmoothTime = 0.3f;
    private float currentZoomVelocity = 0;

    private float resetMove;

    // Use this for initialization
    void Start()
    {

        playerCamera = PlayerCamera.GetComponent<Camera>();
        overheadCamera = OverheadCamera.GetComponent<Camera>();

        playerCamera.enabled = true;  //Start at default position
        overheadCamera.enabled = false;
        //canvas = GUILayout.GetComponent<Canvas>();

        fov = playerCamera.fieldOfView;

    }

    // Update is called once per frame
    void Update()
    {
        float timeScale = Time.deltaTime / GameController.instance.gameTimeScale;

        if (Input.GetKey(KeyCode.Space))
        {
            playerCamera.enabled = false;
            overheadCamera.enabled = true;
            //canvas.worldCamera = overheadCamera;  //Moves the UI with the buttons (worldCamera switches the camera to which it's attached)
        }
        else
        {
            playerCamera.enabled = true;  //return to main camera
            overheadCamera.enabled = false;
            //canvas.worldCamera = playerCamera;
        }

        Vector3 angle = playerCamera.transform.rotation.eulerAngles * Mathf.Deg2Rad; //Calculates the proportions of Y and Z movements necessary to move forwards

        UpdateInputMomentum(timeScale);

        if (playerCamera.enabled && !Input.GetKey(KeyCode.LeftShift)) //Only triggers if not in overhead view
        {
            CameraRotate(angle, playerCamera);
        }

        else if (playerCamera.enabled && Input.GetKey(KeyCode.LeftShift))
        {
            CameraTranslate(angle, playerCamera);

        }
        else if (overheadCamera.enabled && Input.GetKey(KeyCode.LeftShift))
        {
            targetMovementX = Input.GetAxisRaw("Horizontal") * resetMove;
            targetMovementY = Input.GetAxisRaw("Vertical") * resetMove;

            CameraTranslate(angle, overheadCamera);

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
        if(!Mathf.Equals(Input.GetAxis("Mouse ScrollWheel"), 0f)) {
            targetFov -= (Input.GetAxis("Mouse ScrollWheel") * sensitivity);
            targetFov = Mathf.Clamp(targetFov, minFov, maxFov);
        }
        fov = Mathf.SmoothDamp(fov, targetFov, ref currentZoomVelocity, zoomSmoothTime, Mathf.Infinity, timeScale);
        Camera.main.fieldOfView = fov;

        //Axis changes by about 0.05 per frame

    }
    void CameraTranslate(Vector3 angle, Camera camera)
    {
        targetMovementX = momentumTranslateHorizontal * resetMove;
        targetMovementY = momentumTranslateVertical * resetMove;

        movementX = targetMovementX * translationSpeed * 0.0166666667f;
        movementY = Mathf.Sin(angle.x) * targetMovementY * translationSpeed * 0.0166666667f;
        movementZ = Mathf.Cos(angle.x) * targetMovementY * translationSpeed * 0.0166666667f; //Which is actually the camera's local Z axis

        camera.transform.Translate(movementX, movementY, movementZ);
    }

    void CameraRotate(Vector3 angle, Camera camera)
    {
        targetRotation = momentumRotateHorizontal * rotationSpeed * resetMove * (invert ? 1 : -1) * 0.0166666667f;          //Rotates the camera around by pressing left/right (or a,d).
        camera.transform.RotateAround(rotationPoint.position, Vector3.up, targetRotation);   //Thought about adding smoothing but it actually feels fine like this

        targetMovement = momentumRotateVertical; //The forwards direction is relative to the rotation. Will make it so that holding a button allows for translation only.

        movementY = Mathf.Sin(angle.x) * targetMovement * translationSpeed * 0.0166666667f;
        movementZ = Mathf.Cos(angle.x) * targetMovement * translationSpeed * 0.0166666667f;
        camera.transform.Translate(0, movementY, movementZ);
    }

    //Update all input momentum variables
    private void UpdateInputMomentum(float timeScale) {

        //All variables decay if they are not currently being added to
        if (!Mathf.Equals(Input.GetAxisRaw("Horizontal"), 0f)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                momentumTranslateHorizontal = Mathf.MoveTowards(momentumTranslateHorizontal, Input.GetAxisRaw("Horizontal"), momentumRampTime * timeScale);
                momentumRotateHorizontal = Mathf.MoveTowards(momentumRotateHorizontal, 0f, momentumRampTime * timeScale);
            }
            else {
                momentumRotateHorizontal = Mathf.MoveTowards(momentumRotateHorizontal, Input.GetAxisRaw("Horizontal"), momentumRampTime * timeScale);
                momentumTranslateHorizontal = Mathf.MoveTowards(momentumTranslateHorizontal, 0f, momentumRampTime * timeScale);
            }
        }
        else {
            momentumTranslateHorizontal = Mathf.MoveTowards(momentumTranslateHorizontal, 0f, momentumRampTime * timeScale);
            momentumRotateHorizontal = Mathf.MoveTowards(momentumRotateHorizontal, 0f, momentumRampTime * timeScale);
        }

        if (!Mathf.Equals(Input.GetAxisRaw("Vertical"), 0f)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                momentumTranslateVertical = Mathf.MoveTowards(momentumTranslateVertical, Input.GetAxisRaw("Vertical"), momentumRampTime * timeScale);
                momentumRotateVertical = Mathf.MoveTowards(momentumRotateVertical, 0f, momentumRampTime * timeScale);
            }
            else {
                momentumRotateVertical = Mathf.MoveTowards(momentumRotateVertical, Input.GetAxisRaw("Vertical"), momentumRampTime * timeScale);
                momentumTranslateVertical = Mathf.MoveTowards(momentumTranslateVertical, 0f, momentumRampTime * timeScale);
            }
        }
        else {
            momentumTranslateVertical = Mathf.MoveTowards(momentumTranslateVertical, 0f, momentumRampTime * timeScale);
            momentumRotateVertical = Mathf.MoveTowards(momentumRotateVertical, 0f, momentumRampTime * timeScale);
        }
    }
}
