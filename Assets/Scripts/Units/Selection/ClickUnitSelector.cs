using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnitSelector : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && UIManager.canSelectArea())
        {
            Ray ray = UnitSelector.Instance.RTSPlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            ControllableUnit unit = null;
            if(Physics.Raycast(ray, out hit) && (unit = hit.transform.GetComponentInParent<ControllableUnit>()) != null)
            {
                if(Input.GetKey(UnitSelector.Instance.deselectKey))
                    UnitSelector.deselectUnit(unit);
                else
                {
                    bool append = Input.GetKey(UnitSelector.Instance.multiSelectKey);
                    UnitSelector.selectUnit(unit, append);
                }
            }
            else if(!Input.GetKey(UnitSelector.Instance.deselectKey) && !Input.GetKey(UnitSelector.Instance.multiSelectKey))
                UnitSelector.deselectAllUnits();
        }
    }
}
