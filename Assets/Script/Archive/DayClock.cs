using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayClock : MonoBehaviour
{
    public GameObject tickObj;
    public int divisionAmount = 360;

    public float swipeSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    public float rotationX = 0.0f;
    public float swipeDelta = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateTicks();
    }



    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                
                swipeDelta = touch.deltaPosition.x * swipeSpeed * Time.deltaTime;
                rotationX += swipeDelta;

                Debug.Log("swiping" + swipeDelta);

                Quaternion newRotation = Quaternion.Euler(0, 0, rotationX);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void InstantiateTicks(){
        for(int i = 1; i < divisionAmount; i++){
            GameObject newTick = Instantiate(tickObj, transform);
            newTick.transform.localEulerAngles = new Vector3(0, 0, i * (360 / divisionAmount));
        }
    }
}
