using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBuilderEffect : MonoBehaviour {
    private bool active;
    [SerializeField] private float maxPosition = 0.46f;
    [SerializeField] private float sweepTime = 1f;
    private float sweepTimer;
    private float height;
    private LineRenderer[] lasers;

    void Start() {
        lasers = GetComponentsInChildren<LineRenderer>();
        active = false;

        begin();
    }

    void Update() {
        if (active) {
            float temp = Mathf.Sin(sweepTimer / sweepTime * Mathf.PI);
            //setLaserPosition(Mathf.Abs(sweepTimer) / sweepTime * maxPosition);
            setLaserPosition((temp / 2f + 0.5f) * maxPosition);

            sweepTimer += Time.deltaTime;
            if(sweepTimer > sweepTime) {
                sweepTimer = sweepTimer % sweepTime - sweepTime;
            }
        }
    }

    public void begin() {
        active = true;
        sweepTimer = 0;
        height = 1;

        foreach (LineRenderer laser in lasers) {
            laser.gameObject.SetActive(true);
        }
    }

    public void end() {
        active = false;

        foreach (LineRenderer laser in lasers) {
            laser.gameObject.SetActive(false);
        }
    }

    public void setHeight(float heightIn) {
        height = heightIn;
    }

    private void setLaserPosition(float laserPosition) {
        float laserDirection;
        for (int i = 0; i < lasers.Length; i++){
            laserDirection = ((float)i / lasers.Length) * 2 * Mathf.PI;

            if (i % 2 == 0) {
                lasers[i].SetPosition(1, new Vector3(Mathf.Sin(laserDirection) * laserPosition,
                                                     1 - height - 1.5f,
                                                     Mathf.Cos(laserDirection) * laserPosition));
            }
            else {
                lasers[i].SetPosition(1, new Vector3(Mathf.Sin(laserDirection) * (maxPosition - laserPosition),
                                                     1 - height - 1.5f,
                                                     Mathf.Cos(laserDirection) * (maxPosition - laserPosition)));
            }
        }
    }
}
