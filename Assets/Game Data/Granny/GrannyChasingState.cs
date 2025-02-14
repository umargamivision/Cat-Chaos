using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyChasingState : GrannyBaseState
{
    GrannyStateManager grannyStateManager;
    Coroutine chaseCoolDownCoroutine;
    public override void EnterState(GrannyStateManager stateManager)
    {
        Debug.Log("Granning Enter ChasingState");
        grannyStateManager = stateManager;
        stateManager.malee.SetActive(true);
        stateManager.animator.SetTrigger("AngryPointingGesture"); // Normalize speed to be between 0 and 1
        stateManager.navMeshAgent.speed = stateManager.chaseSpeed;
        if(chaseCoolDownCoroutine!=null)
        {
            CoroutineManager.Instance.StopCor(chaseCoolDownCoroutine);
        }
        chaseCoolDownCoroutine = CoroutineManager.Instance.StartCor
        (
            ChaseCoolDownTimer(stateManager.chaseCoolDownTime,
            OnCoolDown
        ));
    }
    public void OnCoolDown()
    {
        grannyStateManager.onGrannyCoolDown.Invoke();
        grannyStateManager.SwitchState(grannyStateManager.grannyPatrollingState);
    }
    public override void ExitState(GrannyStateManager stateManager)
    {
        Debug.Log("Granning Exit ChasingState");
    }
    public override void OnCollisionEnter(GrannyStateManager stateManager, Collision collision)
    {
    }
    public override void UpdateState(GrannyStateManager stateManager)
    {
        Debug.Log("Granning Update ChasingState");
        //stateManager.animator.SetFloat("MoveSpeed",stateManager.navMeshAgent.speed); // Normalize speed to be between 0 and 1
        Vector3 playerPos = PlayerController.Instance.transform.position;
        stateManager.navMeshAgent.SetDestination(playerPos);
        if (Vector3.Distance(stateManager.grannyDistanceCheckPoint.position,playerPos)<stateManager.attackDistance)
        {
            stateManager.SwitchState(stateManager.grannyAttackState);
        }
    }
    public void SetAttackState()
    {
        grannyStateManager.SwitchState(grannyStateManager.grannyAttackState);
    }
    public IEnumerator ChaseCoolDownTimer(int coolDownTime, Action onCoolDown)
    {
        int chaseTime=0;
        while(coolDownTime>=chaseTime)
        {
            yield return new WaitForSeconds(1);
            chaseTime++;
            Debug.Log("chase time : "+chaseTime);
        }
        onCoolDown.Invoke();
    }
}
