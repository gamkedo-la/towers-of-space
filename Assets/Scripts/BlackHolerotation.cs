using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHolerotation : MonoBehaviour {
    float angle = 0;
    float speed = (2 * Mathf.PI) / 5; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    float radius = 5;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        /*    float x;
            float y;

            angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
            x = Mathf.Cos(angle) * radius;
            y = Mathf.Sin(angle) * radius;
            transform.position = new Vector3(x, y, 140.4f);*/

        transform.Rotate(0.0f, Time.deltaTime * 20.0f,0.0f);
    }
}
