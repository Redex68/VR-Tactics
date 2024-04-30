using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;

    public static bool mouseOverButton {get; private set;} = false;
    private static int mouseOverCount = 0;
    
    void Awake() {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public static void setMouseOverButton(bool b)
    {
        if(b) mouseOverCount++;
        else mouseOverCount--;
        mouseOverButton = mouseOverCount > 0;
    }


    public static bool canSelectArea()
    {
        return !mouseOverButton && !UnitPlacer.placingUnit;
    }
}
