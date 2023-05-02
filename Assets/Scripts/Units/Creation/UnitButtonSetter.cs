using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButtonSetter : MonoBehaviour
{
    [SerializeField] public GameObject unit;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnitButton>().unit = unit;
        Destroy(this);
    }
}
