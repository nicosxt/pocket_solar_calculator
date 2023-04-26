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
    public float batterySingleOperatingV = 12f;
    public float batterySingleOperatingAhrs = 0.5f;
    //total setup
    public float batteryTotalV = 0f;
    public float batteryTotalOperatingAhrs = 0f;
    public float batteryTotalCapacity = 0f;
    //current values
    public float batteryCurrentAhrs = 0f;
    public float batteryCurrentWhrs = 0f;
    public float batteryDeltaPower = 0f;

    //objects
    public GameObject batteryArrayContainer;
    public Button addBatterySeriesButton, removeBatterySeriesButton, addBatteryParallelButton, removeBatteryParallelButton;
    public SetBattery defaultBattery;

    [Header("Appliances")]
    public GameObject applianceContainer;
    public float applianceCurrentA = 0f;
    public float applianceCurrentP = 0f;

    [Header("Time")]
    float chargingRate = 0.33f;
    public bool isSimulating;

    [Header("UI Display")]
    public TextMeshProUGUI solarTotalVText;
    public TextMeshProUGUI solarTotalAText, solarTotalKWText, solarPercentageText;
    public TextMeshProUGUI batteryTotalVText, batteryTotalAhrText, batteryTotalKWhrText, batteryPercentageText, batteryDeltaPowerText;
    public TextMeshProUGUI applianceAText, appliancekWText;
    public Transform batteryFill;
    public TextMeshProUGUI batteryChargeHint;

    void Start(){
        //save these for later
        //defaultSolar.OnClick();
        //defaultBattery.OnClick();
        solarOperatingA = 4.47f;
        solarOperatingV = 80.6f;
        batterySingleOperatingAhrs = 100f;
        batterySingleOperatingV = 7.2f;

        solarParallelAmount = 1;
        solarSerieisAmount = 3;
        batteryParallelAmount = 1;
        batterySerieisAmount = 3;

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

        batteryTotalV = batterySingleOperatingV * batterySerieisAmount;
        batteryTotalOperatingAhrs = batterySingleOperatingAhrs * batteryParallelAmount;
        batteryTotalCapacity = batteryTotalV * batteryTotalOperatingAhrs;
        
        batteryTotalVText.text = batteryTotalV + "V";
        
        //batteryTotalKWhrText.text = (batteryTotalCapacity/1000f).ToString("0.0") + "kWh";
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
        applianceCurrentA = totalA;
        applianceCurrentP = totalP;
        applianceAText.text = totalA.ToString("0.0") + "A";
        appliancekWText.text = (totalP/1000f).ToString("0.0") + "kW";
    }

    public void RemoveAppliance(GameObject _appliance){
        Destroy(_appliance);
        UpdateAppliances();
    }

    public void SetSunAmount(float _amount){
        sunAmount = _amount;
        UpdateSolar();
        UpdateBattery();
    }

    void Update(){

        batteryDeltaPower = solarCurrentP - applianceCurrentP;
        if(Mathf.Abs(batteryDeltaPower) < 1000f){
            batteryDeltaPowerText.text = "In:" + batteryDeltaPower.ToString("0") + "W";
        }else{
            batteryDeltaPowerText.text = "In:" + (batteryDeltaPower/1000f).ToString("0.0") + "kW";
        }

        //update battery current ahrs
        if(isSimulating){

            float batteryCurrentAhrsLocal = batteryCurrentAhrs;
            batteryCurrentAhrsLocal += chargingRate * Time.deltaTime * batteryDeltaPower/batteryTotalV;
            batteryCurrentAhrs = Mathf.Clamp(batteryCurrentAhrsLocal, 0f, batteryTotalOperatingAhrs);

            batteryTotalAhrText.text = batteryCurrentAhrs.ToString("0.0") + "/" + batteryTotalOperatingAhrs.ToString("0.0") + "Ah";
        }

        //calculate amount of hours needed for batteryCurrentAhrs to reach batteryTotalOperatingAhrs
        if(batteryDeltaPower > 0){
            //is charging
            float batteryAhrsLeft = batteryTotalOperatingAhrs - batteryCurrentAhrs ;
            float batteryDeltaAhrs = batteryDeltaPower/batteryTotalV;
            float batteryChargingHrs = batteryAhrsLeft / batteryDeltaAhrs;
            Debug.Log("battery ahrs left" + batteryAhrsLeft);
            Debug.Log("battery delta ahrs" + batteryDeltaAhrs);
            Debug.Log("battery charging hrs" + batteryChargingHrs);
            if(batteryChargingHrs == 0){
                batteryChargeHint.text = "full battery";
            }else{
                batteryChargeHint.text = batteryChargingHrs.ToString("0.0") + "hrs till full";
            }

        }else if(batteryDeltaPower < 0){
            //is dischraging
            float batteryDeltaAhrs = -1 * batteryDeltaPower/batteryTotalV;
            float batteryDischargingHrs = batteryCurrentAhrs / batteryDeltaAhrs;
            if(batteryDischargingHrs == 0){
                batteryChargeHint.text = "empty battery";
            }else{
                batteryChargeHint.text = batteryDischargingHrs.ToString("0.0") + "hrs till empty";
            }
            

        }else if(batteryDeltaPower == 0){
            batteryChargeHint.text = "batteries at rest";    
        }

        //update battery fill
        if(batteryTotalOperatingAhrs > 0f){
            batteryFill.localScale = new Vector3(batteryCurrentAhrs/batteryTotalOperatingAhrs, 1f, 1f);
        }
        else{
            batteryFill.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    public void ToggleSimulation(){
        isSimulating = !isSimulating;
    }
}
