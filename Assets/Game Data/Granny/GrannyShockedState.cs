using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyShockedState : GrannyBaseState
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

        stateManager.navMeshAgent.velocity = Vector3.zero;
        stateManager.navMeshAgent.speed = 0;
        stateManager.StartSpecialReaction(6);

        if (CoolDownCoroutine != null)
        {
            CoroutineManager.Instance.StopCor(CoolDownCoroutine);
        }
        CoolDownCoroutine = CoroutineManager.Instance.StartCor
        (
            CoolDownTimer(stateManager.shockedCoolDown,
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

    }
}
