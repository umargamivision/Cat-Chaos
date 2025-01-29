using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyBeeState : GrannyBaseState
{
    GrannyStateManager grannyStateManager;
    Coroutine CoolDownCoroutine;
    public void OnCoolDown()
    {
        grannyStateManager.SwitchState(grannyStateManager.grannyChasingState);
    }
    public IEnumerator CoolDownTimer(int coolDownTime, Action onCoolDown)
    {
        int chaseTime = 0;
        while (coolDownTime >= chaseTime)
        {
            yield return new WaitForSeconds(1);
            chaseTime++;
            Debug.Log("chase time : " + chaseTime);
        }
        onCoolDown.Invoke();
    }
    public override void EnterState(GrannyStateManager stateManager)
    {
        grannyStateManager = stateManager;
        stateManager.navMeshAgent.speed = stateManager.chaseSpeed;

        stateManager.StartSpecialReaction(4);

        if (CoolDownCoroutine != null)
        {
            CoroutineManager.Instance.StopCor(CoolDownCoroutine);
        }
        CoolDownCoroutine = CoroutineManager.Instance.StartCor
        (
            CoolDownTimer(stateManager.beeAttackCoolDown,
            OnCoolDown
        ));
    }

    public override void ExitState(GrannyStateManager stateManager)
    {
    }

    public override void OnCollisionEnter(GrannyStateManager stateManager, Collision collision)
    {
    }

    public override void UpdateState(GrannyStateManager stateManager)
    {

        Debug.Log("bee state");
        Vector3 point = stateManager.wayPointSystem.GetWayPoint();
        stateManager.navMeshAgent.SetDestination(point);
        if(stateManager.navMeshAgent.HasReachedDestination(0.5f))
        {
            stateManager.wayPointSystem.OnReachWayPoint();
        }
    }
}
