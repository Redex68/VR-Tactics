using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitAndDeathManager : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator> ();
    }

    public void onHit(float dmg)
    {
        anim.SetTrigger("hit");
    }

    public void onDeath()
    {
        anim.SetTrigger("dead");
    }
}
