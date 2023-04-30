using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector Instance;

    private struct Unit {
        public ControllableUnit script;
        public Transform transform;
    }
    
    //A list containing all of the selectable units in the game
    private static List<Unit> units = new List<Unit>();
    //A list containing all of the currently selected units
    private static List<Unit> selectedUnits = new List<Unit>();
    //A list containing all of the units that were slected in the previous frame
    private static List<Unit> previouslySelectedUnits = new List<Unit>();

    void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

//Adds the unit to the selector script's list of all units
    public static void addUnit(ControllableUnit unit)
    {
        Unit _unit = new Unit();
        _unit.script = unit;
        _unit.transform = unit.transform;
        units.Add(_unit);
    }

//Removes the unit from the selector script's list of all units
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
