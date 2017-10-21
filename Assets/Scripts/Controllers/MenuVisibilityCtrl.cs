using UnityEngine;
using UnityEngine.UI;

public class MenuVisibilityCtrl : MonoBehaviour
{
    //Made by Unity user ben-rasooli

    [SerializeField]
    bool shouldStartVisible;
    [SerializeField]
    bool onTowerSpot;
    bool awake = false;

    GameObject _invisibleBG;

    void Awake()
    {

        setupInvisibleBG();
        if (!shouldStartVisible)
        {
            hide();
            awake = true; //To avoid a problem with trying to access the GameController before starting the game
        }
    }

    void setupInvisibleBG()
    {
        _invisibleBG = new GameObject("InvisibleBG");

        InvisibleBGCtrl tempInvisibleBGCtrl = _invisibleBG.AddComponent<InvisibleBGCtrl>();
        tempInvisibleBGCtrl.setParentCtrl(this);

        Image tempImage = _invisibleBG.AddComponent<Image>();
        tempImage.color = new Color(1f, 1f, 1f, 0f);

        RectTransform tempTransform = _invisibleBG.GetComponent<RectTransform>();
        tempTransform.anchorMin = new Vector2(0f, 0f);
        tempTransform.anchorMax = new Vector2(1f, 1f);
        tempTransform.offsetMin = new Vector2(0f, 0f);
        tempTransform.offsetMax = new Vector2(0f, 0f);
        tempTransform.SetParent(GetComponentsInParent<Transform>()[1], false);
        tempTransform.SetSiblingIndex(transform.GetSiblingIndex()); // put it right beind this panel in the hierarchy
    }

    void OnEnable()
    {
        _invisibleBG.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
        _invisibleBG.SetActive(false);
        if (onTowerSpot && awake)
        {
            GameController.instance.CloseCircle();
        }
    }
}
