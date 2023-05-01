using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] public Camera RTSPlayerCamera;
    // Update is called once per frame
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
