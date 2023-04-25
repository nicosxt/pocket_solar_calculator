using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetSolar : MonoBehaviour
{
    public float operatingV, operatingA;
    TextMeshProUGUI displayText;
    Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        displayText = GetComponentInChildren<TextMeshProUGUI>();
        displayText.text = operatingV + "V\n    " + operatingA + "A";
        myButton = GetComponentInChildren<Button>();
        myButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        GameController.s.solarOperatingV = operatingV;
        GameController.s.solarOperatingA = operatingA;
    }
}
