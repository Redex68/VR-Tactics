using UnityEngine;

[RequireComponent(typeof(BNG.Damageable))]
public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private FloatEventVariable playerHealth;
    [SerializeField] private GameEvent gameEnd;
    private BNG.Damageable dmg;

    void Start() {
        dmg = GetComponent<BNG.Damageable>();
        playerHealth.value = dmg.Health;
    }

    public void PlayerDied()
    {
        gameEnd.Raise(this, Victor.RTS);
    }

    //Gets called by BNG.Damageable
    public void PlayerDamaged(float damage)
    {
        playerHealth.value = dmg.Health;
    }
}
