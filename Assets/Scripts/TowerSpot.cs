using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour
{
    public GameObject popupButtonCanvas;

    private bool hasRubble = false;
    public bool hasTower = false;
    public bool towerChecked = false; //This is to tell the spot to look for the tower that was built on it
    public Tower childTower;
    private LaserBuilderEffect laserEmitter;
    public string currentType;

    public float autoClearTime = 5f;
    private IEnumerator autoClearCoroutine;

    private void Start() {
        laserEmitter = GetComponent<LaserBuilderEffect>();
    }

    private void LateUpdate() //Late because we want to do it after construction/destruction/upgrade
    {
        if (towerChecked != (hasTower || hasRubble))
        {
            childTower = GetComponentInChildren<Tower>(); //Will return to null when destroying
            towerChecked = (hasTower || hasRubble); //So that we don't get the component every frame
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
            }
            if (hasTower)
            {
                UIController.instance.DisplayTowerOptions(gameObject);
            }
            else if (hasRubble) {
                UIController.instance.DisplayRubbleOptions(gameObject);
            }
            else if (!hasTower && !hasRubble)
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
        }
    }
    private IEnumerator AutoClearRubble()
    {
        yield return new WaitForSeconds(autoClearTime);
        RemoveTower();
    }

    public void DestroyTower() {
        GameController.instance.RefundTower(this);
        hasTower = false;
        hasRubble = true;

        autoClearCoroutine = AutoClearRubble();
        StartCoroutine(autoClearCoroutine);
    }

    public void RemoveTower() {
        hasTower = false;
        hasRubble = false;
        Destroy(childTower.gameObject);
        childTower = null; // Reference to tower must be nulled manually for GC
        laserEmitter.reset();
        if (autoClearCoroutine != null)
        {
            StopCoroutine(autoClearCoroutine);
            autoClearCoroutine = null;
        }
    }

    public void ClearRubble() {
        RemoveTower();
    }

    public void LandEmitter() {
        if (!hasTower && !hasRubble) {
            laserEmitter.land();
        }
    }
}
