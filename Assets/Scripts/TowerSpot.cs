using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour
{
    public GameObject popupButtonCanvas;

    public bool hasTower = false;
    public string currentType;

    void OnMouseUp()
    {
        clickedSpot();
    }

    public void clickedSpot()
    {
        if (hasTower && !EventSystem.current.IsPointerOverGameObject()) //Must have have tower && not be clicking a button at the same time
        {
            UIController.instance.DisplayTowerOptions(gameObject);
        }
        else if (!hasTower && !EventSystem.current.IsPointerOverGameObject())
        {
            UIController.instance.DisplayTowerCreation(gameObject);
        }
    }
}
