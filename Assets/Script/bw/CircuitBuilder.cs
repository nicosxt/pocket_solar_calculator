using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CircuitBuilder : MonoBehaviour
{
    public static CircuitBuilder s;
    void Awake() {
        if (s != null && s != this) {
            Destroy(this.gameObject);
        } else {
            s = this;
        }
    }

    [Header("Solar")]
    public int solarSerieisAmount = 0;
    public int solarParallelAmount = 0;
    public int solarSeriesMax = 5;
    public int solarParallelMax = 5;
    //each panel
    public float solarOperatingV = 12f;
    public float solarOperatingA = 0.5f;
    //total setup
    public float solarTotalV = 0f;
    public float solarTotalA = 0f;
    public float solarTotalP = 0f;
    //objects
    public GameObject solarArrayContainer;
    public Button addSolarSeriesButton, removeSolarSeriesButton, addSolarParallelButton, removeSolarParallelButton;
    public SetSolar defaultSolar;


    [Header("Battery")]
    public int batterySerieisAmount = 0;
    public int batteryParallelAmount = 0;
    public int batterySeriesMax = 5;
    public int batteryParallelMax = 5;
    //each panel
    public float batteryOperatingV = 12f;
    public float batteryOperatingAhrs = 0.5f;
    //total setup
    public float batteryTotalV = 0f;
    public float batteryTotalAhrs = 0f;
    public float batteryCurrentAhrs = 0f;
    public float batteryTotalCapacity = 0f;
    //objects
    public GameObject batteryArrayContainer;
    public Button addBatterySeriesButton, removeBatterySeriesButton, addBatteryParallelButton, removeBatteryParallelButton;
    public SetBattery defaultBattery;

    [Header("Charge Controller")]
    public TextMeshProUGUI chargeControllerInputText;
    public TextMeshProUGUI chargeControllerOutputText;

    void Start(){
        defaultSolar.OnClick();
        defaultBattery.OnClick();

        solarParallelAmount = 1;
        batteryParallelAmount = 1;

        addSolarSeriesButton.onClick.AddListener(AddSolarSeries);
        removeSolarSeriesButton.onClick.AddListener(RemoveSolarSeries);
        addSolarParallelButton.onClick.AddListener(AddSolarParallel);
        removeSolarParallelButton.onClick.AddListener(RemoveSolarParallel);

        addBatterySeriesButton.onClick.AddListener(AddBatterySeries);
        removeBatterySeriesButton.onClick.AddListener(RemoveBatterySeries);
        addBatteryParallelButton.onClick.AddListener(AddBatteryParallel);
        removeBatteryParallelButton.onClick.AddListener(RemoveBatteryParallel);

        UpdateSolar();
        UpdateBattery();
    }

    //add / remove solar series / parallels
    public void AddSolarSeries(){
        if(solarSerieisAmount < solarSeriesMax){
            solarSerieisAmount ++;
        }
        UpdateSolar();
    }
    public void RemoveSolarSeries(){
        if(solarSerieisAmount > 0){
            solarSerieisAmount --;
        }
        UpdateSolar();
    }
    public void AddSolarParallel(){
        if(solarParallelAmount < solarParallelMax){
            solarParallelAmount ++;
        }
        UpdateSolar();
    }
    public void RemoveSolarParallel(){
        if(solarParallelAmount > 1){
            solarParallelAmount --;
        }
        UpdateSolar();
    }

    public void UpdateSolar(){
        //upate display
        for(int i=0; i<solarArrayContainer.transform.childCount; i++){
            solarArrayContainer.transform.GetChild(i).gameObject.SetActive(i<solarParallelAmount);
            for(int j=0; j<solarArrayContainer.transform.GetChild(i).childCount; j++){
                solarArrayContainer.transform.GetChild(i).GetChild(j).gameObject.SetActive(j<solarSerieisAmount);
            }
        }

        solarTotalV = solarOperatingV * solarSerieisAmount;
        solarTotalA = solarOperatingA * solarParallelAmount;
        solarTotalP = solarTotalV * solarTotalA;
        chargeControllerInputText.text = solarTotalV + "V\n" + solarTotalA + "A";
    }

    //add / remove battery series / parallels
    public void AddBatterySeries(){
        if(batterySerieisAmount < batterySeriesMax){
            batterySerieisAmount ++;
        }
        UpdateBattery();
    }
    public void RemoveBatterySeries(){
        if(batterySerieisAmount > 0){
            batterySerieisAmount --;
        }
        UpdateBattery();
    }
    public void AddBatteryParallel(){
        if(batteryParallelAmount < batteryParallelMax){
            batteryParallelAmount ++;
        }
        UpdateBattery();
    }
    public void RemoveBatteryParallel(){
        if(batteryParallelAmount > 1){
            batteryParallelAmount --;
        }
        UpdateBattery();
    }

    public void UpdateBattery(){
        //upate display
        for(int i=0; i<batteryArrayContainer.transform.childCount; i++){
            batteryArrayContainer.transform.GetChild(i).gameObject.SetActive(i<batteryParallelAmount);
            for(int j=0; j<batteryArrayContainer.transform.GetChild(i).childCount; j++){
                batteryArrayContainer.transform.GetChild(i).GetChild(j).gameObject.SetActive(j<batterySerieisAmount);
            }
        }

        batteryTotalV = batteryOperatingV * batterySerieisAmount;
        batteryTotalAhrs = batteryOperatingAhrs * batteryParallelAmount;
        batteryTotalCapacity = batteryTotalV * batteryTotalAhrs;
        chargeControllerOutputText.text = batteryTotalV + "V\n" + batteryTotalAhrs + "Ah";
    }
}
