using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SetBattery : MonoBehaviour
{
    public float operatingV, totalAhrs;
    TextMeshProUGUI displayText;
    Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        displayText = GetComponentInChildren<TextMeshProUGUI>();
        displayText.text = operatingV + "V\n" + totalAhrs + "Ah";
        myButton = GetComponentInChildren<Button>();
        myButton.onClick.AddListener(OnClick);
    }

    public void OnClick(){
        GameController.s.batterySingleOperatingV = operatingV;
        GameController.s.batterySingleOperatingAhrs = totalAhrs;
    }
}
