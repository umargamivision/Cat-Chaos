using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyAttackState : GrannyBaseState
{
    public override void EnterState(GrannyStateManager stateManager)
    {
        stateManager.malee.SetActive(true);
        stateManager.navMeshAgent.velocity = Vector3.zero;
        stateManager.navMeshAgent.speed = 0;
        stateManager.animator.SetTrigger("Attack");
    }

    public override void ExitState(GrannyStateManager stateManager)
    {
    }

    public override void OnCollisionEnter(GrannyStateManager stateManager, Collision collision)
    {
    }

    public override void UpdateState(GrannyStateManager stateManager)
    {

    }
}
