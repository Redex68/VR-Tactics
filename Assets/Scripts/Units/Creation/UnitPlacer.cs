using Fusion;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine;

public class UnitPlacer: MonoBehaviour
{
    [SerializeField] public Camera RTSPlayer;
    [SerializeField] public KeyCode rotateKey = KeyCode.LeftShift;
    [SerializeField] public LayerMask placeRaycastTargets;
    [SerializeField] public LayerMask placeableLayer;
    [SerializeField] float minDistanceFromVRPlayer = 50.0f;
    [SerializeField] Material placeableMaterial;
    [SerializeField] Material notPlaceableMaterial;
    [SerializeField] private PlayerTypeVariable playerType;

    private static UnitPlacer Instance;
    private GameObject VRPlayer;

    //Whether a unit is currently being placed or not
    public static bool placingUnit { get; private set; } = false;
    //The unit that's currently being placed by the RTS player
    private UnitHolder beingPlaced;
    private struct UnitHolder
    {
        public NetworkPrefabRef unit;
        public GameObject unitTemplate;
        public string name;
        public List<Collider> colliders;
        public List<NavMeshObstacle> navmeshColliders;
        public List<MaterialHolder> materials;
        public List<LayerHolder> layers;
    };

    private struct LayerHolder
    {
        public Transform transform;
        public int oldLayer;

        public LayerHolder(Transform transform, int layer)
        {
            this.transform = transform;
            this.oldLayer = layer;
        }
    };

    private struct MaterialHolder
    {
        public Renderer renderer;
        public Material[] oldMaterials;

        public MaterialHolder(Renderer renderer, Material[] materials)
        {
            this.renderer = renderer;
            this.oldMaterials = materials;
        }
    }

