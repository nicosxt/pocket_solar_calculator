using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectInstance objectToSpawn;

    //Standard
    public float inputV, inputA, outputV, outputA;

    //Battery
    public float operatingV, operatingA, totalAhrs;
    
    // Start is called before the first frame update
    void Start()
    {
        EventTrigger trigger = GetComponentInChildren<EventTrigger>();
        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.BeginDrag;
        drag.callback.AddListener((data) => { SpawnObject(); });
        trigger.triggers.Add(drag);

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { OnPointerDown(); });
        trigger.triggers.Add(pointerDown);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(){
        //Debug.Log("pointer down");
    }

    public void SpawnObject(){

        //UiManager.s.debugText.text = "Spawn " + gameObject.name;

        //Debug.Log("spawn " + gameObject.name);
        ObjectInstance obj = Instantiate(objectToSpawn, UiManager.s.bg.transform);
        obj.transform.localEulerAngles = new Vector3(0, 180, 0);

        //detect if obj is also Solar
        if(obj.GetType() == typeof(Solar)){
            Solar solar = (Solar)obj;
            solar.operatingV = outputV;
            solar.operatingA = outputA;
            solar.operatingP = outputV * outputA;
        }else if(obj.GetType() == typeof(Battery)){
            Battery battery = (Battery)obj;
            battery.operatingV = operatingV;
            battery.totalAhrs = totalAhrs;
        }else if(obj.GetType() == typeof(Appliance)){
            Appliance appliance = (Appliance)obj;
            appliance.operatingA = inputA;
        }

        UiManager.s.holdingObjectInstance = obj;
    }

}
