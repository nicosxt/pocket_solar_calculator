using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    [Header("UI")]
    public bool isOverUI = false;
    public bool isDragging = false;
    public bool isTouching = false;

    [Header("Moving Background")]
    public GameObject bg;
    public float bgMoveMax = 45f;
    float moveAmount = 12f;
    Vector2 virtualPosDelta, actualPosDelta;

    [Header("Zoom Related")]
    public Camera camera;
    float originalOrthoSize, currentOrthoSize;
    Vector2 orthoSizeRange = new Vector2(2, 12);

    //[Header("Spawn Object")]
    public ObjectSpawner hoveringObjectSpawner;
    public ObjectInstance holdingObjectInstance;
    public ObjectInstance hoveringObjectInstance;
    

    // Start is called before the first frame update
    void Start()
    {
        playerActions.Build.Move.started += DragStart;
        playerActions.Build.Move.performed += Dragging;
        playerActions.Build.Move.canceled += DragStop;

        playerActions.Build.Touch.started += TouchStart;
        playerActions.Build.Touch.performed += Touching;
        playerActions.Build.Touch.canceled += TouchStop;

        playerActions.Build.Zoom.performed += Zooming;

        currentOrthoSize = originalOrthoSize = camera.orthographicSize;
    }

    void Update()
    {
        isOverUI = EventSystem.current.IsPointerOverGameObject();
        
        //when pointer is not on UI, and is currently holding an object, move the object with pointer
        if(isTouching && holdingObjectInstance != null){
            holdingObjectInstance.transform.position = getPointerPos3D(-5f);
        }

    }

    void Zooming(InputAction.CallbackContext ctx){
        Vector2 zoomAmount = ctx.ReadValue<Vector2>();
        if(zoomAmount.y != 0){
            currentOrthoSize = camera.orthographicSize;
            currentOrthoSize += zoomAmount.y * Time.deltaTime * 0.5f;
            camera.orthographicSize = Mathf.Clamp(currentOrthoSize, orthoSizeRange.x, orthoSizeRange.y);
        }
    }

    void TouchStart(InputAction.CallbackContext ctx){
        //Spawn Electronic Objects
        if(isOverUI && hoveringObjectSpawner != null){
            if(hoveringObjectSpawner.objectToSpawn != null){
                holdingObjectInstance = hoveringObjectSpawner.SpawnObject();
            }
        }

        //Move Object Instances
        if(hoveringObjectInstance != null){
            holdingObjectInstance = hoveringObjectInstance;
        }
        isTouching = true;
    }

    void TouchStop(InputAction.CallbackContext ctx){
        if(holdingObjectInstance != null){
            //drop the object
            holdingObjectInstance = null;
        }
        isTouching = false;
    }

    void Touching(InputAction.CallbackContext ctx){

    }

    void DragStart(InputAction.CallbackContext ctx) {


        isDragging = true;
    }

    void DragStop(InputAction.CallbackContext ctx) {
        //Debug.Log("Move Canceled");

        isDragging = false;

    }

    void Dragging(InputAction.CallbackContext ctx) {

        if(UiManager.s.isOverUI || holdingObjectInstance != null) {
            return;
        }

        //Move BG
        MoveBG(ctx);

    }

    void MoveBG(InputAction.CallbackContext ctx){
        Vector3 bgPos = bg.transform.localPosition;

        bgPos.x += ctx.ReadValue<Vector2>().x * moveAmount * Time.deltaTime * currentOrthoSize / originalOrthoSize;
        bgPos.y += ctx.ReadValue<Vector2>().y * moveAmount * Time.deltaTime * currentOrthoSize / originalOrthoSize;

        bgPos.x = Mathf.Clamp(bgPos.x, -bgMoveMax, bgMoveMax);
        bgPos.y = Mathf.Clamp(bgPos.y, -bgMoveMax, bgMoveMax);
        bg.transform.localPosition = bgPos;
    }

    Vector3 getPointerPos3D(float _z){
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        pos.z = _z;
        return pos;
    }

    void RaycastFromPointer(){
        RaycastHit hit;
        LayerMask mask = 1 << 6;
        if (Physics.Raycast(getPointerPos3D(-10), Vector3.forward, out hit, Mathf.Infinity, mask)){
            Debug.Log("hit " + hit.collider.name);
            if(hit.collider.transform.parent.GetComponent<ObjectInstance>()){
                hoveringObjectInstance = hit.collider.transform.parent.GetComponent<ObjectInstance>();
                Debug.Log("hitting + " + hit.collider.name);
            }else{
                hoveringObjectInstance = null;
            }
        }else{
            hoveringObjectInstance = null;
        }
    }
}
