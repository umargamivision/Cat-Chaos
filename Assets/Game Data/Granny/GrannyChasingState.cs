using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyChasingState : GrannyBaseState
{
    public override void EnterState(GrannyStateManager stateManager)
    {
        stateManager.navMeshAgent.speed = stateManager.chaseSpeed;
    }
    public override void ExitState(GrannyStateManager stateManager)
    {
    }
    public override void OnCollisionEnter(GrannyStateManager stateManager, Collision collision)
    {
    }
    public override void UpdateState(GrannyStateManager stateManager)
    {
        stateManager.animator.SetFloat("MoveSpeed",stateManager.navMeshAgent.speed); // Normalize speed to be between 0 and 1
        Vector3 playerPos = PlayerController.Instance.transform.position;
        stateManager.navMeshAgent.SetDestination(playerPos);
        if (Vector3.Distance(stateManager.transform.position,playerPos)<stateManager.attackDistance)
        {
            stateManager.SwitchState(stateManager.grannyAttackState);
            //stateManager.wayPointSystem.OnReachWayPoint();
        }
    }
}
