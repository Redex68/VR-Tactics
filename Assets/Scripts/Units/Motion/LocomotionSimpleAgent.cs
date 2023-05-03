using UnityEngine;
using UnityEngine.AI;

//Src: https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class LocomotionSimpleAgent : MonoBehaviour {
    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector3 velocity = Vector2.zero;
    LookAt lookAt;

    void Start ()
    {
        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        // Don’t update position automatically
        agent.updatePosition = false;
        
        lookAt = GetComponent<LookAt>();
    }
    
    void Update ()
    {
        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = transform.InverseTransformDirection(agent.velocity);

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.z);
        anim.SetFloat("vel", velocity.magnitude);

        lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    void OnAnimatorMove ()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }

    public void died()
    {
        agent.isStopped = true;
    }
}