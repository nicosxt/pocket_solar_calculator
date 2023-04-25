using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject dragObj;
    //Know what player is pressing on
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateDrag(){
        Debug.Log("Drag Start");
        dragObj.SetActive(!dragObj.activeSelf);
    }
}
