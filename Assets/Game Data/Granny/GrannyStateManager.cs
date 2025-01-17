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
    public WayPointSystem wayPointSystem;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GrannyBaseState currentState;
    public GrannyPatrollingState grannyPatrollingState = new GrannyPatrollingState();
    public GrannyChasingState grannyChasingState = new GrannyChasingState();
    public GrannyAttackState grannyAttackState = new GrannyAttackState();
    void Start()
    {
        currentState = grannyPatrollingState;
        currentState.EnterState(this);
    }
    void Update()
    {
        currentState.UpdateState(this);
    }
    void OnCollisionEnter(Collision other) 
    {
        currentState.OnCollisionEnter(this, other);    
    }
    public void SwitchState(GrannyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
