using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class GrannyStateManager : MonoBehaviour
{
    public float patrollingSpeed=0.2f;
    public float chaseSpeed=1;
    public float attackDistance=5;
    public float chaseDistanceLimit;
    public int chaseCoolDownTime;
    public WayPointSystem wayPointSystem;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GrannyBaseState currentState;
    public GrannyPatrollingState grannyPatrollingState = new GrannyPatrollingState();
    public GrannyChasingState grannyChasingState = new GrannyChasingState();
    public GrannyAttackState grannyAttackState = new GrannyAttackState();
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
        animator.SetFloat("MoveSpeed", navMeshAgent.speed);
        currentState.UpdateState(this);
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
    public void OnCollisionEnter(Collision other) 
    {
        currentState.OnCollisionEnter(this, other);    
    }
    public void SwitchState(GrannyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
