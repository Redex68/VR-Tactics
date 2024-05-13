using BNG;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BNG.Damageable))]
public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private FloatEventVariable playerHealth;
    [SerializeField] private GameEvent gameEnd;
    private BNG.Damageable dmg;
    private float maxHp;

    void Start() {
        dmg = GetComponent<BNG.Damageable>();
        playerHealth.value = dmg.Health;
        maxHp = dmg.Health;
    }

    public void PlayerDied()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            var teleporter = GetComponent<BNG.PlayerTeleport>();
            teleporter.TeleportPlayerToTransform(GameObject.Find("VR Player Spawn Point").transform);
            dmg.Health = maxHp;
            PlayerDamaged(-maxHp);
        }
        else gameEnd.Raise(this, Victor.RTS);
    }

    //Gets called by BNG.Damageable
    public void PlayerDamaged(float damage)
    {
        playerHealth.value = dmg.Health;
    }
}
