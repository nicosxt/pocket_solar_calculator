using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    public Transform objTypeMenu, container;
    public int selected = 0;
    public List<Button> uiSelectors = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Button bttn in uiSelectors)
        {
            bttn.onClick.AddListener(() => ClickOnButton(bttn));
        }

    }

    public void ClickOnButton(Button button)
    {
        selected = uiSelectors.IndexOf(button);
        for(int i=0; i<container.childCount; i++)
        {
            container.GetChild(i).gameObject.SetActive(i == selected);
        }
    }



}
