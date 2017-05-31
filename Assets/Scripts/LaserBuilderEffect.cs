using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBuilderEffect : MonoBehaviour {
    private enum State {
        Idle,
        Takeoff,
        Hovering,
        Landing,
        Hiding
    }

    private State state;
    private bool building; // later effect to add
    [SerializeField] private float maxPosition = 0.46f;
    [SerializeField] private float sweepTime = 1f;
    [SerializeField] private float flightAnimationTime = 0.5f;
    private float sweepTimer;
    private float flightAnimationTimer;
    private float laserHeight;
    private float laserWidth;
    [SerializeField] private Transform laserEmitter;
    [SerializeField] private Transform landingPosition;
    [SerializeField] private Transform flightPosition;
    public AnimationCurve takeoffCurve;
    public AnimationCurve landingCurve;
    private LineRenderer[] lasers;

    void Start() {
        lasers = GetComponentsInChildren<LineRenderer>(true);
        state = State.Idle;
    }

    void Update() {
        if (building) {
            float circularizedPosition = Mathf.Sin(sweepTimer / sweepTime * Mathf.PI);
            setLaserPosition((circularizedPosition / 2f + 0.5f) * maxPosition);

            sweepTimer += Time.deltaTime;
            if (sweepTimer > sweepTime) {
                sweepTimer = sweepTimer % sweepTime - sweepTime;
            }
        }

        if (state == State.Takeoff) {
            flightAnimationTimer += Time.deltaTime;
            if (flightAnimationTimer > flightAnimationTime) {
                state = State.Hovering;
                flightAnimationTimer = flightAnimationTime;
            }

            setEmitterPosition(takeoffCurve, flightAnimationTimer / flightAnimationTime);
        }
        else if (state == State.Landing) {
            flightAnimationTimer += Time.deltaTime;
            if (flightAnimationTimer > flightAnimationTime) {
                state = State.Idle;
                flightAnimationTimer = flightAnimationTime;
            }

            setEmitterPosition(landingCurve, flightAnimationTimer / flightAnimationTime);
        }
        else if (state == State.Hovering) {

        }
    }

    public void beginConstruction() {
        building = true;
        sweepTimer = 0;
        laserHeight = 0;

        foreach (LineRenderer laser in lasers) {
            laser.gameObject.SetActive(true);
        }
    }

    public void endConstruction(bool constructionComplete) {
        building = false;

        if (!constructionComplete) {
            land();
        }

        foreach (LineRenderer laser in lasers) {
            laser.gameObject.SetActive(false);
        }
    }

    public void takeoff() {
        beginFlightPath(State.Takeoff);
    }

    public void land() {
        beginFlightPath(State.Landing);
    }

    private void beginFlightPath(State flightPlan) {
        if (state == flightPlan) { // Already executing this plan, so do nothing
            return;
        }
        else if (state == State.Takeoff || state == State.Landing) { // Going in the opposite direction
            float t = flightAnimationTimer / flightAnimationTime;
            AnimationCurve path = (state == State.Takeoff) ? takeoffCurve : landingCurve; // Path to follow
            float position = (state == State.Takeoff) ? landingCurve.Evaluate(t) : takeoffCurve.Evaluate(t); // Local height of emitter

            // Set animation time based on height
            flightAnimationTimer = flightAnimationTime * findAnimationCurveTimeByValue(path, position, 100); 
        }
        else { //Fresh animation from the start
            flightAnimationTimer = 0;
        }

        state = flightPlan;
    }

    public void vanish() {
        //state = State.Hiding;
        reset(); //Change to add hiding animation
    }

    public void reset() {
        state = State.Idle;
        laserEmitter.localPosition = landingPosition.localPosition;
    }

    public void setHeight(float heightIn) {
        laserHeight = heightIn;
    }

    public void setWidth(float widthIn) {
        laserWidth = widthIn;
    }

    private void setLaserPosition(float laserPosition) {
        float laserDirection;
        float xRatio, zRatio;
        float laserY = laserHeight - 1.5f + (flightPosition.localPosition.y - laserEmitter.localPosition.y);

        for (int i = 0; i < lasers.Length; i++){
            laserDirection = ((float)i / lasers.Length) * 2 * Mathf.PI;
            xRatio = Mathf.Sin(laserDirection) * laserWidth;
            zRatio = Mathf.Cos(laserDirection) * laserWidth;

            if (i % 2 == 0) {
                lasers[i].SetPosition(1, new Vector3(xRatio * laserPosition, laserY, zRatio * laserPosition));
            }
            else {
                lasers[i].SetPosition(1, new Vector3(xRatio * (maxPosition - laserPosition), laserY, zRatio * (maxPosition - laserPosition)));
            }
        }
    }

    private void setEmitterPosition(AnimationCurve curve, float t) {
        laserEmitter.localPosition = Vector3.LerpUnclamped(landingPosition.localPosition, flightPosition.localPosition, curve.Evaluate(t));
    }


    /// <summary>
    /// Finds the first instance of a target value in an animation curve.
    /// </summary>
    /// <param name="targetCurve">Curve to search.</param>
    /// <param name="targetValue">Value to find in curve.</param>
    /// <param name="steps">Search granularity.</param>
    /// <returns>Returns a float that indicates the time at which the value first occurs or 0 on failure.</returns>
    private float findAnimationCurveTimeByValue(AnimationCurve targetCurve, float targetValue, int steps) {
        float marginOfError = (1f / steps) * 2f;

        for (int i = 0; i <= steps; i++) {
            float valueToCheck = targetCurve.Evaluate((float)i / steps);
            //Debug.Log(valueToCheck);
            if (valueToCheck > targetValue - marginOfError && valueToCheck < targetValue + marginOfError) {
                return (float)i / steps;
            }
        }
        Debug.Log("Failed to find animation value - Params: " + targetCurve.ToString() + ", " + targetValue);
        return 0;
    }
}
