using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : NetworkBehaviour
{
    [SerializeField] LayerMask moveRaycastTargets;
    [SerializeField] PlayerTypeVariable playerType;

    void Update()
    {
        if(playerType.value == PlayerType.RTS && Input.GetMouseButtonDown(1))
        {
            Ray ray = UnitSelector.Instance.RTSPlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 400.0f, moveRaycastTargets))
                RpcMoveUnits(UnitSelector.selectedUnits.Select(o => o.script.gameObject.GetComponent<NetworkObject>()).ToArray(), hit.point);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RpcMoveUnits(NetworkObject[] units, Vector3 dest)
    {
        foreach(NetworkObject unit in units)
            unit.GetComponent<ControllableUnit>().setDestination(dest);
    }
}
