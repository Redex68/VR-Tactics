using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BNG;

public class ControllableUnit : MonoBehaviour
{
    [SerializeField] GameObject weaponMuzzle;
    [SerializeField] float minSpotDistance = 7;
    [SerializeField] float playerPosMemoryTime = 15;
    [SerializeField] float repositionDelay = 7;
    [SerializeField] float repositionMaxDistance = 7;
    [SerializeField] float optimalEngagementDistance = 25;
    [SerializeField] float maxEngagementDistance = 50;

    public LayerMask mask;
    private NavMeshAgent agent;
    private RaycastWeapon weaponScript;
    private Transform VRPlayerController;
    private static int playerLayer;
    private float time = 0;
    private bool dead = false;

    private float lastMoved = 0;
    private bool moving = false;

    private Vector3 lastKnownPlayerPos = Vector3.zero;
    private bool playerInLOS = false;
    private float lastPlayerSighting = 0;
    private bool movingToLastKnown = false;
    private bool movedThisFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        weaponScript = weaponMuzzle.GetComponentInParent<RaycastWeapon>();
        VRPlayerController = GameObject.Find("VR Player/PlayerController").transform;
        playerLayer = LayerMask.NameToLayer("Player");

        UnitSelector.addUnit(this);
    }

    void Update()
    {
        if(!dead && agent.isOnNavMesh)
        {
            checkIfPlayerInLOS();
            if(!movedThisFrame && moving && agent.isOnNavMesh && agent.remainingDistance < agent.stoppingDistance && !dead)
            {
                moving = false;
                movingToLastKnown = false;
                lastMoved = Time.time;
            }

            if(!moving)
            {
                Vector3 relativePlayerPos = VRPlayerController.transform.position - transform.position;
                if(playerInLOS && relativePlayerPos.magnitude < maxEngagementDistance) turnAndShoot();
                else goToLastKnown();

                if(playerInLOS && Time.time - lastMoved > repositionDelay)
                {
                    relativePlayerPos.y = 0;

                    Vector3 newPos = Util.randomPosition(transform.position, repositionMaxDistance);
                    //Bias the new position so that the optimal engagement distance is preserved.
                    if(relativePlayerPos.magnitude < optimalEngagementDistance)
                        newPos -=  relativePlayerPos.normalized * repositionMaxDistance * 0.25f;
                    else
                        newPos +=  relativePlayerPos.normalized * repositionMaxDistance * 0.25f;

                    if(newPos != Vector3.zero) setDestination(newPos);
                }
                
            }
            else if(playerInLOS && movingToLastKnown) 
            {
                movingToLastKnown = false;
                setDestination(transform.position + agent.velocity / 2);
            }
        }
        movedThisFrame = false;
    }

    public void place()
    {
        agent.enabled = true;
        lastMoved = Time.time;
        lastPlayerSighting = Time.time;
    }

    public void setDestination(Vector3 destination) {
        agent.SetDestination(destination);
        moving = true;
        movedThisFrame = true;
    } 

    void OnDestroy()
    {
        UnitSelector.removeUnit(this);
    }

    public void died()
    {
        dead = true;
    }

    private void turnAndShoot()
    {
        float angle = getAngle(VRPlayerController.position, weaponMuzzle.transform.position, weaponMuzzle.transform.forward);
        transform.eulerAngles += Vector3.up * Mathf.Clamp(angle, -Time.deltaTime * 720, Time.deltaTime * 720);

        if(Time.time - time > 1 && angle < 2)
        {
            time = Time.time;
            weaponScript.Shoot();
        }
    }

    private void goToLastKnown()
    {
        if(lastKnownPlayerPos == Vector3.zero) return;
        setDestination(lastKnownPlayerPos);
        movingToLastKnown = true;
        //float angle = getAngle(lastKnownPlayerPos, weaponMuzzle.transform.position, weaponMuzzle.transform.forward);
        //transform.eulerAngles += Vector3.up * Mathf.Clamp(angle, -Time.deltaTime * 720, Time.deltaTime * 720);
    }

    private void checkIfPlayerInLOS()
    {
        Vector3 playerRelativePos = VRPlayerController.position - weaponMuzzle.transform.position;
        Ray ray = new Ray(weaponMuzzle.transform.position, playerRelativePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, mask) && hit.transform.gameObject.layer == playerLayer)
        {
            Vector3 fwd = weaponMuzzle.transform.forward;
            playerRelativePos.y = 0;
            fwd.y = 0;

            //If dot product is positive, the player is infront
            if(Vector3.Dot(fwd, playerRelativePos) > 0)
            {
                //Don't run away if the player showed up in front
                if(playerInLOS == false && lastMoved > repositionDelay) lastMoved = Time.time;
                playerSighted();
                return;
            }
            else playerInLOS = false;
        }
        else playerInLOS = false;

        //If the player is close to the unit
        if(playerRelativePos.magnitude < minSpotDistance) 
        {
            playerSighted();
            return;
        }

        if(Time.time - lastPlayerSighting > playerPosMemoryTime) lastKnownPlayerPos = Vector3.zero;
    }
    
    private void playerSighted()
    {
        playerInLOS = true;
        lastPlayerSighting = Time.time;
        lastKnownPlayerPos = VRPlayerController.position;
    }

    private static float getAngle(Vector3 player, Vector3 weaponMuzzle, Vector3 weaponForward)
    {
        Vector3 playerRelativePos = player - weaponMuzzle;
        playerRelativePos.y = 0;
        weaponForward.y = 0;

        return Vector3.SignedAngle(weaponForward, playerRelativePos, Vector3.up);
    }
}
