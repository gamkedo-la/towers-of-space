using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBuilderEffect : MonoBehaviour {
    private enum State {
        Idle,
        Takeoff,
        Hovering,
        Landing,
        Building,
        Hiding
    }

    private State state;
    //private bool hovering; // later effect to add
    [SerializeField] private float maxPosition = 0.46f;
    [SerializeField] private float sweepTime = 1f;
    [SerializeField] private float flightAnimationTime = 0.5f;
    private float sweepTimer;
    private float flightAnimationTimer;
    private float laserHeight;
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
        if (state == State.Building) {
            float circularizedPosition = Mathf.Sin(sweepTimer / sweepTime * Mathf.PI);
            setLaserPosition((circularizedPosition / 2f + 0.5f) * maxPosition);

            sweepTimer += Time.deltaTime;
            if(sweepTimer > sweepTime) {
                sweepTimer = sweepTimer % sweepTime - sweepTime;
            }
        }
        else if (state == State.Takeoff) {
            setEmitterPosition(takeoffCurve, flightAnimationTimer / flightAnimationTime);

            flightAnimationTimer += Time.deltaTime;
            if(flightAnimationTimer > flightAnimationTime) {
                state = State.Hovering;
                flightAnimationTimer = flightAnimationTime;
            }
        }
        else if (state == State.Landing) {
            setEmitterPosition(landingCurve, flightAnimationTimer / flightAnimationTime);

            flightAnimationTimer += Time.deltaTime;
            if (flightAnimationTimer > flightAnimationTime) {
                state = State.Hovering;
                flightAnimationTimer = flightAnimationTime;
            }
        }
        else if(state == State.Hovering) {

        }
    }

    public void beginConstruction() {
        state = State.Building;
        sweepTimer = 0;
        laserHeight = 0;

        foreach (LineRenderer laser in lasers) {
            laser.gameObject.SetActive(true);
        }
    }

    public void endConstruction(bool constructionComplete) {
        if (constructionComplete) {
            vanish();
        }
        else {
            land();
        }

        foreach (LineRenderer laser in lasers) {
            laser.gameObject.SetActive(false);
        }
    }

    public void takeoff() {
        state = State.Takeoff;
        flightAnimationTimer = 0;
    }

    public void land() {
        state = State.Landing;
        flightAnimationTimer = 0;
    }

    private void vanish() {
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

    private void setLaserPosition(float laserPosition) {
        float laserDirection;
        for (int i = 0; i < lasers.Length; i++){
            laserDirection = ((float)i / lasers.Length) * 2 * Mathf.PI;

            if (i % 2 == 0) {
                lasers[i].SetPosition(1, new Vector3(Mathf.Sin(laserDirection) * laserPosition,
                                                     laserHeight - 1.5f + (flightPosition.localPosition.y - laserEmitter.localPosition.y),
                                                     Mathf.Cos(laserDirection) * laserPosition));
            }
            else {
                lasers[i].SetPosition(1, new Vector3(Mathf.Sin(laserDirection) * (maxPosition - laserPosition),
                                                     laserHeight - 1.5f + (flightPosition.localPosition.y - laserEmitter.localPosition.y),
                                                     Mathf.Cos(laserDirection) * (maxPosition - laserPosition)));
            }
        }
    }

    private void setEmitterPosition(AnimationCurve curve, float t) {
        laserEmitter.localPosition = Vector3.LerpUnclamped(landingPosition.localPosition, flightPosition.localPosition, curve.Evaluate(t));
    }
}
