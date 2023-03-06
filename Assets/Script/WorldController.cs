using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldController : MonoBehaviour
{

    public static WorldController s;

    [Header("Time Related")]
    // 🌙-sunriseTime-/-sunriseLength-☀️☀️☀️-sunsetTime-\-sunsetLength-🌙
    public float sunriseTime = 6f;
    public float sunriseLength = 0.5f;
    public float sunsetTime = 18f;
    public float sunsetLength = 0.5f;
    //public float fullSunTime = 2f;
    float dayLength;

    public float currentTimeHour, currentTimeMinutes = 0f;
    public TextMeshProUGUI timeText;
    public float currentSpeed = 1f;
    //when speed = 1/60f, 1s game time = 1s real time
    public int speedIndex = 0;
    float[] speedOptions = {0f, 1f, 2f, 5f};
    public GameObject speedButton;

    //environment related
    [Header("Environment Related")]
    public float sunAmount = 0f;
    public Material skybox;
    public Color day1, day2, night1, night2;
    public Color uiDay, uiNight;
    public Image[] uiAdaptableImages;
    public TextMeshProUGUI[] uiAdaptableTexts;


    void Awake()
    {
        if (s != null && s != this) {
            Destroy(this.gameObject);
        } else {
            s = this;
        }
        dayLength = sunsetTime - (sunriseTime + sunriseLength);
    }

    //Time does not stop
    void Start()
    {
        currentSpeed = speedOptions[speedIndex];
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeMinutes += Time.deltaTime * currentSpeed * 60f;
        if (currentTimeMinutes > 60f) {
            currentTimeMinutes = 0f;
            currentTimeHour += 1f;
        }
        if (currentTimeHour > 24f) {
            currentTimeHour=0f;
        }
        //display current hour and minutes in the form of 00:00
        timeText.text = currentTimeHour.ToString("00") + ":" + currentTimeMinutes.ToString("00");

        if(currentTimeHour < sunriseTime){
            sunAmount = 0f;
        }else if(currentTimeHour >= sunriseTime && currentTimeHour < sunriseTime + sunriseLength){
            //lerp sunAmount from 0 to 1 based on currentTimeHour
            sunAmount = Mathf.Lerp(0f, 1f, (currentTimeHour - sunriseTime) / sunriseLength);
        }else if(currentTimeHour >= sunriseTime + sunriseLength && currentTimeHour < sunsetTime){
            sunAmount = 1f;
        }else if(currentTimeHour > sunsetTime && currentTimeHour < sunsetTime + sunsetLength){
            //lerp sunAmount from 1 to 0 based on currentTimeHour
            sunAmount = Mathf.Lerp(1f, 0f, (currentTimeHour - sunsetTime) / sunsetLength);
        }else if(currentTimeHour >= sunsetTime + sunsetLength){
            sunAmount = 0f;
        }

        UpdateColors();
        
    }

    void UpdateColors(){
        skybox.SetColor("_Color2", Color.Lerp(night1, day1, sunAmount));
        skybox.SetColor("_Color1", Color.Lerp(night2, day2, sunAmount));

        foreach(Image img in uiAdaptableImages){
            img.color = Color.Lerp(uiNight, uiDay, sunAmount);
            
        }
        foreach(TextMeshProUGUI txt in uiAdaptableTexts){
            txt.color = Color.Lerp(uiNight, uiDay, sunAmount);
        }
    }

    public void ChangeSpeed(){
        speedIndex ++;
        speedIndex %= (speedOptions.Length-1);
        currentSpeed = speedOptions[speedIndex];
        for(int i=0; i<speedButton.transform.childCount; i++){
            speedButton.transform.GetChild(i).gameObject.SetActive(i == speedIndex); 
        }
    }

}
