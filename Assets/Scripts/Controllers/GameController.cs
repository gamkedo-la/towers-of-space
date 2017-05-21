﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject selectedTowerType; //Related to tower spawning
    public GameObject[] TowerTypes;
    public GameObject towerSpotToModify;
    private Tower currentTowerClass; //This is the Tower component of the selected type, NOT the Tower object

    public bool isPaused = false; //Related to time manipulation
    public float gameTimeScale = 1.0f;

    public float timeBeforeSpawning = 4f; //Related to enemy spawning
    public float timeBetweenEnemies = 0.25f;

    public int lives = 20;
    public int energy = 20;
    private int basicCount = 0;
    private int doubleCount = 0;
    private int empCount = 0;
    private int currentCost;

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

    public void TowerChange(string towerType, bool creating)
    {
        int modCount; //Changes the tower count
        int modIndex; //Changes the index from which we take the cost/refund

        if (creating)
        {
            modCount = 1;
            modIndex = 0;
        }
        else //implies destroying
        {
            modCount = -1;
            modIndex = -1;
        }

        switch (towerType)
        {
            case "Basic":
                selectedTowerType = TowerTypes[0]; //Gets the GameObject
                currentTowerClass = selectedTowerType.GetComponent<Tower>(); //Gets its Tower component
                currentCost = currentTowerClass.costLadder[basicCount + modIndex]; //Access the array based on the number of towers
                basicCount += modCount; //Move up the ladder
                break;
            case "Double":
                selectedTowerType = TowerTypes[1];
                currentTowerClass = selectedTowerType.GetComponent<Tower>();
                currentCost = currentTowerClass.costLadder[doubleCount + modIndex];
                doubleCount += modCount;
                break;
            case "EMP":
                selectedTowerType = TowerTypes[2];
                currentTowerClass = selectedTowerType.GetComponent<Tower>();
                currentCost = currentTowerClass.costLadder[empCount + modIndex];
                empCount += modCount;
                break;
            default:
                selectedTowerType = null;
                Debug.Log("No tower of name: " + towerType);
                break;
        }
    } //This function both gets the cost/refund and changes the number of towers depending on creation or destruction

    public void InstantiateTower(string towerType)
    {
        TowerChange(towerType, true); //See comments in function

        if (selectedTowerType != null && isPaused != true)
        {

            if (!HasEnergy(currentCost))
            {
                Debug.Log("Not enough energy!");
                return;
            }

            UseEnergy(currentCost);

            TowerSpot towerSpot = towerSpotToModify.GetComponent<TowerSpot>(); //This tells the spot what tower it holds, so it can share it when we click it later
            towerSpot.currentType = towerType;
            towerSpot.hasTower = true;
            GameObject tower = Instantiate(selectedTowerType, towerSpotToModify.transform.position, towerSpotToModify.transform.rotation);
            tower.name = "Tower"; //It's called Tower since there can only be one, and we can find it by name later
            tower.transform.SetParent(towerSpotToModify.transform); //Sets the tower as a child of the platform so that it can be accessed later

            UIController.instance.ClosePanel("Creation");

        }

        towerSpotToModify = null;
    }

    public void DestroyTower()
    {
        string onSpot = towerSpotToModify.GetComponent<TowerSpot>().currentType; //Because this time, we can't get the type from a button, we have to read it from the component

        TowerChange(onSpot, false); //Same function, but now we're destroying
   
        Transform attachedTower = towerSpotToModify.transform.Find("Tower"); //Finds the child tower Transform, using the transform
        if (attachedTower == null) //Sometimes doesn't catch the tower, will remove when other problems are fixed
        {
            return;
        }

        energy += currentCost; //Refunds energy
        UIController.instance.TextUpdate("Energy"); //Updates it

        Destroy(attachedTower.gameObject); //Destroys the GameObject attached to the transform

        towerSpotToModify.GetComponent<TowerSpot>().hasTower = false; //No tower for you

        UIController.instance.ClosePanel("Options");
    }

    public void LoseLife(int l = 1)
    {
        lives -= l;
        if (lives <= 0)
        {
            GameOver();
        }

        UIController.instance.TextUpdate("Lives");
    }

    public bool HasEnergy(int e = 1)
    {
        return e <= energy;
    }

    public void UseEnergy(int e = 1)
    {
        energy -= e;

        UIController.instance.TextUpdate("Energy");
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        // TODO: Send the player to a game-over screen instead!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
