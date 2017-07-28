using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltEnemyRayCast : MonoBehaviour
{


    //UnityEngine.AI.NavMeshAgent navMeshAgent;



    // Update is called once per frame
    void Update()
    {

        RaycastHit rayInfoInFrontOfMe;
        RaycastHit rayInfoUnderMe;



        float upAngle = -30f;



        if (Physics.Raycast(transform.position, Vector3.down, out rayInfoUnderMe, 5f))
        {
            //upTheFirstRamp = true;
            if (Physics.Raycast(transform.position + transform.forward * 0.15f, Vector3.down, out rayInfoInFrontOfMe, 5f))
            {

                float pointUnderMe = rayInfoUnderMe.point.y;
                float pointInFontOfMe = rayInfoInFrontOfMe.point.y;

                //if(rayInfoUnderMe.point.y>rayInfoInFrontOfMe.point.y)
                //Debug.Log("NOW " + "underme: " + rayInfoUnderMe.point.y + " infront: " + rayInfoInFrontOfMe.point.y);
                if (pointUnderMe - pointInFontOfMe > 0.05)
                {
                    //Debug.Log("downhill" + "underme: " + rayInfoUnderMe.point.y + "infront: " + rayInfoInFrontOfMe.point.y);
                    //Debug.Log("downHill");
                    transform.Rotate(Vector3.right * -upAngle);

                    //} else if (rayInfoUnderMe.point.y < rayInfoInFrontOfMe.point.y)
                }
                else if (pointInFontOfMe - pointUnderMe > 0.05)
                {
                    //Debug.Log("uphill" + "underme: " + rayInfoUnderMe.point.y + "infront: " + rayInfoInFrontOfMe.point.y);
                    //Debug.Log("uphill");
                    transform.Rotate(Vector3.right * upAngle);
                    //Debug.DrawLine(transform.position, rayInfoUnderMe.point, Color.green);
                    //Debug.DrawLine(transform.position, rayInfoInFrontOfMe.point, Color.red);

                }
                else
                {
                    //Debug.Log("flat" + "underme: " + rayInfoUnderMe.point.y + "infront: " + rayInfoInFrontOfMe.point.y);
                    Debug.Log("Flat");
                    transform.Rotate(Vector3.zero);
                    //Debug.DrawLine(transform.position, rayInfoUnderMe.point, Color.cyan);
                    //Debug.DrawLine(transform.position, rayInfoInFrontOfMe.point, Color.blue);
                }
            }
            //Debug.Log("rayInfo " + rayInfo.normal);//bouncing back line perpendicular from the raycast - normal perpendicular line
        }


    }

}
