using UnityEngine;

public class TPOnSpawn : MonoBehaviour
{
    BNG.PlayerTeleport teleporter;

    void Awake()
    {
        teleporter = GetComponentInChildren<BNG.PlayerTeleport>();
    }

    void OnEnable()
    {
        Debug.Log("Teleporting");
        teleporter.TeleportPlayerToTransform(transform);
    }
}
