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
    public string currentType;

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
                childTower.line.enabled = true; //Activates the range circle
            }
            if (hasTower)
            {
                UIController.instance.DisplayTowerOptions(gameObject);
            }
            else if (!hasTower)
            {
                UIController.instance.DisplayTowerCreation(gameObject);
            }
        }
    }
}
