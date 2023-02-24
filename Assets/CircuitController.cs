using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircuitController : MonoBehaviour
{
    [Header("__Summary__")]
    public TextMeshProUGUI totalInputDisplayText;
    public GameObject rowPrefab;
    [Header("__Generators__")]
    public GameObject generatorContainer;
    public GameObject generatorPrefab;
    public TextMeshProUGUI generatorDisplayText;
    public float inputPower = 0;
    float inputPowerKW = 0;
    public float inputVolts = 0;
    public float inputAmps = 0;
    float generatorEnergyLoss = 0.75f;
    int generatorParallelAmount = 0;
    int generatorSeriesAmount = 0;
    float generatorUIHeight = 35f;
    float generatorUIWidth = 600f;
    
    float solarPanelDefaultAmps = 8;
    float solarPanelDefaultVolts = 30;


    [Header("__ChargeController__")]
    public TextMeshProUGUI chargeControllerInputText;
    public TextMeshProUGUI chargeControllerOutputText;

    [Header("__Batteries__")]
    public GameObject batteryContainer;
    public GameObject batteryPrefab;
    public TextMeshProUGUI batteryDisplayText;
    public int batteryParallelAmount, batterySeriesAmount;

    float batteryDefaultVolts = 14.4f;
    float batteryDefaultAmpHours = 200f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInputPower() {

    }

    public void UpdateGeneratorParallelText(string _txt){
        generatorParallelAmount = _txt == "" ? 0 : int.Parse(_txt);
        UpdateGenerators();
        
    }

    public void UpdateGeneratorSeriesText(string _txt){
        generatorSeriesAmount = _txt == "" ? 0 : int.Parse(_txt);
        UpdateGenerators();
    }

    public void UpdateBatterySeriesText(string _txt){
        batterySeriesAmount = _txt == "" ? 0 : int.Parse(_txt);
        UpdateBatteries();
    }

    public void UpdateBatteryParallelText(string _txt){
        batteryParallelAmount = _txt == "" ? 0 : int.Parse(_txt);
        UpdateBatteries();
    }
    
    public void UpdateBatteries(){
        foreach(Transform child in batteryContainer.transform){
            Destroy(child.gameObject);
        }

        //update visuals
        for(int i = 0; i < batteryParallelAmount; i++){
            GameObject newBatteryRow = Instantiate(rowPrefab, batteryContainer.transform);
            //keep all y position between 0 and 30
            float y = i * generatorUIHeight / (batteryParallelAmount - 1);
            newBatteryRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            for(int j=0; j < batterySeriesAmount; j ++){
                GameObject newBattery = Instantiate(batteryPrefab, newBatteryRow.transform);
                //keep all the x position within -300 and 300
                float x = -generatorUIWidth*0.5f + j * generatorUIWidth / (batterySeriesAmount - 1);
                newBattery.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
        }

        //calculate power
        float batteryVolts = batterySeriesAmount * batteryDefaultVolts;
        float batteryAmps = batteryParallelAmount * batteryDefaultAmpHours;

        float batteryPower = batteryVolts * batteryAmps;
        float batteryPowerKW = batteryPower / 1000;

        //update text
        batteryDisplayText.text = "TOTAL CAPACITY: " + batteryPowerKW + " kWhr";
    }

    public void UpdateGenerators(){
        foreach(Transform child in generatorContainer.transform){
            Destroy(child.gameObject);
        }

        //update visuals
        for(int i = 0; i < generatorParallelAmount; i++){
            GameObject newGeneratorRow = Instantiate(rowPrefab, generatorContainer.transform);
            //keep all y position between 0 and 30
            float y = i * generatorUIHeight / (generatorParallelAmount - 1);
            newGeneratorRow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            for(int j=0; j < generatorSeriesAmount; j ++){
                GameObject newGenerator = Instantiate(generatorPrefab, newGeneratorRow.transform);
                //keep all the x position within -300 and 300
                float x = -generatorUIWidth*0.5f + j * generatorUIWidth / (generatorSeriesAmount - 1);
                newGenerator.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
        }

        //calculate power
        inputVolts = generatorSeriesAmount * solarPanelDefaultVolts;
        inputAmps = generatorParallelAmount * solarPanelDefaultAmps;

        inputPower = inputVolts * inputAmps * generatorEnergyLoss;

        inputPowerKW = inputPower / 1000;

        //update text
        generatorDisplayText.text = "Generating: " + inputPowerKW + " kW";
        totalInputDisplayText.text = inputPowerKW + " kW";
        chargeControllerInputText.text = inputVolts + " V\n" + inputAmps + " A";
    }
}
