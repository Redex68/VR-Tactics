using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BNG;

public class ControllableUnit : MonoBehaviour
{
    [SerializeField] GameObject weaponMuzzle;

    public LayerMask mask;
    private Vector3 goal;
    private NavMeshAgent agent;
    private RaycastWeapon weaponScript;
    private Transform VRPlayerController;
    private static int playerLayer;
    private float time = 0;
    private bool dead = false;


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
        if(agent.isOnNavMesh && agent.remainingDistance < 0.2 && !dead)
        {
            Vector3 playerRelativePos = VRPlayerController.position - weaponMuzzle.transform.position;
            Ray ray = new Ray(weaponMuzzle.transform.position, playerRelativePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, mask) && hit.transform.gameObject.layer == playerLayer)
            {
                Vector3 fwd = weaponMuzzle.transform.forward;
                playerRelativePos.y = 0;
                fwd.y = 0;

                float angle = Vector3.SignedAngle(fwd, playerRelativePos, Vector3.up);

                if(MathF.Abs(angle) < 90)
                {
                    transform.eulerAngles += Vector3.up * Mathf.Clamp(angle, -Time.deltaTime * 720, Time.deltaTime * 720);

                    if(Time.time - time > 1)
                    {
                        time = Time.time;
                        weaponScript.Shoot();
                    }
                    Debug.DrawLine(weaponMuzzle.transform.position, transform.position + playerRelativePos, Color.green);
                }
                else Debug.DrawLine(weaponMuzzle.transform.position, transform.position + playerRelativePos, Color.red);
            }
            else Debug.DrawLine(weaponMuzzle.transform.position, transform.position + playerRelativePos, Color.red);
        }
    }

    public void place()
    {
        agent.enabled = true;
    }

    public void setDestination(Vector3 destination) {
        goal = destination;
        agent.SetDestination(goal);
    }

    void OnDestroy()
    {
        UnitSelector.removeUnit(this);
    }

    public void died()
    {
        dead = true;
    }
}
