using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaUnitSelector : MonoBehaviour
{
    [SerializeField] private Image selectArea;
    [SerializeField] private Camera RTSPlayerCamera;

    public static bool selectingUnits {get; private set;} = false;
    private static Vector2 startMousePos;


    void Update()
    {
        if(selectingUnits) updateArea();
        else if(Input.GetMouseButton(0) && UIManager.canSelectArea()) startSelecting();
    }

//Updates the script's state
    private void startSelecting()
    {
        selectingUnits = true;
        startMousePos = Input.mousePosition;
        selectArea.enabled = true;
        selectArea.rectTransform.position = startMousePos;

        //First frame refresh
        updateArea();
    }

//Updates the script's state
    private void stopSelecting()
    {
        selectingUnits = false;
        selectArea.enabled = false;
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
        
        RectTransform trans = selectArea.rectTransform;
        trans.position = bottomLeft;
        trans.sizeDelta = (topRight - bottomLeft);
    }

    private void getUnitsInSelection()
    {
        // foreach(Unit u in units)
        // {
        //     Instance.RTSPlayerCamera.WorldToScreenPoint(u.transform.position);
        //     //TODO
        // }
    }
}
