using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour
{
    public GameObject popupButtonCanvas;

    public bool hasTower = false;
    public bool towerChecked = false; //This is to tell the spot to look for the tower that was built on it
    public Tower childTower;
    private LaserBuilderEffect laserEmitter;
    public string currentType;

    private void Start() {
        laserEmitter = GetComponent<LaserBuilderEffect>();
    }

    private void LateUpdate() //Late because we want to do it after construction/destruction/upgrade
    {
        if (towerChecked != hasTower)
        {
            childTower = GetComponentInChildren<Tower>(); //Will return to null when destroying
            towerChecked = hasTower; //So that we don't get the component every frame
        }
    }

    void OnMouseUp()
    {
        ClickedSpot();
    }

    public void ClickedSpot()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) //Ignores click if user also clicked a button at the same time
        {
            if (childTower != null)
            {
                childTower.Selected();
                // childTower.line.enabled = true; //Activates the range circle
                // childTower.selectionCirclePrefab.SetActive(true);
            }
            if (hasTower)
            {
                UIController.instance.DisplayTowerOptions(gameObject);
            }
            else if (!hasTower)
            {
                laserEmitter.takeoff();
                UIController.instance.DisplayTowerCreation(gameObject);
            }
        }
    }

    public void CloseCircle()
    {
        if (childTower != null)
        {
            childTower.Deselected();
            // childTower.line.enabled = false; //Deactivates the range circle
            // childTower.selectionCirclePrefab.SetActive(false);
        }
    }

    public void DestroyTower() {
        hasTower = false;
        childTower = null; // Reference to tower must be nulled manually for GC
    }

    public void LandEmitter() {
        if (!hasTower) {
            laserEmitter.land();
        }
    }
}
