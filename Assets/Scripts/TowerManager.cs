using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public GameObject selectedTowerType;

    public GameObject[] TowerTypes;
    public bool isPaused = false;
    public GameObject pausePanel;
    public float gameTimeScale = 1.0f;
    public GameObject towerPanel;
    public Transform panelPosition;
    public GameObject towerSpotToModify;

    MenuVisibilityCtrl menuVisibilityCtrl;

    private void Awake()
    {
        instance = this;
        menuVisibilityCtrl = towerPanel.GetComponent<MenuVisibilityCtrl>();
    }

    private void Update()
    {
        if (panelPosition != null) {
            towerPanel.transform.position = Camera.main.WorldToScreenPoint(panelPosition.position); //The transform is moving around before clicking a button and its annoying! But it doesn't bug...
        }
    }

    /*public void SelectTowerType(GameObject prefab)
    {
        selectedTowerType = prefab;
    }

    public void ChangeTowerType(int towerTypeIndex)
    {
        if (isPaused == false)
        {
            selectedTowerType = TowerTypes[towerTypeIndex];
        }
    }*/

    public void TogglePause()
    {
        if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = gameTimeScale;
            pausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }

    public void SlowTime()
    {
        if (isPaused == false)
        {
            gameTimeScale = Mathf.Max(gameTimeScale / 2, 0.25f);
            Time.timeScale = gameTimeScale;
        }
    }

    public void ResetTimeScale()
    {
        if (isPaused == false)
        {
            gameTimeScale = 1.0f;
            Time.timeScale = gameTimeScale;
        }
    }

    public void SpeedUpTime()
    {
        if (isPaused == false)
        {
            gameTimeScale = Mathf.Min(gameTimeScale * 2, 4);
            Time.timeScale = gameTimeScale;
        }
    }

    public void DisplayTowerCreation(GameObject towerSpot)
    {
        if (isPaused == false && towerPanel.activeSelf == false) //Added a check to make it so we don't switch the panel when we click a button, however that means that we need to deactivate before changing panels
        {
            towerPanel.SetActive(true);
            panelPosition = towerSpot.transform;
            towerPanel.transform.position = Camera.main.WorldToScreenPoint(panelPosition.position);
            towerSpotToModify = towerSpot;
        }
    }

    public void InstantiateTower(string towerType)
    {
        switch (towerType)
        {
            case "Basic":
                selectedTowerType = TowerTypes[0];
                break;
            case "Double":
                selectedTowerType = TowerTypes[1];
                break;
            case "EMP":
                selectedTowerType = TowerTypes[2];
                break;
            default:
                selectedTowerType = null;
                Debug.Log("No tower of name: " + towerType);
                break;
        }


        if (selectedTowerType != null && isPaused != true)
        {
            
            if (!ScoreManager.instance.HasEnergy(selectedTowerType.GetComponent<Tower>().energy))
            {
                Debug.Log("Not enough energy!");
                return;
            }

            ScoreManager.instance.UseEnergy(selectedTowerType.GetComponent<Tower>().energy);
            Instantiate(selectedTowerType, towerSpotToModify.transform.position, towerSpotToModify.transform.rotation);
            towerSpotToModify.GetComponent<TowerSpot>().hasTower = true;

            towerPanel.SetActive(false);
            menuVisibilityCtrl.hide();
            
        }

        towerSpotToModify = null;
    }
}

