using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour {
    public GameObject popupButtonCanvas;

    public bool hasTower = false;
    public string currentType;
    
    void OnMouseUp() {
        if (hasTower)
        {
            UIController.instance.DisplayTowerOptions(gameObject);
            
            return;
        }

        else
        {
            UIController.instance.DisplayTowerCreation(gameObject);
        }
    }

}
