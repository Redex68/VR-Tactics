using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{

    [SerializeField] public GameObject unit;
    [SerializeField] public Camera RTSPlayer;

    //Whether a unit is currently being placed or not
    private bool placingUnit = false;
    //The unit that's currently being placed by the RTS player
    private UnitPlacer beingPlaced;

    private struct UnitPlacer {
        public GameObject unit;
        public List<Collider> colliders;
    };

    void Start() {
        beingPlaced.colliders = new List<Collider>();
    }

    void Update() {
        
        if(!placingUnit && Input.GetMouseButtonDown(0)) {
            placingUnit = true;
            Ray ray = RTSPlayer.ScreenPointToRay(Input.mousePosition);

            beingPlaced.unit = GameObject.Instantiate(unit);
            Collider[] colliders = beingPlaced.unit.GetComponentsInChildren<Collider>();
            beingPlaced.colliders.Clear();
            foreach(Collider collider in colliders) {
                if(collider.enabled) {
                    collider.enabled = false;
                    beingPlaced.colliders.Add(collider);
                }
            }
            beingPlaced.unit.layer = 12;
        }

        else if(placingUnit) {
            Ray ray = RTSPlayer.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;


            if(Physics.Raycast(ray, out hitinfo))
                beingPlaced.unit.transform.position = hitinfo.point;
            else
                beingPlaced.unit.transform.position = RTSPlayer.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50.0f));
                
            if(Input.GetMouseButtonDown(0)) {
                //Put down the unit
                placingUnit = false;
                beingPlaced.unit.layer = 1;
                foreach (Collider collider in beingPlaced.colliders) collider.enabled = true;
            }
        }
    }
}
