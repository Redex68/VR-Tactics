using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BNG.Damageable))]
public class CommsTruck : MonoBehaviour
{
    [SerializeField] GameObject smallFire;
    [SerializeField] GameObject largeFire;
    [SerializeField] GameObject explosion;
    [SerializeField] GameEvent gameEnd;
    private BNG.Damageable damageable;
    private float maxHp;

    void Start()
    {
        damageable = GetComponent<BNG.Damageable>();
        maxHp = damageable.Health;
    }

    public void OnDamage(float damage)
    {
        if(damageable.Health / maxHp < 0.75)
            smallFire.SetActive(true);
        if(damageable.Health / maxHp < 0.40)
            largeFire.SetActive(true);
    }

    public void OnDestruction()
    {
        gameEnd.Raise(this ,Victor.VR);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
