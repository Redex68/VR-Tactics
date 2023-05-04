using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Camera RTSPlayerCamera;

    void Start()
    {
        UnitSelector script = GetComponent<UnitSelector>();
        if(!script) 
        {
            Debug.LogError("Game object doesn't contain a UnitSelector script.");
            Destroy(this);
        }
        RTSPlayerCamera = script.RTSPlayerCamera;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = RTSPlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
                foreach(UnitSelector.Unit unit in UnitSelector.selectedUnits)
                    unit.script.setDestination(hit.point);
        }
    }
}
