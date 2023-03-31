using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayerControler : MonoBehaviour
{

    [SerializeField] float maxDistance = 250f;
    [SerializeField] float minDistance = 200f;
    [SerializeField] [Range(0.0f, 1.0f)] float startDistancePercent;
    private float zoom;
    private float defaultLocalZ;
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GameObject.Find("RTS Player Canvas").GetComponent<Canvas>();
        canvas.worldCamera = gameObject.GetComponentInChildren<Camera>();

        defaultLocalZ = transform.localPosition.z;
        zoom = Mathf.Lerp(minDistance, maxDistance, 1 - startDistancePercent);
    }

    // Update is called once per frame
    void Update()
    {
        Canvas canvas = GameObject.Find("RTS Player Canvas").GetComponent<Canvas>();
        canvas.worldCamera = gameObject.GetComponentInChildren<Camera>();

        float forwardMovement = Input.GetAxis("Vertical");
        float sideMovement = Input.GetAxis("Horizontal");

        Vector3 posNew = transform.position;
        float coef = 12500.0f / zoom * Time.deltaTime;
        Vector3 moveDir = transform.forward;
        moveDir.y = 0;
        moveDir = moveDir.normalized;
        Vector3 sidewaysDir = transform.right;
        sidewaysDir.y = 0;
        sidewaysDir = sidewaysDir.normalized;

        posNew += moveDir * forwardMovement * coef;
        posNew += sidewaysDir * sideMovement * coef;

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel") * 12500.0f * Time.deltaTime;

        if(zoom <= minDistance && scroll >= 0 || zoom >= maxDistance && scroll <= 0 || zoom >= minDistance && zoom <= maxDistance) {
            zoom += scroll;
            posNew += scroll * transform.forward;
        }

        if(Input.GetMouseButton(1)) {
            float mouseX = Input.GetAxisRaw("Mouse X");

            Vector3 angles = transform.eulerAngles;
            angles.y += mouseX * 2;
            transform.eulerAngles = angles;
        }        

        transform.position = posNew;
    }
}
