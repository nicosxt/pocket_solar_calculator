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
    int solarSeriesMax = 5;
    int solarParallelMax = 10;
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
    //layout
    public VerticalLayoutGroup solarLayoutVertical;
    //objects
    public GameObject solarArrayContainer;
    public Button addSolarSeriesButton, removeSolarSeriesButton, addSolarParallelButton, removeSolarParallelButton;
    public GameObject solarControl;
    public SetSolar defaultSolar;


    [Header("Battery")]
    public int batterySerieisAmount = 0;
    public int batteryParallelAmount = 0;
    int batterySeriesMax = 5;
    int batteryParallelMax = 10;
    
    //each obj
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
    //layout
    public VerticalLayoutGroup batteryLayoutVertical;
    //objects
    public GameObject batteryArrayContainer;
    public Button addBatterySeriesButton, removeBatterySeriesButton, addBatteryParallelButton, removeBatteryParallelButton;
    public GameObject batteryControl;
    public SetBattery defaultBattery;

    [Header("Appliances")]
    public GameObject applianceContainer;
    public Button applianceToggleButton;
    public float applianceCurrentA = 0f;
    public float applianceCurrentP = 0f;

    [Header("Time")]
    float chargingRate = 0.33f;
    public bool isSimulating, isFinishedSetup;
    //configuring, simulating
    public string currentState;
    public bool isPlaying;

    [Header("UI Display")]
    public TextMeshProUGUI solarTotalVText;
    public TextMeshProUGUI solarTotalAText, solarPowerText;
    public TextMeshProUGUI batteryTotalVText, batteryAhrText, batteryDeltaPowerText;
    public TextMeshProUGUI applianceAText, appliancePowerText;
    public Transform batteryFill;
    public TextMeshProUGUI batteryChargeHint, simulationIndicator;

    void Start(){
        SetCurrentState("configuring");
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

        //scale UI
        solarLayoutVertical.spacing = GetVerticalSpacing(solarParallelAmount);

        //calculate values
        solarTotalV = solarOperatingV * solarSerieisAmount;
        solarTotalA = solarOperatingA * solarParallelAmount;
        solarTotalP = solarTotalV * solarTotalA;

        solarCurrentA = solarTotalA * sunAmount;
        solarCurrentP = solarTotalP * sunAmount;

        solarTotalVText.text = solarTotalV + "V";
        solarTotalAText.text = solarCurrentA.ToString("0.0") + "A";

        if(Mathf.Abs(solarCurrentP) < 1000f){
            solarPowerText.text = "In:" + solarCurrentP.ToString("0") + "W";
        }else{
            solarPowerText.text = "In:" + (solarCurrentP/1000f).ToString("0.0") + "kW";
        }

        //chargeControllerInputText.text = solarTotalV + "V\n" + solarTotalA + "A";
        //solarTotalKWText.text = (solarCurrentP/1000f).ToString("0.0") + "kW";
        //solarPercentageText.text = (sunAmount * 100f).ToString("0") + "%";
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
        //scale UI
        batteryLayoutVertical.spacing = GetVerticalSpacing(batteryParallelAmount);

        batteryTotalV = batterySingleOperatingV * batterySerieisAmount;
        batteryTotalOperatingAhrs = batterySingleOperatingAhrs * batteryParallelAmount;
        batteryTotalCapacity = batteryTotalV * batteryTotalOperatingAhrs;
        
        batteryTotalVText.text = batteryTotalV + "V";
        
        //battery text is updated in UpdateBatteryAhrText()
        
    }

    public void UpdateBatteryAhrText(){
        batteryAhrText.text = batteryCurrentAhrs.ToString("0.0") + "/" + batteryTotalOperatingAhrs.ToString("0.0") + "Ah";
    }

    public void UpdateAppliances(){
        Appliance[] appliances = applianceContainer.GetComponentsInChildren<Appliance>();
        float totalA = 0f;
        float totalP = 0f;
        foreach(Appliance appliance in appliances){
            if(appliance.isOn){
                totalA += appliance.operatingA;
            }
        }
        totalP = totalA * 120f;
        applianceCurrentA = totalA;
        applianceCurrentP = totalP;

        applianceAText.text = totalA.ToString("0.0") + "A";
        if(Mathf.Abs(totalP) < 1000f){
            appliancePowerText.text = "Out:" + totalP.ToString("0") + "W";
        }else{
            appliancePowerText.text = "Out:" + (totalP/1000f).ToString("0.0") + "kW";
        }
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

    public void UpdateBatteryFill(){
        //update battery fill
        if(batteryTotalOperatingAhrs > 0f){
            batteryFill.localScale = new Vector3(batteryCurrentAhrs/batteryTotalOperatingAhrs, 1f, 1f);
        }
        else{
            batteryFill.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    void Update(){

        batteryDeltaPower = solarCurrentP - applianceCurrentP;
        if(Mathf.Abs(batteryDeltaPower) < 1000f){
            batteryDeltaPowerText.text = "In:" + batteryDeltaPower.ToString("0") + "W";
        }else{
            batteryDeltaPowerText.text = "In:" + (batteryDeltaPower/1000f).ToString("0.0") + "kW";
        }

        //update battery current ahrs
        if(isPlaying && currentState == "simulating"){
            float batteryCurrentAhrsLocal = batteryCurrentAhrs;
            batteryCurrentAhrsLocal += chargingRate * Time.deltaTime * batteryDeltaPower/batteryTotalV;
            batteryCurrentAhrs = Mathf.Clamp(batteryCurrentAhrsLocal, 0f, batteryTotalOperatingAhrs);
            UpdateBatteryAhrText();
        }

        //calculate amount of hours needed for batteryCurrentAhrs to reach batteryTotalOperatingAhrs
        if(batteryDeltaPower > 0){
            //is charging
            float batteryAhrsLeft = batteryTotalOperatingAhrs - batteryCurrentAhrs ;
            float batteryDeltaAhrs = batteryDeltaPower/batteryTotalV;
            float batteryChargingHrs = batteryAhrsLeft / batteryDeltaAhrs;
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

        UpdateBatteryFill();
    }

    public void SetCurrentState(string _state){
        string lastState = currentState;
        if(_state == "configuring"){
            //pause simulation
            isPlaying = false;

            //set battery to 0
            batteryCurrentAhrs = 0;
            UpdateBatteryAhrText();

            //update controls
            SetControls(true);

            //update text
            simulationIndicator.text = "configuring...";
        }else if(_state == "simulating"){
            SetControls(false);

            //if just switched out of configuring state
            if(lastState == "configuring"){
                isPlaying = true;
                simulationIndicator.text = "simulating...";
            }
        }
        currentState = _state;
    }

    public void ToggleGameState(){
        if(currentState == "configuring"){
            SetCurrentState("simulating");
        }else if(currentState == "simulating"){
            SetCurrentState("configuring");
        }
    }

    public void TogglePlay(){

        if(currentState != "simulating"){
            return;
        }

        isPlaying = !isPlaying;
        if(isPlaying)
            simulationIndicator.text = "simulating...";
            else
            simulationIndicator.text = "paused.";
        
    }

    public void SetControls(bool _on){
        batteryControl.SetActive(_on);
        solarControl.SetActive(_on);
        applianceToggleButton.enabled = _on;

        //when it's time to configure
        if(_on){
            simulationIndicator.text = "configuring...";
            batteryAhrText.text = batteryCurrentAhrs.ToString("0.0") + "/" + batteryTotalOperatingAhrs.ToString("0.0") + "Ah";
        }
    }

    float GetVerticalSpacing(int objAmount){
        if(objAmount < 4)
            return 0;

        switch(objAmount){
            case 4:
                return -35.2f;
            case 5:
                return -51.1f;
            case 6:
                return -60.8f;
            case 7:
                return -67.7f;
            case 8:
                return -71.7f;
            case 9:
                return -75f;
            case 10:
                return -78f;
                default:
                return 0;
        }
    }

}
