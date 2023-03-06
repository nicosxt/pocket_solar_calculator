using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CircuitController : MonoBehaviour
{
    public static CircuitController s;
    void Awake() {
        if (s != null && s != this) {
            Destroy(this.gameObject);
        } else {
            s = this;
        }
    }


    [Header("__Summary__")]
    public TextMeshProUGUI totalInputDisplayText;
    public GameObject rowPrefab;
    public bool solarArrayExpanded;
    public bool batteryArrayExpanded;

    [Header("__Generators__")]
    public GameObject generatorContainer;
    public GameObject generatorPrefab;
    public TextMeshProUGUI generatorDisplayText;
    public float inputPower = 0;
    float inputPowerKW = 0;
    public float inputVolts = 0;
    public float inputVoltsMax = 0;
    public float inputAmps = 0;
    float generatorEnergyLoss = 0.75f;
    int generatorParallelAmount = 0;
    int generatorSeriesAmount = 0;
    float uiHeight = 32f;
    float uiWidth = 450f;
    
    float solarPanelDefaultAmps = 8;
    float solarPanelDefaultVolts = 30;

    [Header("__ChargeController__")]
    public TextMeshProUGUI chargeControllerBatteryVoltsText;
    public TextMeshProUGUI chargeControllerInputPowerText, chargeControllerOutputPowerText;
    public float totalPowerWithdraw = 0f;
    float totalPowerWithdrawKW = 0f;

    [Header("__Inverter__")]
    public TextMeshProUGUI inverterAmpsWithdrawText;


    [Header("__Batteries__")]
    public GameObject batteryContainer;
    public GameObject batteryPrefab;
    public TextMeshProUGUI batteryDisplayText;
    public int batteryParallelAmount, batterySeriesAmount;
    public float batteryChargingAmps, batteryDischargingAmps;
    public float batteryVolts, batteryTotalAmpHours;
    public float batteryCurrentAmpHours;
    public float batteryChargedPercentage;
    public TextMeshProUGUI batteryPercentageText;
    public Image batteryPercentageView;

    float singleBatteryVolts = 14.4f;
    float singlebatteryTotalAmpHours = 200f;

    [Header("__Appliances__")]
    public GameObject applianceContainer;
    public float totalAmpsWithdraw = 0f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        generatorParallelAmount = 1;
        generatorSeriesAmount = 5;
        batteryParallelAmount = 1;
        batterySeriesAmount = 5;
        SetupBattery();
        SetupSolar();
        UpdateInverterText(totalAmpsWithdraw);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSolar();
        UpdateBatteries();
    }

    public void SetupSolar(){
        foreach(Transform child in generatorContainer.transform){
            Destroy(child.gameObject);
        }

        //update visuals
        for(int i = 0; i < generatorParallelAmount; i++){
            GameObject newGeneratorRow = Instantiate(rowPrefab, generatorContainer.transform);
            //keep all y position between 0 and 30
            float y = (generatorParallelAmount <= 1) ? 0 : -i * uiHeight / (generatorParallelAmount - 1);
            newGeneratorRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            //Debug.Log("generator Amount " + generatorParallelAmount + " y " + y);
            for(int j=0; j < generatorSeriesAmount; j ++){
                GameObject newGenerator = Instantiate(generatorPrefab, newGeneratorRow.transform);
                //keep all the x position within -300 and 300
                float x = (generatorSeriesAmount <= 1) ? 0 : -uiWidth*0.5f + j * uiWidth / (generatorSeriesAmount - 1);
                newGenerator.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
        }

        inputVoltsMax = generatorSeriesAmount * solarPanelDefaultVolts;
    }

    public void UpdateSolar(){
        //calculate input power
        inputVolts = generatorSeriesAmount * solarPanelDefaultVolts * WorldController.s.sunAmount;
        
        inputAmps = generatorParallelAmount * solarPanelDefaultAmps * WorldController.s.sunAmount;
        inputPower = inputVolts * inputAmps * generatorEnergyLoss;
        inputPowerKW = inputPower / 1000;

        //update text
        generatorDisplayText.text = inputVolts + "V | " + inputAmps + "A | " + inputPowerKW + "kW";
        totalInputDisplayText.text = inputPowerKW + " kW";
        //chargeControllerInputText.text = inputVolts + " V\n" + inputAmps + " A";
    }

    //toggle second to last array of objects
    public void ToggleSolarArrays(){
        solarArrayExpanded = !solarArrayExpanded;
        for(int i=1; i<generatorParallelAmount; i++){
            Debug.Log("toggle" + i);
            Debug.Log(generatorContainer.transform.GetChild(i).gameObject.name);
            generatorContainer.transform.GetChild(i).gameObject.SetActive(solarArrayExpanded);

            Debug.Log("set active as " + solarArrayExpanded);
        }
    }

    public void ToggleBatteryArrays(){
        batteryArrayExpanded = !batteryArrayExpanded;
        for(int i=1; i<batteryParallelAmount; i++){
            batteryContainer.transform.GetChild(i).gameObject.SetActive(batteryArrayExpanded);
        }
    }
    
    public void SetupBattery(){
        foreach(Transform child in batteryContainer.transform){
            Destroy(child.gameObject);
        }

        //update visuals
        for(int i = 0; i < batteryParallelAmount; i++){
            GameObject newBatteryRow = Instantiate(rowPrefab, batteryContainer.transform);
            //keep all y position between 0 and 30
            float y = (batteryParallelAmount <= 1) ? 0 : -i * uiHeight / (batteryParallelAmount - 1);
            newBatteryRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            for(int j=0; j < batterySeriesAmount; j ++){
                GameObject newBattery = Instantiate(batteryPrefab, newBatteryRow.transform);
                //keep all the x position within -300 and 300
                float x = (batterySeriesAmount <= 1) ? 0 : -uiWidth*0.5f + j * uiWidth / (batterySeriesAmount - 1);
                newBattery.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
        }
        
    }

    public void UpdateBatteries(){

        //calculate power
        batteryVolts = batterySeriesAmount * singleBatteryVolts;
        batteryTotalAmpHours = batteryParallelAmount * singlebatteryTotalAmpHours;

        float batteryEnergy = batteryVolts * batteryTotalAmpHours;
        float batteryEnergyKW = batteryEnergy / 1000;

        //update text
        batteryDisplayText.text = batteryVolts + "V | " + (batteryTotalAmpHours/1000f).ToString() + "Ahrs | " + batteryEnergyKW + "kWhr";
        chargeControllerBatteryVoltsText.text = batteryVolts + "V";

        //every hour, batteries gets charged by the inputPower
        batteryChargingAmps = (batteryVolts == 0) ? 0 : inputPower / batteryVolts;
        totalPowerWithdraw = totalAmpsWithdraw * 120f;
        totalPowerWithdrawKW = totalPowerWithdraw / 1000;
        batteryDischargingAmps = (batteryVolts == 0) ? 0 : totalPowerWithdraw/ batteryVolts;

        if(batteryCurrentAmpHours >= 0 && batteryCurrentAmpHours <= batteryTotalAmpHours){
            batteryCurrentAmpHours += (batteryChargingAmps * WorldController.s.sunAmount - batteryDischargingAmps) * Time.deltaTime * WorldController.s.currentSpeed;
        }
        
        if(batteryCurrentAmpHours < 0){
            batteryCurrentAmpHours = 0;
        }
        
        if(batteryCurrentAmpHours > batteryTotalAmpHours){
            batteryCurrentAmpHours = batteryTotalAmpHours;
        }

        //get percentage
        batteryChargedPercentage = batteryCurrentAmpHours / batteryTotalAmpHours;
        batteryPercentageText.text = Mathf.RoundToInt(batteryChargedPercentage * 100) + "%";
        batteryPercentageView.transform.localScale = new Vector3(0.6f, 0.6f * batteryChargedPercentage, 0.35f);

        //update ChargeController text
        chargeControllerInputPowerText.text = "IN:\n" + inputPowerKW.ToString("0.0") + "kW";
        chargeControllerOutputPowerText.text = "OUT:\n" + totalPowerWithdrawKW.ToString("0.0")+ "kW";
        
    }

    public void UpdateGeneratorParallelText(string _txt){
        generatorParallelAmount = _txt == "" ? 0 : int.Parse(_txt);
        SetupSolar();
        
    }

    public void UpdateGeneratorSeriesText(string _txt){
        generatorSeriesAmount = _txt == "" ? 0 : int.Parse(_txt);
        SetupSolar();
    }

    public void UpdateBatterySeriesText(string _txt){
        batterySeriesAmount = _txt == "" ? 0 : int.Parse(_txt);
        SetupBattery();
    }

    public void UpdateBatteryParallelText(string _txt){
        batteryParallelAmount = _txt == "" ? 0 : int.Parse(_txt);
        SetupBattery();
    }

    public void UpdateInverterText(float _t){
        inverterAmpsWithdrawText.text = _t.ToString("0.0") + "A";
    }
}
