using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ommy.Audio;
using UnityEngine;
using UnityEngine.Events;

public sealed class Grabbable : IGrabbable
{
    public bool setScaleOnGrab;
    public Vector3 scaleOnGrab;
    public Outline outline;
    public Rigidbody rb;
    public Grabbable(bool canGrab) : base(canGrab)
    {
    }
    private void OnEnable()
    {
        if (outline == null) outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public void OnFocus(bool focus)
    {
        outline.enabled = focus;
    }
    public override void Grab()
    {
        if (setScaleOnGrab)
            transform.localScale = scaleOnGrab;
        isGrabbed = true;
        rb.isKinematic = true;
        OnGrab.Invoke();
    }
    public void Release()
    {
        if (isGrabbed)
        {
            isGrabbed = false;
            rb.isKinematic = false;
            transform.parent = null;
        }
    }
    public override void Throw(Vector3 direction, float force)
    {
        if (isGrabbed)
        {
            isGrabbed = false;
            rb.isKinematic = false;
            rb.AddForce(direction * force, ForceMode.Impulse);
            transform.parent = null;
            OnThrow.Invoke();
        }
    }
}
public abstract class IGrabbable : MonoBehaviour
{
    public enum State
    {
        Idle,Throw,Grab
    }
    public State state;
    public UnityEvent OnGrab;
    public UnityEvent OnThrow;
    public UnityEvent OnThrowCollision;
    public bool isGrabbed;
    public bool canGrab = true;
    public abstract void Grab();
    private void OnCollisionEnter(Collision other) 
    {
        
        OnThrowCollision.Invoke();    
    }
    public abstract void Throw(Vector3 direction, float force);
    public IGrabbable(bool canGrab)
    {
        this.canGrab = canGrab;
    }
}