using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using Fusion;

public class UnitCreator : NetworkBehaviour
{
    [Networked]
    [SerializeField]
    private int swatUnitCounter { get; set; }
    [Networked]
    [SerializeField]
    private int barricadeUnitCounter { get; set; }

    private bool _wasSpawned = false;
    override public void Spawned()
    {
        StartCoroutine(Delayed());
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _wasSpawned = false;
    }

    IEnumerator Delayed()
    {
        yield return new WaitForNextFrameUnit();

        _wasSpawned = true;
    }

    public int? SwatUnitCounter {
        get {
            if (_wasSpawned) return swatUnitCounter;
            else return null;
        }
    }
    public int? BarricadeUnitCounter {
        get {
            if (_wasSpawned) return barricadeUnitCounter;
            else return null;
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RpcSpawnUnit(NetworkPrefabRef unit, Vector3 pos, Quaternion rot, string name)
    {
        NetworkObject obj = Runner.Spawn(unit, pos, rot);
        StartCoroutine(DelayedInit(obj, name));
    }

    private IEnumerator DelayedInit(NetworkObject obj, string name)
    {
        yield return new WaitForNextFrameUnit();

        ControllableUnit script = obj.GetComponent<ControllableUnit>();
        if (script != null) script.place();

        switch(name)
        {
            case "Roadblock":
                barricadeUnitCounter--;
                break;
            case "SWAT":
                swatUnitCounter--;
                break;
            default:
                Debug.LogError($"Unknown unit name: {name}");
                break;
        }
    }
}
