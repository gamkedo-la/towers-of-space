using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public GameObject pausePanel;
    public GameObject creationPanel;
    public GameObject optionsPanel;
    public GameObject rubblePanel;
    public GameObject gameOverPanel;
    public GameObject gameWonPanel;

    public Transform creationPanelPosition;
    public Transform optionsPanelPosition;
    public Transform rubblePanelPosition;

    public MenuVisibilityCtrl menuVisibilityCtrl;

    public Text livesText;
    public Text energyText;
	public Text deconstructsText;
	public Image spawnBar;
	public List<List<EnemySpawner.WaveComponent>> nextWaves = new List<List<EnemySpawner.WaveComponent>>();

    // Use this for initialization
    private void Awake()
    {
        instance = this;
        menuVisibilityCtrl = creationPanel.GetComponent<MenuVisibilityCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (creationPanel.activeSelf)
        {
            creationPanel.transform.position = Camera.main.WorldToScreenPoint(creationPanelPosition.position); //The transform is moving around before clicking a button and its annoying! But it doesn't bug...
        }
        if (optionsPanel.activeSelf)
        {
            optionsPanel.transform.position = Camera.main.WorldToScreenPoint(optionsPanelPosition.position); //The transform is moving around before clicking a button and its annoying! But it doesn't bug...
        }
        if (rubblePanel.activeSelf)
        {
            rubblePanel.transform.position = Camera.main.WorldToScreenPoint(rubblePanelPosition.position); //The transform is moving around before clicking a button and its annoying! But it doesn't bug...
        }
    }


    public void DisplayTowerCreation(GameObject towerSpot)
    {
        if (GameController.instance.isPaused == false && creationPanel.activeSelf == false) //Added a check to make it so we don't switch the panel when we click a button, however that means that we need to deactivate before changing panels
        {
            creationPanel.SetActive(true);
            creationPanelPosition = towerSpot.transform;
            creationPanel.transform.position = Camera.main.WorldToScreenPoint(creationPanelPosition.position);
            GameController.instance.towerSpotToModify = towerSpot;
        }
    }

    public void DisplayTowerOptions(GameObject towerSpot)
    {
        if (GameController.instance.isPaused == false && optionsPanel.activeSelf == false) //Added a check to make it so we don't switch the panel when we click a button, however that means that we need to deactivate before changing panels
        {
            optionsPanel.SetActive(true);
            optionsPanelPosition = towerSpot.transform;
            optionsPanel.transform.position = Camera.main.WorldToScreenPoint(optionsPanelPosition.position);
            GameController.instance.towerSpotToModify = towerSpot;
			optionsPanel.GetComponent<CanDeconstruct>( ).TryCanDeconstruct( );
        }
    }

    public void DisplayRubbleOptions(GameObject towerSpot)
    {
        if (GameController.instance.isPaused == false && rubblePanel.activeSelf == false) //Added a check to make it so we don't switch the panel when we click a button, however that means that we need to deactivate before changing panels
        {
            rubblePanel.SetActive(true);
            rubblePanelPosition = towerSpot.transform;
            rubblePanel.transform.position = Camera.main.WorldToScreenPoint(rubblePanelPosition.position);
            GameController.instance.towerSpotToModify = towerSpot;
        }
    }

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + GameController.instance.lives.ToString();
    }

    public void UpdateEnergy(int energy)
    {
        energyText.text = "Energy: " + GameController.instance.energy.ToString();
    }

	public void UpdateDeconstructs( int deconstructs )
	{
		deconstructsText.text = "Tower Deconstructs Available: " + deconstructs.ToString( );
	}

	public void UpdateSpawnBar(float progress){
		progress = Mathf.Clamp (progress, 0f, 1f);
		spawnBar.transform.localScale = new Vector3(progress, 1f, 1f) ;
	}

    public void ClosePanel(string panelToClose)
    {
        switch (panelToClose)
        {
            case "Creation":
                creationPanel.GetComponent<MenuVisibilityCtrl>().hide();
                creationPanel.SetActive(false); //Hides the panel
                break;
            case "Options":
                optionsPanel.GetComponent<MenuVisibilityCtrl>().hide();
                optionsPanel.SetActive(false); //Hides the panel
                break;
            case "Rubble":
                rubblePanel.GetComponent<MenuVisibilityCtrl>().hide();
                rubblePanel.SetActive(false); //Hides the panel
                break;
        }
    }
}
