using UnityEngine;
using UnityEngine.EventSystems;

public class InvisibleBGCtrl : MonoBehaviour, IPointerClickHandler
{
    //Made by Unity user ben-rasooli

    MenuVisibilityCtrl _parentCtrl;

    public void setParentCtrl(MenuVisibilityCtrl ctrl)
    {
        _parentCtrl = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _parentCtrl.hide();
    }
}
