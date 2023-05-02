using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector Instance;

    public struct Unit {
        public ControllableUnit script;
        public Transform transform;
        public GameObject marker;

        public Unit(ControllableUnit script)
        {
            this.script = script;
            transform = script.transform;
            marker = script.transform.Find("Marker").gameObject;
        }
    }
    
    //A list containing all of the selectable units in the game
    public static Dictionary<ControllableUnit, Unit> units = new Dictionary<ControllableUnit, Unit>();
    //A list containing all of the currently selected units
    public static List<Unit> selectedUnits {get;} = new List<Unit>();

    void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

//Adds the unit to the selector script's list of all units
    public static void addUnit(ControllableUnit unit)
    {
        Unit _unit = new Unit(unit);
        _unit.marker.SetActive(false);
        units.Add(unit, _unit);
    }

//Removes the unit from the selector script's list of all units
    public static void removeUnit(ControllableUnit unit)
    {
        Unit _unit = units[unit];
        if(_unit.Equals(default(Unit)))
        {
            Debug.Log("Unit not found in list of all controllable units");
            return;
        }

        units.Remove(unit);
    }

    public static void selectUnit(ControllableUnit unit, bool append)
    {
        if(!append) deselectUnits();
        selectUnit(units[unit]);
    }

    public static void selectUnit(Unit unit, bool append)
    {
        if(!append) deselectUnits();
        selectUnit(unit);
    }

    private static void selectUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        unit.marker.SetActive(true);
    }

    public static void deselectUnits()
    {
        foreach(Unit u in selectedUnits)
            u.marker.SetActive(false);
        selectedUnits.Clear();
    }
}
