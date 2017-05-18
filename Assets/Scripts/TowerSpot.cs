using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour {
    public GameObject popupButtonCanvas;

    public bool hasTower = false;
    
    void OnMouseUp() {
        if (hasTower) {
            return;
        }

        TowerManager.instance.DisplayTowerSelectionPopup(gameObject);
    }

}
