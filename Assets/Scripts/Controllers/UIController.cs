using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController instance;

    public GameObject pausePanel;
    public GameObject towerPanel;
    public Transform panelPosition;
    public MenuVisibilityCtrl menuVisibilityCtrl;

    public int lives = 20;
    public int energy = 20;
    public Text livesText;
    public Text energyText;

    // Use this for initialization
    private void Awake()
    {
        instance = this;
        menuVisibilityCtrl = towerPanel.GetComponent<MenuVisibilityCtrl>();
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (towerPanel.activeSelf)
        {
            towerPanel.transform.position = Camera.main.WorldToScreenPoint(panelPosition.position); //The transform is moving around before clicking a button and its annoying! But it doesn't bug...
        }
    }

    //This section is the old ScoreManager script (except for the Game Over, now in GameController
    public void LoseLife(int l = 1)
    {
        lives -= l;
        if (lives <= 0)
        {
            GameOver();
        }

        livesText.text = "Lives: " + lives.ToString();
    }

    public bool HasEnergy(int e = 1)
    {
        return e <= energy;
    }

    public void UseEnergy(int e = 1)
    {
        energy -= e;

        energyText.text = "Energy: " + energy.ToString();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        // TODO: Send the player to a game-over screen instead!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayTowerSelectionPopup(GameObject towerSpot)
    {
        if (GameController.instance.isPaused == false && towerPanel.activeSelf == false) //Added a check to make it so we don't switch the panel when we click a button, however that means that we need to deactivate before changing panels
        {
            towerPanel.SetActive(true);
            panelPosition = towerSpot.transform;
            towerPanel.transform.position = Camera.main.WorldToScreenPoint(panelPosition.position);
            GameController.instance.towerSpotToModify = towerSpot;
        }
    }
}
