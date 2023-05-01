using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllableUnit : MonoBehaviour
{
    private Vector3 goal;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        UnitSelector.addUnit(this);
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
}
