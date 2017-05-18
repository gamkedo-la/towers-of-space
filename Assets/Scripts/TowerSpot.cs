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

        //popupButtonCanvas.SetActive(true);
        //popupButtonCanvas.transform.position = Input.mousePosition;

       

        TowerManager.instance.DisplayTowerSelectionPopup(gameObject);

        /*if (towerManager.selectedTowerType != null && towerManager.isPaused != true)
        {
            if (!ScoreManager.instance.HasEnergy(towerManager.selectedTowerType.GetComponent<Tower>().energy))
            {
                Debug.Log("Not enough energy!");
                return;
            }

            ScoreManager.instance.UseEnergy (towerManager.selectedTowerType.GetComponent<Tower>().energy);

            Instantiate(towerManager.selectedTowerType, transform.position, transform.rotation);

            // todo disable the script, or temporarily stop it from spawning another tower
            //Destroy(transform.parent.gameObject);
            //gameObject.SetActive (false);
            hasTower = true;
        }*/
    }

}
