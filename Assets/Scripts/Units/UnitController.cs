using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Camera RTSPlayerCamera;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = UnitSelector.Instance.RTSPlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
                foreach(UnitSelector.Unit unit in UnitSelector.selectedUnits)
                    unit.script.setDestination(hit.point);
        }
    }
}
