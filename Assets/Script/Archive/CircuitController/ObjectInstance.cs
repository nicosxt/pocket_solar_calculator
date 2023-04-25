using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstance : MonoBehaviour
{
    public Connector dcInPositive, dcInNegative, dcOutPositive, dcOutNegative, acIn, acOut;


    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public void EnterObjectSpawner(){
        UiManager.s.hoveringObjectInstance = this;
        //Debug.Log("enter!");
    }

    public void ExitObjectSpawner(){
        if(UiManager.s.hoveringObjectInstance == this){
            UiManager.s.hoveringObjectInstance = null;
        }
        //Debug.Log("exit!");
    }


}