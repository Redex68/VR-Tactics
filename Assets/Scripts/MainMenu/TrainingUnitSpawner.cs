using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainingUnitSpawner : NetworkBehaviour
{
    [SerializeField] NetworkPrefabRef unitPrefab;
    [SerializeField] GameEvent spawnUnits;
    
    Transform[] unitSpawnpoints;
    List<NetworkObject> spawnedUnits = new();

    void Awake()
    {
        var spawnPoints = GameObject.Find("Enemies Spawn Points");
        if(spawnPoints != null) unitSpawnpoints = spawnPoints.transform.GetComponentsInChildren<Transform>();
    }

    override public void Spawned()
    {
        if(Runner.IsServer) spawnUnits.OnEvent += SpawnTrainingUnits;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if(Runner.IsServer) spawnUnits.OnEvent -= SpawnTrainingUnits;
    }

    void SpawnTrainingUnits(Component sender, object data)
    {
        foreach(var unit in spawnedUnits)
            if(unit) Runner.Despawn(unit);
        for(int i = 1; i < unitSpawnpoints.Length; i++)
        {
            var unitPos = unitSpawnpoints[i];
            var unit = Runner.Spawn(unitPrefab, unitPos.position, unitPos.rotation);
            spawnedUnits.Add(unit);
            StartCoroutine(DelayedInit(unit, "SWAT"));
        }
    }
    private IEnumerator DelayedInit(NetworkObject obj, string name)
    {
        yield return new WaitForNextFrameUnit();

        ControllableUnit script = obj.GetComponent<ControllableUnit>();
        if (script != null) script.place();
    }
}
