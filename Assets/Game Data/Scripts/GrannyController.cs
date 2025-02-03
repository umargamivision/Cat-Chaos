using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Ommy.Animation;
using Ommy.Audio;
using UnityEngine;
using UnityEngine.Events;
public class GrannyController : MonoBehaviour, IDetectable
{
    public float hitDetectForce = 1;
    [Header("Audio Clips")]
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip burnClip;
    public AudioClip beeAttackClip;
    public AudioClip shockClip;
    public AudioClip[] angryClips;
    public StateMachineEventListner stateMachineEventListner;
    public PlayerController playerController;
    public GrannyStateManager grannyStateManager;
    public GrannyItemCollision grannyItemCollision;
    public ParticleSystem[] vfxs;
    public UnityEvent onGrannyHit;
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
            case StateMachineEventType.FootStep:
                //audioSource.PlayOneShot(footstepClip);
            break;
            case StateMachineEventType.FireRun:
                PlaySound(burnClip);
            break;
            case StateMachineEventType.BeeAttack:
                PlaySound(beeAttackClip);
            break;
            case StateMachineEventType.Shocked:
                PlaySound(shockClip);
            break;
            case StateMachineEventType.GrannyAnger:
                PlaySound(angryClips[UnityEngine.Random.Range(0,angryClips.Length)]);
            break;
        }
    }
    public void PlaySound(AudioClip audioClip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    [InspectorButton]
    public void ChasePlayer()
    {
        grannyStateManager.SwitchState(grannyStateManager.grannyChasingState);
    }
    public void ChasePlayerTill(int time)
    {
        grannyStateManager.chaseCoolDownTime=time;
        grannyStateManager.SwitchState(grannyStateManager.grannyChasingState);
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.CompareTag("Item"))
        {
            //ChasePlayer();
        }
        if(other.transform.TryGetComponent<Item>(out Item item))
        {
            if(item.itemType == ItemType.beeComb)
            {
                ShootGranny(SpecialItemType.Bee);
            }
            else
            {
                Debug.Log("hit force: "+item.grabbable.rb.velocity.magnitude);
                if(item.grabbable.rb.velocity.magnitude>hitDetectForce)
                {
                    ChasePlayer();
                    onGrannyHit.Invoke();
                    grannyItemCollision.CollideWith(item.itemType);
                }
            }
        }
    }
    public void GrannyCatch()
    {
        SetVFX(SpecialItemType.Bee, false);
        playerController.GrannyCatch();
    }
    public void GiveCatDamage()
    {
        grannyStateManager.holdState = false;
        playerController.TakeGrannyDamage();
    }

    public void OnDetected()
    {
        Debug.Log("Granny Detected");
    }
    public void SetVFX(SpecialItemType specialItemType, bool active)
    {
        foreach (var vfx in vfxs)
        {
            vfx.gameObject.SetActive(false);
        }
        vfxs[(int)specialItemType].gameObject.SetActive(active);
    }
    public void SetVFX(bool active)
    {
        foreach (var vfx in vfxs)
        {
            vfx.gameObject.SetActive(false);
        }
    }
    public void ShootGranny(SpecialItemType specialItemType)
    {
        Debug.Log("Shooted with " + specialItemType);
        switch (specialItemType)
        {
            case SpecialItemType.Bee:
                if(grannyStateManager.currentState != grannyStateManager.grannyBeeState)
                    grannyStateManager.SwitchState(grannyStateManager.grannyBeeState);
            break;
            case SpecialItemType.Fire:
                if(grannyStateManager.currentState != grannyStateManager.grannyOnFireState)
                grannyStateManager.SwitchState(grannyStateManager.grannyOnFireState);
            break;
            case SpecialItemType.Electric:
                if(grannyStateManager.currentState != grannyStateManager.grannyShockedState)
                grannyStateManager.SwitchState(grannyStateManager.grannyShockedState);
            break;            
        }
        SetVFX(specialItemType, true);
    }
}