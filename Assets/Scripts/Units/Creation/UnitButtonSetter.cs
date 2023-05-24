using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButtonSetter : MonoBehaviour
{
    [SerializeField] public GameObject unit;
    [SerializeField] public string unitName;
    [SerializeField] public int maxUnitCount;

    // Start is called before the first frame update
    void Start()
    {
        UnitButton button = GetComponent<UnitButton>();
        button.unit = unit;
        button.unitName = unitName;
        button.maxUnitCount = maxUnitCount;
        button.numUnitsLeft = maxUnitCount;
        Destroy(this);
    }
}
