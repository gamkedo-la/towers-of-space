using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public GameObject[] nextNodes;

    void Start()
    {
        // Randomly rotate the cube model to make the path a bit more interesting
        Transform cube = transform.FindChild("Cube");
        if (cube == null)
        {
            return;
        }

        Quaternion r = cube.rotation;
        int x = 90 * Random.Range(-2, 2);
        int y = 90 * Random.Range(-2, 2);
        int z = 90 * Random.Range(-2, 2);
        r.eulerAngles = new Vector3(x, y, z);

        cube.rotation = r;
    }

}
