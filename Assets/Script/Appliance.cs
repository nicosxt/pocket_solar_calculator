using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Appliance : MonoBehaviour
{
    public float operatingA;
    public bool isOn;
    public GameObject isOnIndicator;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnClick);
        isOnIndicator.GetComponentInChildren<TextMeshProUGUI>().text = operatingA.ToString("0.0") + "A";

        isOn = false;
        isOnIndicator.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        isOn = !isOn;
        isOnIndicator.SetActive(isOn);
    }




}
