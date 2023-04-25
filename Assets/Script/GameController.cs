using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController s;
    void Awake() {
        if (s != null && s != this) {
            Destroy(this.gameObject);
        } else {
            s = this;
        }
    }
    [Header("Solar")]
    public float sunAmount;

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
    //current values
    public float solarCurrentA = 0f;
    public float solarCurrentP = 0f;
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
    public float batteryTotalOperatingAhrs = 0f;
    public float batteryTotalCapacity = 0f;
    //current values
    public float batteryCurrentAhrs = 0f;

    //objects
    public GameObject batteryArrayContainer;
    public Button addBatterySeriesButton, removeBatterySeriesButton, addBatteryParallelButton, removeBatteryParallelButton;
    public SetBattery defaultBattery;

    [Header("Appliances")]
    public GameObject applianceContainer;

    [Header("UI Display")]
    public TextMeshProUGUI solarTotalVText;
    public TextMeshProUGUI solarTotalAText, solarTotalKWText, solarPercentageText;
    public TextMeshProUGUI batteryTotalVText, batteryTotalAhrText, batteryTotalKWhrText, batteryPercentageText;
    public TextMeshProUGUI applianceAText, appliancekWText;

    void Start(){
        //save these for later
        //defaultSolar.OnClick();
        //defaultBattery.OnClick();
        solarOperatingA = 4.47f;
        solarOperatingV = 80.6f;
        batteryOperatingAhrs = 100f;
        batteryOperatingV = 7.2f;

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

        solarCurrentA = solarTotalA * sunAmount;
        solarCurrentP = solarTotalP * sunAmount;

        solarTotalVText.text = solarTotalV + "V";
        //change text string later
        solarTotalAText.text = solarCurrentA.ToString("0.0") + "A";
        solarTotalKWText.text = (solarCurrentP/1000f).ToString("0.0") + "kW";
        solarPercentageText.text = (sunAmount * 100f).ToString("0") + "%";

        //chargeControllerInputText.text = solarTotalV + "V\n" + solarTotalA + "A";
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
        batteryTotalOperatingAhrs = batteryOperatingAhrs * batteryParallelAmount;
        batteryTotalCapacity = batteryTotalV * batteryTotalOperatingAhrs;
        
        //ADD TIME
        //battery current ahrs = charging time + charging rate
        //batteryCurrentAhrs

        batteryTotalVText.text = batteryTotalV + "V";
        batteryTotalAhrText.text = batteryTotalOperatingAhrs.ToString("0.0") + "Ah";
        batteryTotalKWhrText.text = (batteryTotalCapacity/1000f).ToString("0.0") + "kWh";
        //batteryPercentageText.text
    }

    public void UpdateAppliances(){
        Appliance[] appliances = applianceContainer.GetComponentsInChildren<Appliance>();
        float totalA = 0f;
        float totalP = 0f;
        foreach(Appliance appliance in appliances){
            totalA += appliance.operatingA;
        }
        totalP = totalA * 120f;
        applianceAText.text = totalA.ToString("0.0") + "A";
        appliancekWText.text = (totalP/1000f).ToString("0.0") + "kW";
    }

    public void SetSunAmount(float _amount){
        sunAmount = _amount;
        UpdateSolar();
        UpdateBattery();
    }
}
