using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using Fusion;

public class UnitCreator : NetworkBehaviour
{
    public static UnityEvent<string> unitCreated = new UnityEvent<string>();

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RpcSpawnUnit(NetworkPrefabRef unit, Vector3 pos, Quaternion rot)
    {
        NetworkObject obj = Runner.Spawn(unit, pos, rot);
        StartCoroutine(DelayedInit(obj));
    }

    private IEnumerator DelayedInit(NetworkObject obj)
    {
        yield return new WaitForNextFrameUnit();

        ControllableUnit script = obj.GetComponent<ControllableUnit>();
        if (script != null) script.place();
        unitCreated?.Invoke(obj.name);
    }
}
