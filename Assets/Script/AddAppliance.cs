using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddAppliance : MonoBehaviour
{
    public GameObject appliancePrefab;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        GameObject newAppliance = Instantiate(appliancePrefab, GameController.s.applianceContainer.transform);
        GameController.s.UpdateAppliances();

        //reset sibling index of the add button for appliances
        GameController.s.applianceAddButton.transform.SetSiblingIndex(GameController.s.applianceContainer.transform.childCount - 1);
    }
}
