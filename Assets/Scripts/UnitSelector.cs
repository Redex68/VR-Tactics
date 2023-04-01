using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] public GameObject unit;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnitButton>().unit = unit;
    }
}
