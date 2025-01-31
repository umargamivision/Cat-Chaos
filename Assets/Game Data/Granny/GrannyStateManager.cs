using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GrannyStateManager : MonoBehaviour
{
    public float patrollingSpeed=0.2f;
    public float chaseSpeed=1;
    public float attackDistance=5;
    public float chaseDistanceLimit;
    public int chaseCoolDownTime;
    public int burnRunCoolDown=10;
    public int beeAttackCoolDown=10;
    public int shockedCoolDown=4;
    public Transform grannyDistanceCheckPoint;
    public WayPointSystem wayPointSystem;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GrannyBaseState currentState;
    public GameObject malee;
    public GrannyPatrollingState grannyPatrollingState = new GrannyPatrollingState();
    public GrannyChasingState grannyChasingState = new GrannyChasingState();
    public GrannyAttackState grannyAttackState = new GrannyAttackState();
    public GrannyBeeState grannyBeeState = new GrannyBeeState();
    public GrannyOnFireState grannyOnFireState = new GrannyOnFireState();
    public GrannyShockedState grannyShockedState = new GrannyShockedState();
    public UnityEvent onGrannyCoolDown;
    public UnityEvent<GrannyBaseState> onStateChange;
    [Header("Debug")]
    public float distanceWithPlayer;

    public void Start()
    {
        currentState = grannyPatrollingState;
        currentState.EnterState(this);
    }
    public void Update()
    {
        //CheckChaseThresould();
        currentState.UpdateState(this);
        animator.SetFloat("MoveSpeed", navMeshAgent.velocity.magnitude);
    }
    public void CheckChaseThresould()
    {
        distanceWithPlayer = Vector3.Distance(PlayerController.Instance.transform.position,transform.position);
        if(distanceWithPlayer>chaseDistanceLimit)
        {
            if(currentState == grannyChasingState)
            {
                SwitchState(grannyPatrollingState);
            }
        }
    }
    public void StartSpecialReaction(int index)
    {
        animator.SetFloat("HitReactionBlendTree_Index",index);
        animator.SetTrigger("ReactionHit_Start");
    }
    public void OnCollisionEnter(Collision other) 
    {
        currentState.OnCollisionEnter(this, other);    
    }
    public void SwitchState(GrannyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
        onStateChange.Invoke(state);
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(grannyDistanceCheckPoint.position,attackDistance);
    }
}
