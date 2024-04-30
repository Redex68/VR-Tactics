using System.Net;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RTSPlayerControler : MonoBehaviour
{
    [Serializable]
    private struct CustomBounds
    {
        [SerializeField] public float xMin;
        [SerializeField] public float xMax;
        [SerializeField] public float zMin;
        [SerializeField] public float zMax;
    }

    [Tooltip("An arbitrary number that represents the maximum distance the RTS player can zoom out by.")]
    [SerializeField] float maxHeight;
    [Tooltip("An arbitrary number that represents the maximum distance the RTS player can zoom in by.")]
    [SerializeField] float minHeight;
    [SerializeField] CustomBounds cameraBounds;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject mainMenu;
    private float defaultLocalZ;
    private Renderer[] playerRenderers;
    private Camera RTSCamera;
    private RectTransform markerTransform;
    private float zoom;
    private float desiredY;

    // Start is called before the first frame update
    void Start()
    {
        playerRenderers = GameObject.Find("VR Player/BodyIK").GetComponentsInChildren<Renderer>();
        RTSCamera = GetComponentInChildren<Camera>();
        markerTransform = GameObject.Find("RTS Player Canvas/EnemyMarker").GetComponent<RectTransform>();
        defaultLocalZ = transform.localPosition.z;
        desiredY = transform.position.y;
        EventManager.onGameOver.AddListener(onGameOver);
        zoom = calculateZoom();
    }

    // Update is called once per frame
    void Update()
    {
        //Open main menu
        if(Input.GetKeyDown(KeyCode.Escape) && !UnitPlacer.placingUnit)
            mainMenu.SetActive(!mainMenu.activeSelf);
        if(mainMenu.activeSelf) return;

        //Move camera
        float forwardMovement = Input.GetAxisRaw("Vertical");
        float sideMovement = Input.GetAxisRaw("Horizontal");

        //For testing
        //if(Input.GetMouseButton(0)) 
        //{
        //    GameObject.Find("TestGun").GetComponent<BNG.RaycastWeapon>().Shoot();
        //}

        Vector3 posNew = transform.position;
        float coef = (Input.GetKey(KeyCode.LeftShift) == true ? 50.0f : 25.0f) * zoom * Time.deltaTime;
        Vector3 moveDir = transform.forward;
        moveDir.y = 0;
        Vector3 sidewaysDir = transform.right;
        sidewaysDir.y = 0;
        moveDir = (moveDir * forwardMovement + sidewaysDir * sideMovement).normalized;
        posNew += moveDir * coef;

        //Zoom in
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 7500.0f * Time.deltaTime;

        Vector3 move = scroll * transform.forward;
        if(desiredY + move.y >= minHeight && desiredY + move.y <= maxHeight)
        {
            zoom = calculateZoom();
            desiredY += move.y;

            posNew += move;
            posNew.y = desiredY;
        }
        validatePos(ref posNew, transform.position);

        transform.position = posNew;


        //Rotate camera
        if(Input.GetMouseButton(2)) {
            float mouseX = Input.GetAxisRaw("Mouse X");

            Vector3 angles = transform.eulerAngles;
            angles.y += mouseX * 2;
            transform.eulerAngles = angles;
        }

        //Move the marker that denotes the player
        Bounds bounds = Util.getBounds(playerRenderers);
        Vector2 posOnScreen = Util.getMarkerPos(bounds, RTSCamera);
        
        //Util.drawBounds(bounds);

        markerTransform.position = posOnScreen;
    }
    
    private float calculateZoom()
    {
        return Mathf.InverseLerp(minHeight, maxHeight, desiredY) + 1;
    }

    private void validatePos(ref Vector3 pos, Vector3 original)
    {
        if(pos.x <= cameraBounds.xMin
        || pos.x >= cameraBounds.xMax)
            pos.x = original.x;

        if(pos.z <= cameraBounds.zMin
        || pos.z >= cameraBounds.zMax)
            pos.z = original.z;

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        Vector3 offsetPos = pos;
        offsetPos.y += 150f;
        Physics.Raycast(offsetPos, Vector3.down, out hit, 300f);
        float height = hit.point.y + 5f;

        if(height > pos.y) pos.y = height;
    }

    void onGameOver(EventManager.Victor victor)
    {
        GameObject.Find("RTS Player Canvas/Unit buttons")?.SetActive(false);
        switch(victor)
        {
            case EventManager.Victor.RTSPlayerWin:
            {
                victoryScreen.SetActive(true);
            } break;
            case EventManager.Victor.VRPlayerWin:
            {
                gameOverScreen.SetActive(true);
            } break;
        }
    }
}