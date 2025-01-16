using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ommy.Audio;
using UnityEngine;
using UnityEngine.Events;

public sealed class Grabbable : IGrabbable
{
    public Rigidbody rb;
    public Grabbable(bool canGrab) : base(canGrab)
    {
    }
    public override void Grab()
    {
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
    public override void Throw(Vector3 direction,float force)
    {
        if (isGrabbed)
        {
            isGrabbed=false;
            rb.isKinematic=false;
            rb.AddForce(direction*force,ForceMode.Impulse);
            transform.parent = null;
            OnThrow.Invoke();
        }
    }
}
public abstract class IGrabbable : MonoBehaviour
{
    public UnityEvent OnGrab;
    public UnityEvent OnThrow;
    public bool isGrabbed;
    public bool canGrab=true;
    public abstract void Grab();
    public abstract void Throw(Vector3 direction, float force);
    public IGrabbable(bool canGrab)
    {
        this.canGrab = canGrab;
    }
}