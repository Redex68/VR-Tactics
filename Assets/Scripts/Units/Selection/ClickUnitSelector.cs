using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnitSelector : MonoBehaviour
{
    [SerializeField] Camera RTSPlayerCamera;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && UIManager.canSelectArea())
        {
            Ray ray = RTSPlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            ControllableUnit unit = null;
            if(Physics.Raycast(ray, out hit) && (unit = hit.transform.GetComponentInParent<ControllableUnit>()) != null)
            {
                bool append = Input.GetKey(KeyCode.LeftControl);
                UnitSelector.selectUnit(unit, append);
            }
            else UnitSelector.deselectUnits();
        }
    }
}
