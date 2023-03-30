using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{

    [SerializeField] public GameObject unit;

    [SerializeField] public Camera RTSPlayer;

    void Update() {
        
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = RTSPlayer.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;

            if(Physics.Raycast(ray, out hitinfo)) GameObject.Instantiate(unit, hitinfo.point, Quaternion.identity);
        }
    }
}
