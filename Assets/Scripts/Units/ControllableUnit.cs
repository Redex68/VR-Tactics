using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllableUnit : MonoBehaviour
{
    Vector3 goal;
    [SerializeField] NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void isPlaced()
    {
        agent.enabled = true;
    }

    void setDestination(Vector3 destination) {
        goal = destination;
        agent.SetDestination(goal);
    }

    void OnDestroy()
    {
        Debug.Log("Here");
    }
}
