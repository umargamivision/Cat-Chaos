using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Ommy.Animation;
using Ommy.Attribute;
using UnityEngine;
public class GrannyController : MonoBehaviour
{
    public StateMachineEventListner stateMachineEventListner;
    public PlayerController playerController;
    public GrannyStateManager grannyStateManager;
    private void OnEnable() 
    {
        stateMachineEventListner.OnEventInvoke.AddListener(OnAnimationEventInvoke);
    }
    public void OnAnimationEventInvoke(StateMachineEventType stateMachineEventType)
    {
        switch (stateMachineEventType)
        {
            case StateMachineEventType.GrannyAttackStart:
                GrannyCatch();
            break;
            case StateMachineEventType.GrannyAttackDone:
                GiveCatDamage();
            break;
        }
    }
    [InspectorButton]
    public void ChasePlayer()
    {
        grannyStateManager.SwitchState(grannyStateManager.grannyChasingState);
    }
    public void GrannyCatch()
    {
        playerController.GrannyCatch();
    }
    public void GiveCatDamage()
    {
        playerController.TakeGrannyDamage();
    }
}