using Fusion;
using UnityEngine;
using UnityEngine.AI;

//Src: https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class LocomotionSimpleAgent : NetworkBehaviour {
    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector3 velocity = Vector2.zero;
    LookAt lookAt;
    [SerializeField] private PlayerTypeVariable playerType;

    override public void Spawned ()
    {
        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        // Don’t update position automatically
        agent.updatePosition = false;
        
        lookAt = GetComponent<LookAt>();
    }
    
    override public void FixedUpdateNetwork()
    {
        if (playerType.value == PlayerType.VR && Runner.IsForward)
        {
            velocity = transform.InverseTransformDirection(agent.velocity);

            bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
            // Update animation parameters
            anim.SetBool("move", shouldMove);
            anim.SetFloat("velx", velocity.x);
            anim.SetFloat("vely", velocity.z);
            anim.SetFloat("vel", velocity.magnitude);

            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
        }
    }

    void OnAnimatorMove ()
    {
        if(agent != null)
        {
            // Update position to agent position
            transform.position = agent.nextPosition;
        }
    }

    public void died()
    {
        agent.isStopped = true;
    }
}