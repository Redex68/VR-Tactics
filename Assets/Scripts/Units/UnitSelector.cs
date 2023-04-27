using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector Instance;
    
    [SerializeField] private Image selectArea;
    [SerializeField] private Camera RTSPlayerCamera;

    private struct Unit {
        public ControllableUnit script;
        public Transform transform;
    }

    public static bool selectingUnits {get; private set;} = false;
    private static Vector2 startMousePos;

    private static List<Unit> units = new List<Unit>();
    private static List<Unit> selectedUnits = new List<Unit>();
    private static List<Unit> previouslySelectedUnits = new List<Unit>();

    void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Update()
    {
        if(selectingUnits) updateArea();
        else if(Input.GetMouseButton(0) && UIManager.canSelectArea()) startSelecting();
    }

    private static void startSelecting()
    {
        selectingUnits = true;
        startMousePos = Input.mousePosition;
        Instance.selectArea.enabled = true;
        Instance.selectArea.rectTransform.position = startMousePos;

        updateArea();
    }

    private static void stopSelecting()
    {
        selectingUnits = false;
        Instance.selectArea.enabled = false;
    }

    private static void updateArea()
    {
        if(!Input.GetMouseButton(0)) stopSelecting();
        Vector3 pos = Input.mousePosition;

        Vector2 bottomLeft;
        Vector2 topRight;

        if(startMousePos.x < pos.x) 
        {
            bottomLeft.x = startMousePos.x;
            topRight.x = pos.x;
        }
        else
        {
            bottomLeft.x = pos.x;
            topRight.x = startMousePos.x;
        }

        if(startMousePos.y < pos.y)
        {
            bottomLeft.y = startMousePos.y;
            topRight.y = pos.y;
        }
        else
        {
            bottomLeft.y = pos.y;
            topRight.y = startMousePos.y;
        }
        
        RectTransform trans = Instance.selectArea.rectTransform;
        trans.position = bottomLeft;
        trans.sizeDelta = (topRight - bottomLeft);
    }

    private static void getUnitsInSelection()
    {
        foreach(Unit u in units)
        {
            Instance.RTSPlayerCamera.WorldToScreenPoint(u.transform.position);

        }
    }



    public static void addUnit(ControllableUnit unit)
    {
        Unit _unit = new Unit();
        _unit.script = unit;
        _unit.transform = unit.transform;
        units.Add(_unit);
    }

    public static void removeUnit(ControllableUnit unit)
    {
        Unit _unit = units.Find(u => u.script.Equals(unit));
        if(_unit.Equals(default(Unit)))
        {
            Debug.Log("Unit not found in list of all controllable units");
            return;
        }

        units.Remove(_unit);
    }
}
