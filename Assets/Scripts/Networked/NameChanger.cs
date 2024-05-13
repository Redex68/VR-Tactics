using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChanger : MonoBehaviour
{
    [SerializeField] string Name;
    void Start()
    {
        gameObject.name = Name;
        Destroy(this);
    }
}