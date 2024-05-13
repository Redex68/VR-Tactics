using Fusion;
using UnityEngine;

public class UnitButtonSetter : MonoBehaviour
{
    [SerializeField] public NetworkPrefabRef unit;
    [SerializeField] public GameObject unitTemplate;
    [SerializeField] public string unitName;
    [SerializeField] public int maxUnitCount;

    // Start is called before the first frame update
    void Awake()
    {
        UnitButton button = GetComponent<UnitButton>();
        button.unit = unit;
        button.unitTemplate = unitTemplate;
        button.unitName = unitName;
        button.maxUnitCount = maxUnitCount;
        button.numUnitsLeft = maxUnitCount;
        Destroy(this);
    }
}
