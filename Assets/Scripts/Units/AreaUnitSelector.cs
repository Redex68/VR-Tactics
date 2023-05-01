using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUnitSelector : MonoBehaviour
{
    [SerializeField] private Image selectArea;
    [SerializeField] private Camera RTSPlayerCamera;

    public bool selectingUnits {get; private set;} = false;
    private Vector2 startMousePos;


    private Vector2 bottomLeft;
    private Vector2 topRight;
    private Rect selectionArea;

    private List<UnitSelector.Unit> previouslySelected = null;

    void Update()
    {
        if(selectingUnits) updateArea();
        else if(Input.GetMouseButtonDown(0) && UIManager.canSelectArea()) startSelecting();
    }

//Updates the script's state
    private void startSelecting()
    {
        selectingUnits = true;
        startMousePos = Input.mousePosition;
        selectArea.enabled = true;
        selectArea.rectTransform.position = startMousePos;

        if(Input.GetKey(KeyCode.LeftControl)) previouslySelected = new List<UnitSelector.Unit>(UnitSelector.selectedUnits);

        //First frame refresh
        updateArea();
    }

//Updates the script's state
    private void stopSelecting()
    {
        selectingUnits = false;
        selectArea.enabled = false;
        previouslySelected = null;
    }

//Refreshes the selection area that the RTS player can see
    private void updateArea()
    {
        if(!Input.GetMouseButton(0))
        {
            stopSelecting();
            return;
        }

        Vector3 pos = Input.mousePosition;

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
        
        RectTransform trans = selectArea.rectTransform;
        trans.position = bottomLeft;
        trans.sizeDelta = (topRight - bottomLeft);

        selectionArea = new Rect(bottomLeft, trans.sizeDelta);
        getUnitsInSelection();
    }

    private void getUnitsInSelection()
    {
        UnitSelector.deselectUnits();
        foreach(UnitSelector.Unit u in UnitSelector.units.Values)
            if(selectionArea.Contains(RTSPlayerCamera.WorldToScreenPoint(u.transform.position)))
                UnitSelector.selectUnit(u, true);
        if(previouslySelected != null)
            foreach(UnitSelector.Unit u in previouslySelected)
                UnitSelector.selectUnit(u, true);
    }
}
