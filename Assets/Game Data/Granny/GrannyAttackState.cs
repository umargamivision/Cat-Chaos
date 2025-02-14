using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyAttackState : GrannyBaseState
{
    public override void EnterState(GrannyStateManager stateManager)
    {
        Debug.Log("Granning Enter Attack");

        stateManager.holdState = true;
        stateManager.malee.SetActive(true);
        stateManager.navMeshAgent.velocity = Vector3.zero;
        stateManager.navMeshAgent.speed = 0;
        stateManager.animator.SetTrigger("Attack");
    }

    public override void ExitState(GrannyStateManager stateManager)
    {
        Debug.Log("Granning Exit Attack");
    }

    public override void OnCollisionEnter(GrannyStateManager stateManager, Collision collision)
    {
        Debug.Log("Granning Collsion Attack");
    }

    public override void UpdateState(GrannyStateManager stateManager)
    {
        Debug.Log("Granning Update Attack");
    }
}
