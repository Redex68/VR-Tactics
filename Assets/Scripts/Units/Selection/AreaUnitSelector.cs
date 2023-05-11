using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUnitSelector : MonoBehaviour
{
    [SerializeField] private Image selectArea;

    public bool selectingUnits {get; private set;} = false;
    private Vector2 startMousePos;
    private bool mouseMoved = false;
    private bool deselecting = false;
    private bool appending = false;

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

        if(Input.GetKey(UnitSelector.Instance.deselectKey))
        {
            deselecting = true;
            previouslySelected = new List<UnitSelector.Unit>(UnitSelector.selectedUnits);
        }
        else if(Input.GetKey(UnitSelector.Instance.multiSelectKey))
        {
            appending = true;
            previouslySelected = new List<UnitSelector.Unit>(UnitSelector.selectedUnits);
        }

        //First frame refresh
        updateArea();
    }

//Updates the script's state
    private void stopSelecting()
    {
        selectingUnits = false;
        selectArea.enabled = false;
        previouslySelected = null;
        mouseMoved = false;
        deselecting = false;
        appending = false;
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

        if(!mouseMoved)
        {
            if(Mathf.Abs(Input.mousePosition.x - startMousePos.x) > 2 || Mathf.Abs(Input.mousePosition.y - startMousePos.y) > 2)
                mouseMoved = true;
            else return;
        }

        selectionArea = new Rect(bottomLeft, trans.sizeDelta);
        getUnitsInSelection();
    }

    private void getUnitsInSelection()
    {
        UnitSelector.deselectAllUnits();
        if(deselecting || appending)
            foreach(UnitSelector.Unit u in previouslySelected)
                UnitSelector.selectUnit(u, true);
        foreach(UnitSelector.Unit u in UnitSelector.units.Values)
            if(selectionArea.Contains(UnitSelector.Instance.RTSPlayerCamera.WorldToScreenPoint(u.transform.position)))
            {
                if(deselecting)
                    UnitSelector.deselectUnit(u);
                else
                    UnitSelector.selectUnit(u, true);
            }
    }
}
