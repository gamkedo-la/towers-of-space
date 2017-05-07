﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour {

    bool hasTower = false;
    
    void OnMouseUp() {
        if (hasTower) {
            return;
        }

        TowerManager towerManager = GameObject.FindObjectOfType<TowerManager>();

        if (towerManager.selectedTowerType != null) {
            ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
            if (!sm.HasEnergy(towerManager.selectedTowerType.GetComponent<Tower>().energy)) {
                Debug.Log("Not enough energy!");
                return;
            }

            sm.UseEnergy (towerManager.selectedTowerType.GetComponent<Tower>().energy);

            //Vector3 pos = transform.position;
            //pos.y += 1;
            Instantiate(towerManager.selectedTowerType, transform.position, transform.rotation);

            // todo disable the script, or temporarily stop it from spawning another tower
            //Destroy(transform.parent.gameObject);
            //gameObject.SetActive (false);
            hasTower = true;
        }
    }

}
