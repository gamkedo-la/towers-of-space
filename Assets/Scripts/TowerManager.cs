using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public GameObject selectedTowerType;

    public GameObject[] TowerTypes;

    public void SelectTowerType(GameObject prefab)
    {
        selectedTowerType = prefab;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(145, 5, 100, 50), "Standard"))
        {
            selectedTowerType = TowerTypes[0];
        }

        if (GUI.Button(new Rect(285, 5, 100, 50), "Double Barrel"))
        {
            selectedTowerType = TowerTypes[1];
        }
    }
}

