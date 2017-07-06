using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject selectedTowerType; //Related to tower spawning
    public GameObject[] TowerTypes;
    public GameObject towerSpotToModify;
    private Tower currentTowerClass; //This is the Tower component of the selected type, NOT the Tower object

    public bool hasNextWave = true;
    public bool isPaused = false; //Related to time manipulation
    public float gameTimeScale = 1.0f;

    public int lives = 20;
    public int energy = 20;
	public int deconstructs = 2;

	private int currentCost;
	private int currentDeconstructsAvailable;

	public enum TowerTypeEnum
    {
        Basic,
        Double,
        EMP,
        Plasma
    }

    private int[] towerCounts = new int[System.Enum.GetNames(typeof(TowerTypeEnum)).Length]; //Ensures that the tower counts match the number of towers

    private void Awake()
    {
        instance = this;

        // Start out with 0 deconstructs, when next wave comes, it is reset
        currentDeconstructsAvailable = 0;

		UIController.instance.UpdateEnergy( energy );
		UIController.instance.UpdateLives( lives );
        UIController.instance.UpdateDeconstructs( currentDeconstructsAvailable );
	}

    protected void TogglePause()
    {
        if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = gameTimeScale;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void TogglePausePanel()
    {
        TogglePause();
        UIController.instance.pausePanel.SetActive(isPaused);
    }

    public void GameOver()
    {
        Invoke("ShowGameOver", 2);
    }

    public void ShowGameOver()
    {
        if (UIController.instance.gameOverPanel.activeSelf)
        {
            return;
        }

        TogglePause();
        UIController.instance.gameOverPanel.SetActive(isPaused);
    }

    public void CheckGameWon()
    {
        if (hasNextWave)
        {
            return;
        }

		Enemy[] enemies = FindObjectsOfType<Enemy>();
        // Current enemy is not 'gone' yet, so we need to check for more than 1
        if (enemies.Length > 1)
        {
            return;
        }

        if (0 < lives)
        {
            Invoke("ShowGameWon", 2);
        }
    }

    public void ShowGameWon()
    {
        if (UIController.instance.gameWonPanel.activeSelf)
        {
            return;
        }

        TogglePause();
        UIController.instance.gameWonPanel.SetActive(isPaused);
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

    public void TowerChange(TowerTypeEnum towerTypeEnum, bool creating)  //This function both gets the cost/refund and changes the number of towers depending on creation or destruction
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
        int index = (int)towerTypeEnum; //Gets the corresponding int of the enum value
        int currentCount = towerCounts[index];
        selectedTowerType = TowerTypes[index]; //Gets the GameObject
        currentTowerClass = selectedTowerType.GetComponent<Tower>(); //Gets its Tower component


        if (currentCount + modIndex > currentTowerClass.costLadder.Length - 1)
        {
            currentCost = currentTowerClass.costLadder[currentTowerClass.costLadder.Length - 1];
        }
        else if (currentCount + modIndex < 0) //Added for safety! Might remove when feeling confident
        {
            currentCost = currentTowerClass.costLadder[0];
        }
        else
        {
            currentCost = currentTowerClass.costLadder[currentCount + modIndex];  //Access the array based on the number of towers
        }
        if (HasEnergy(currentCost) && creating || !creating)
        {
            currentCount += modCount;
        }
        towerCounts[index] = currentCount; //Updates the tower count
    }

    public void InstantiateTower(string towerType)
    {
        TowerTypeEnum towerTypeEnum = (TowerTypeEnum)System.Enum.Parse(typeof(TowerTypeEnum), towerType); //Uses the button's string to find the corresponding enum value
        TowerChange(towerTypeEnum, true); //Now takes the enum as argument to remove the previous big "switch"

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
            Debug.Log("Game Controller - CC");
        }

        towerSpotToModify = null;
    }

	public bool CanDeconstructTower()
	{
		return currentDeconstructsAvailable > 0 ? true : false;
	}

	public void ResetDeconstructs()
	{
		currentDeconstructsAvailable = deconstructs;
		UIController.instance.UpdateDeconstructs( currentDeconstructsAvailable );
	}

	public void RemoveTower()
    {
        TowerSpot currentTowerSpot = towerSpotToModify.GetComponent<TowerSpot>();
        RefundTower(currentTowerSpot); //Because this time, we can't get the type from a button, we have to read it from the component

        //Destroy(attachedTower.gameObject); //Destroys the GameObject attached to the transform
        currentTowerSpot.RemoveTower();

		currentDeconstructsAvailable--;
		UIController.instance.UpdateDeconstructs( currentDeconstructsAvailable );

        UIController.instance.ClosePanel("Options");
        Debug.Log("remove tower gamecontroller");
    }

    public void RefundTower(TowerSpot currentTowerSpot)
    {
        string onSpot = currentTowerSpot.currentType;

        TowerTypeEnum onSpotEnum = (TowerTypeEnum)System.Enum.Parse(typeof(TowerTypeEnum), onSpot); //Finds the enum value using the given string
        TowerChange(onSpotEnum, false); //Same function, but now we're destroying

        Transform attachedTower = currentTowerSpot.transform.Find("Tower"); //Finds the child tower Transform, using the transform
        if (attachedTower == null) //Sometimes doesn't catch the tower, will remove when other problems are fixed
        {
            return;
        }

        AddEnergy(currentCost); //Refunds energy

	}

    public void ClearRubble()
    {
        TowerSpot currentTowerSpot = towerSpotToModify.GetComponent<TowerSpot>();
        currentTowerSpot.ClearRubble();
        UIController.instance.ClosePanel("Rubble");

        Debug.Log("clear rubble gamecontroller");
    }

    public void CloseCircle() //I wanted to give this to the UIController, but it isn't aware of the currently selected spot...
    {
        TowerSpot towerSpot = towerSpotToModify.GetComponent<TowerSpot>();
        if (towerSpot)
        {
            towerSpot.CloseCircle();
        }
    }

    public void LoseLife(int l = 1)
    {
        lives -= l;
        if (lives < 0)
        {
            lives = 0;
        }

        if (lives <= 0)
        {
            GameOver();
        }

        UIController.instance.UpdateLives(lives);
        #if !UNITY_EDITOR_LINUX
            AkSoundEngine.SetRTPCValue("Life", lives);
        #endif
    }

    public bool HasEnergy(int e = 1)
    {
        return e <= energy;
    }

    public void UseEnergy(int e = 1)
    {
        energy -= e;

        UIController.instance.UpdateEnergy(energy); //Updates it
    }

    public void AddEnergy(int e)
    {
        energy += e;

        UIController.instance.UpdateEnergy(energy); //Updates it
    }

}
