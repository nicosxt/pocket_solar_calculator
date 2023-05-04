using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject obj;
    public GameObject obj2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetObjActive(bool _activateDefault){
        obj.SetActive(_activateDefault);
        if(obj2 != null){
            obj2.SetActive(!_activateDefault);
        }
    }

    public void Toggle(){
        obj.SetActive(!obj.activeSelf);
        if(obj2 != null){
            obj2.SetActive(!obj2.activeSelf);
        }
    }

}