    private bool rotatingUnit = false;
    private Vector2 rotationCenter;
    private bool validPos;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        beingPlaced.colliders = new List<Collider>();
        beingPlaced.navmeshColliders = new List<NavMeshObstacle>();
        beingPlaced.materials = new List<MaterialHolder>();
        beingPlaced.layers = new List<LayerHolder>();
    }

    void Update()
    {
        //Check if this is being run on the RTS player side
        if (playerType.value == PlayerType.RTS)
        {
            checkRotating();

            if (placingUnit)
            {
                if (rotatingUnit) rotateUnit();
                else moveUnit();

                if (Input.GetKey(KeyCode.Escape) || Input.GetMouseButtonDown(1)) placingUnitFail();
                if (!Input.GetMouseButton(0)) tryPlacingDown();
            }
        }
    }

    public static void placeUnit(NetworkPrefabRef unit, GameObject unitTemplate, String unitName)
    {
        placingUnit = true;

        Instance.setupUnit(unit, unitTemplate, unitName);
        Instance.moveUnit();
    }

    /// <summary>
    /// Rotates the unit in the direction of the mouse.
    /// </summary>
    private void rotateUnit()
    {
        //Get the angle of the mouse to the centre of rotation
        Vector2 reference = Vector2.up;
        float angle = Vector2.SignedAngle(reference, new Vector2(Input.mousePosition.x, Input.mousePosition.y) - rotationCenter);

        //Find the angle to which the unit should be rotated to
        float cameraAngle = Mathf.Atan2(RTSPlayer.transform.forward.x, RTSPlayer.transform.forward.z) * Mathf.Rad2Deg;
        float newAngle = cameraAngle - angle;

        beingPlaced.unitTemplate.transform.eulerAngles = new Vector3(0, newAngle, 0);
    }

    /// <summary>
    /// Moves the unit to the mouse's current position. 
    /// </summary>
    private void moveUnit()
    {
        Ray ray = RTSPlayer.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        bool wasValid = validPos;
        VRPlayer = GameObject.Find("VR Player/PlayerController");

        if (Physics.Raycast(ray, out hitinfo, 400.0f, placeRaycastTargets))
        {
            validPos = ((1 << hitinfo.transform.gameObject.layer) & placeableLayer.value) != 0
            && (VRPlayer != null && Vector3.Distance(hitinfo.point, VRPlayer.transform.position) > minDistanceFromVRPlayer);

            beingPlaced.unitTemplate.transform.position = hitinfo.point;
        }
        else
        {
            validPos = false;
            beingPlaced.unitTemplate.transform.position = RTSPlayer.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50.0f));
        }
        if (wasValid != validPos)
            foreach (MaterialHolder holder in beingPlaced.materials)
                holder.renderer.material = validPos ? placeableMaterial : notPlaceableMaterial;
    }

    /// <summary>
    /// Attempts to put the unit down at it's current position by checking the validPos variable.
    /// If validPos is false then the unit is destroyed, otherwise it is placed down.
    /// </summary>
    /// <returns>Whether the unit was placed down or not.</returns>
    private bool tryPlacingDown()
    {
        if (validPos)
        {
            //Put down the unit
            placeDownUnit();
        }
        else
        {
            //Unit is in an invalid position, destroy it.
            placingUnitFail();
        }

        return validPos;
    }

    /// <summary>
    /// Sets the default parameters for the unit including disabling all colliders and making it invisible to the
    /// VR player.
    /// </summary>
    private void setupUnit(NetworkPrefabRef unit, GameObject unitTemplate, string unitName)
    {
        beingPlaced.unit = unit;
        beingPlaced.unitTemplate = GameObject.Instantiate(unitTemplate);
        beingPlaced.name = unitName;
        float cameraAngle = Mathf.Atan2(RTSPlayer.transform.forward.x, RTSPlayer.transform.forward.z) * Mathf.Rad2Deg;
        beingPlaced.unitTemplate.transform.eulerAngles = new Vector3(0, cameraAngle - 90f, 0);

        //Disable all colliders and save them
        //beingPlaced.colliders.Clear();
        //beingPlaced.navmeshColliders.Clear();
        //beingPlaced.layers.Clear();
        //Collider[] colliders = beingPlaced.unit.GetComponentsInChildren<Collider>();
        //NavMeshObstacle[] navmeshColliders = beingPlaced.unit.GetComponentsInChildren<NavMeshObstacle>();
        //Transform[] transforms = beingPlaced.unit.GetComponentsInChildren<Transform>();
        //
        //foreach(Collider collider in colliders)
        //    if(collider.enabled) {
        //        collider.enabled = false;
        //        beingPlaced.colliders.Add(collider);
        //    }
        //foreach(NavMeshObstacle obstacle in navmeshColliders)
        //    if(obstacle.enabled) {
        //        obstacle.enabled = false;
        //        beingPlaced.navmeshColliders.Add(obstacle);
        //    }
        //foreach(Transform trans in transforms)
        //{
        //    beingPlaced.layers.Add(new LayerHolder(trans, trans.gameObject.layer));
        //    trans.gameObject.layer = 12;
        //}

        //Get the renderers and materials
        beingPlaced.materials.Clear();
        Renderer[] renderers = beingPlaced.unitTemplate.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            beingPlaced.materials.Add(new MaterialHolder(renderer, renderer.materials));
            Material[] newMaterials = new Material[1];
            newMaterials[0] = notPlaceableMaterial;
            renderer.materials = newMaterials;
        }

        validPos = false;
    }

    /// <summary>
    /// Removes any temporary changes made to the unit by the sertupUnit method (including enabling
    /// the object's colliders).
    /// </summary>
    private void placeDownUnit()
    {
        placingUnit = false;
        rotatingUnit = false;
        //beingPlaced.unit.layer = 0;
        //foreach (Collider collider in beingPlaced.colliders) collider.enabled = true;
        //foreach (NavMeshObstacle obstacle in beingPlaced.navmeshColliders) obstacle.enabled = true;
        //foreach (MaterialHolder holder in beingPlaced.materials) holder.renderer.materials = holder.oldMaterials;
        //foreach (LayerHolder holder in beingPlaced.layers) holder.transform.gameObject.layer = holder.oldLayer;

        Vector3 pos = beingPlaced.unitTemplate.transform.position;
        Quaternion rot = beingPlaced.unitTemplate.transform.rotation;
        GameObject.Destroy(beingPlaced.unitTemplate);

        GameObject.FindObjectOfType<UnitCreator>().RpcSpawnUnit(beingPlaced.unit, pos, rot);
    }

    /// <summary>
    /// Checks whether the user is currently trying to rotate the unit and saves the rotation center
    /// if they are.
    /// </summary>
    private void checkRotating()
    {
        if (Input.GetKeyDown(rotateKey))
        {
            rotatingUnit = true;
            rotationCenter = Input.mousePosition;
        }

        if (Input.GetKeyUp(rotateKey)) rotatingUnit = false;
    }

    private void placingUnitFail()
    {
        placingUnit = false;
        rotatingUnit = false;
        Destroy(beingPlaced.unitTemplate);
    }
}