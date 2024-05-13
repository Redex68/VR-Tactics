using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitButton: Button {
    //Doesn't work for some reason
    [SerializeField] public NetworkPrefabRef unit;
    [SerializeField] public GameObject unitTemplate;
    [SerializeField] public string unitName;
    [SerializeField] public int maxUnitCount;
    
    public int numUnitsLeft;
    private TMPro.TMP_Text numUnitsLeftText;

    protected override void Start()
    {
        base.Start();
        numUnitsLeftText = transform.parent.Find("Counter/Number (TMP)")?.GetComponent<TMPro.TMP_Text>();
        if(numUnitsLeftText) numUnitsLeftText.text = numUnitsLeft.ToString();
        else Debug.LogError("No unit number counter found.");
    }

    void Update()
    {
        if(!Application.isPlaying) return;

        if (unitName == "Roadblock") numUnitsLeft = FindObjectOfType<UnitCreator>()?.BarricadeUnitCounter ?? maxUnitCount;
        else if (unitName == "SWAT") numUnitsLeft = FindObjectOfType<UnitCreator>()?.SwatUnitCounter ?? maxUnitCount;
        else Debug.LogError($"Unknown unit name: {unitName}");

        if (numUnitsLeftText) numUnitsLeftText.text = numUnitsLeft.ToString();
        if (numUnitsLeft <= 0) this.interactable = false;
        else this.interactable = true;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if(interactable) UnitPlacer.placeUnit(unit, unitTemplate, unitName);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        UIManager.setMouseOverButton(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        UIManager.setMouseOverButton(false);
    }
}