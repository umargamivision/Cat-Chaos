using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class GrannyPatrollingState : GrannyBaseState
{
    public override void EnterState(GrannyStateManager stateManager)
    {

        stateManager.malee.SetActive(false);
        stateManager.navMeshAgent.speed = stateManager.patrollingSpeed;
    }
    public override void ExitState(GrannyStateManager stateManager)
    {
    }
    public override void OnCollisionEnter(GrannyStateManager stateManager, Collision collision)
    {
    }
    public override void UpdateState(GrannyStateManager stateManager)
    {
        stateManager.animator.SetFloat("MoveSpeed", stateManager.navMeshAgent.speed); // Normalize speed to be between 0 and 1
        Vector3 point = stateManager.wayPointSystem.GetWayPoint();
        stateManager.navMeshAgent.SetDestination(point);
        if(stateManager.navMeshAgent.HasReachedDestination(0.5f))
        {
            stateManager.wayPointSystem.OnReachWayPoint();
        }
    }
}
