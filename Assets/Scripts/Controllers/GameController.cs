using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject selectedTowerType; //Related to tower spawning
    public GameObject[] TowerTypes;
    public GameObject towerSpotToModify;

    public bool isPaused = false; //Related to time manipulation
    public float gameTimeScale = 1.0f;

    public float timeBeforeSpawning = 4f; //Related to enemy spawning
    public float timeBetweenEnemies = 0.25f;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TogglePause()
    {
        if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = gameTimeScale;
            UIController.instance.pausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            UIController.instance.pausePanel.SetActive(true);
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

    public void GameOver() //previously in ScoreManager
    {
        Debug.Log("Game Over");
        // TODO: Send the player to a game-over screen instead!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

            if (!UIController.instance.HasEnergy(selectedTowerType.GetComponent<Tower>().energy))
            {
                Debug.Log("Not enough energy!");
                return;
            }

            UIController.instance.UseEnergy(selectedTowerType.GetComponent<Tower>().energy);
            Instantiate(selectedTowerType, towerSpotToModify.transform.position, towerSpotToModify.transform.rotation);
            towerSpotToModify.GetComponent<TowerSpot>().hasTower = true;

            UIController.instance.towerPanel.SetActive(false);
            UIController.instance.menuVisibilityCtrl.hide();

        }

        towerSpotToModify = null;
    }
}
