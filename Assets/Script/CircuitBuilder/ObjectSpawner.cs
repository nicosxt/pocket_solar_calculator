using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterObjectSpawner(){
        UiManager.s.hoveringObjectSpawner = this;
    }

    public void ExitObjectSpawner(){
        if(UiManager.s.hoveringObjectSpawner == this){
            UiManager.s.hoveringObjectSpawner = null;
        }

    }

    public ObjectInstance SpawnObject(){
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
        
        return obj;
    }

}
