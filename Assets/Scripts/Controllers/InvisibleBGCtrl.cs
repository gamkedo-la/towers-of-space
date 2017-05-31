using UnityEngine;
using UnityEngine.EventSystems;

public class InvisibleBGCtrl : MonoBehaviour, IPointerClickHandler
{
    //Made by Unity user ben-rasooli

    MenuVisibilityCtrl _parentCtrl;
    EventTrigger eventTrigger;

    public void setParentCtrl(MenuVisibilityCtrl ctrl)
    {
        _parentCtrl = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Land the laser projector when the menu closes
        GameController.instance.towerSpotToModify.GetComponent<TowerSpot>().LandEmitter();

        //Hide the menu
        _parentCtrl.hide();
    }
}
