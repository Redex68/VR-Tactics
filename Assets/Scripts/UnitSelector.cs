using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField]
    public GameObject unit;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }

    private void onClick() {
        UnitCreator.placeUnit(unit);
    }
}
