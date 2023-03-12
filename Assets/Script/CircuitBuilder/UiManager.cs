using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager s;
    MainActions playerActions;
    void Awake() {
        if (s != null && s != this) {
            Destroy(this.gameObject);
        } else {
            s = this;
        }
        playerActions = new MainActions();
    }
    private void OnEnable() {
        playerActions.Enable();
    }
    private void OnDisable() {
        playerActions.Disable();
    }

    //public TextMeshProUGUI debugText;

    [Header("UI")]
    //public bool isOverUI = false;
    public bool manipulateBackground = false;
    public bool dragBackground = false;
    public bool isTouching = false;
    public GameObject pointingObject;
    public Vector2 pointerPosition;
    //public Vector3 worldPos;
    // public Ray mouseRay;

    [Header("Moving Background")]
    public GameObject bg;
    float bgMoveMax = 45f;
    float moveRate = 5f;
    Vector3 lastPointerPosition;
    

    [Header("Zoom Related")]
    public Camera camera;
    Vector3 curBGScale;
    Vector2 bgScaleRange = new Vector2(30f, 150f);
    float zoomRate = 10f;

    //[Header("Spawn Object")]
    public ObjectSpawner hoveringObjectSpawner;
    public ObjectInstance holdingObjectInstance;
    public ObjectInstance hoveringObjectInstance;
    

    // Start is called before the first frame update
    void Start()
    {
        if(manipulateBackground){
            playerActions.Build.Move.started += DragStart;
            playerActions.Build.Move.performed += Dragging;
            playerActions.Build.Move.canceled += DragStop;
            playerActions.Build.Zoom.performed += Zooming;
        }
    }

    void Update()
    {
    }

    void Zooming(InputAction.CallbackContext ctx){
        Vector2 zoomAmount = ctx.ReadValue<Vector2>();
        if(zoomAmount.y != 0){
            curBGScale = bg.transform.localScale;
            curBGScale.x = Mathf.Clamp(curBGScale.x + zoomAmount.y * Time.deltaTime * zoomRate, bgScaleRange.x, bgScaleRange.y);
            curBGScale.y = curBGScale.x;
            curBGScale.z = curBGScale.x;
            bg.transform.localScale = curBGScale;
        }
    }

    void DragStart(InputAction.CallbackContext ctx) {
        lastPointerPosition = camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        //Debug.Log("Drag Start");
    }

    void Dragging(InputAction.CallbackContext ctx) {
        //Debug.Log("Dragging");
        pointerPosition = ctx.ReadValue<Vector2>();
        if(holdingObjectInstance){
            //Debug.Log("Dragging Object");
            holdingObjectInstance.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, bg.transform.position.z - 10f));
        }
        else{
            if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
                //Debug.Log("Moving BG");
                MoveBackground(pointerPosition);
                dragBackground = true;
            }

        }

    }

    void DragStop(InputAction.CallbackContext ctx) {
        //Debug.Log("Drag Stop");

        dragBackground = false;
        if(holdingObjectInstance)
            holdingObjectInstance = null;
    }

    void MoveBackground(Vector2 pointerPosition){
        Vector3 currentPointerPosition = camera.ScreenToWorldPoint(pointerPosition);
        Vector3 movementDelta = currentPointerPosition - lastPointerPosition;
        movementDelta /= camera.orthographicSize;
        movementDelta.z = 0;
        //Debug.Log(movementDelta);
        
        Vector3 bgPos = bg.transform.localPosition;
        bgPos += movementDelta * moveRate;
        bgPos.x = Mathf.Clamp(bgPos.x, -bgMoveMax, bgMoveMax);
        bgPos.y = Mathf.Clamp(bgPos.y, -bgMoveMax, bgMoveMax);
        bg.transform.localPosition = bgPos;

        lastPointerPosition = currentPointerPosition;
    }
}
