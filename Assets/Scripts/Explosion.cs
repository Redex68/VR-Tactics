using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        float maxLen = Mathf.Max(GetComponent<ParticleSystem>().main.duration, GetComponent<AudioSource>().clip.length);
        Destroy(this, maxLen);
    }
}
