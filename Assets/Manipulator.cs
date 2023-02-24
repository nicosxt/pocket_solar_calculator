using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manipulator : MonoBehaviour
{
    public static Manipulator s;
    void Awake() {
        if (s != null && s != this) {
            Destroy(this.gameObject);
        } else {
            s = this;
        }
    }

    public GameObject dropDownMenu;

    public Button[] expandableScreensButton;
    float expandScreenHeight = 100f;
    List<ExpandableScreen> expandableScreens = new List<ExpandableScreen>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Button button in expandableScreensButton) {
            button.onClick.AddListener(() => ToggleExpandScreen(button));
            expandableScreens.Add(new ExpandableScreen(button.GetComponent<RectTransform>()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDropDownMenu(GameObject _item) {
        _item.SetActive(!dropDownMenu.activeSelf);
    }

    //toggle expandableScreen element to expand or collapse
    public void ToggleExpandScreen(Button _bttn){
        return;

        foreach(ExpandableScreen e in expandableScreens){
            if(e.bttn == _bttn){
                e.ToggleExpand(expandScreenHeight);
            }else{
                e.Collapse();
            }
        }
    }
}

//create a custom class ExpandableScreen, document its original height of RectTransform, and the original position of RectTransform
public class ExpandableScreen {
    public RectTransform rectTransform;
    public float originalHeight;
    public Vector3 originalPosition;
    public Button bttn;
    public bool isExpanded;
    //constructor
    public ExpandableScreen(RectTransform _rectTransform) {
        rectTransform = _rectTransform;
        originalHeight = rectTransform.sizeDelta.y;
        originalPosition = rectTransform.position;
        bttn = _rectTransform.GetComponent<Button>();
        isExpanded = false;
    }

    public void ToggleExpand(float _h) {
        float newHeight = isExpanded ? originalHeight : _h;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
        isExpanded = !isExpanded;
    }

    // public void Expand(float _h) {
    //     rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _h);
    //     isExpanded = true;
    // }

    public void Collapse() {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, originalHeight);
        isExpanded = false;
    }
}