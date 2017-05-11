using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public GameObject selectedTowerType;

    public GameObject[] TowerTypes;
    public bool isPaused = false;
    public GameObject pausePanel;

    public void SelectTowerType(GameObject prefab)
    {
        selectedTowerType = prefab;
    }

    //Moved the buttons out into the editor so they can be more easily modified - Renaud Marshall
    /*private void OnGUI()
    {
        if (GUI.Button(new Rect(145, 5, 100, 50), "Standard"))
        {
            selectedTowerType = TowerTypes[0];
        }

        if (GUI.Button(new Rect(285, 5, 100, 50), "Double Barrel"))
        {
            selectedTowerType = TowerTypes[1];
        }
    }*/

    public void ChangeTowerType(int towerTypeIndex)
    {
        if (isPaused == false)
        {
            selectedTowerType = TowerTypes[towerTypeIndex];
        }
    }

    public void TogglePause()
    {
        if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }
}

