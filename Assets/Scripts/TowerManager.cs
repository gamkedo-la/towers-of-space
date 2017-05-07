using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    
    public GameObject selectedTowerType;

    public void SelectTowerType(GameObject prefab) {
        selectedTowerType = prefab;
    }

}
