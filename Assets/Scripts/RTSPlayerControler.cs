using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayerControler : MonoBehaviour
{

    [Tooltip("An arbitrary number that represents the maximum distance the RTS player can zoom out by.")]
    [SerializeField] float maxDistance = 250f;
    [Tooltip("An arbitrary number that represents the maximum distance the RTS player can zoom in by.")]
    [SerializeField] float minDistance = 200f;
    [SerializeField] Sprite EnemyMarker;
    private float zoom;
    private float defaultLocalZ;
    private Renderer[] playerRenderers;
    private Camera RTSCamera;
    private RectTransform markerTransform;
    // Start is called before the first frame update
    void Start()
    {
        // Canvas canvas = GameObject.Find("RTS Player Canvas").GetComponent<Canvas>();
        // canvas.worldCamera = gameObject.GetComponentInChildren<Camera>();

        playerRenderers = GameObject.Find("PlayerController").GetComponentsInChildren<Renderer>();
        RTSCamera = GetComponentInChildren<Camera>();
        markerTransform = GameObject.Find("EnemyMarker").GetComponent<RectTransform>();
        defaultLocalZ = transform.localPosition.z;
        zoom = minDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //Move camera
        float forwardMovement = Input.GetAxisRaw("Vertical");
        float sideMovement = Input.GetAxisRaw("Horizontal");

        Vector3 posNew = transform.position;
        float coef = 12500.0f / zoom * Time.deltaTime;
        Vector3 moveDir = transform.forward;
        moveDir.y = 0;
        Vector3 sidewaysDir = transform.right;
        sidewaysDir.y = 0;
        moveDir = (moveDir * forwardMovement + sidewaysDir * sideMovement).normalized;

        posNew += moveDir * coef;

        //Zoom in
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 12500.0f * Time.deltaTime;

        if(zoom <= minDistance && scroll >= 0 || zoom >= maxDistance && scroll <= 0 || zoom >= minDistance && zoom <= maxDistance) {
            zoom += scroll;
            posNew += scroll * transform.forward;
        }
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
        markerTransform.position = posOnScreen + new Vector2(0, 10);
    }
}
