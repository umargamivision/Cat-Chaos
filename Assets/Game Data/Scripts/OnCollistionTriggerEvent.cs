using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollistionTriggerEvent : MonoBehaviour
{
    public bool useTag;
    public string tagToCheck;
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;
    public UnityEvent onCollisionStay;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;

    private void OnCollisionEnter(Collision collision)
    {
        if (useTag && collision.gameObject.CompareTag(tagToCheck))
        {
            onCollisionEnter?.Invoke();
        }
        else if (!useTag)
        {
            onCollisionEnter?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (useTag && collision.gameObject.CompareTag(tagToCheck))
        {
            onCollisionExit?.Invoke();
        }
        else if (!useTag)
        {
            onCollisionExit?.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (useTag && collision.gameObject.CompareTag(tagToCheck))
        {
            onCollisionStay?.Invoke();
        }
        else if (!useTag)
        {
            onCollisionStay?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useTag && other.gameObject.CompareTag(tagToCheck))
        {
            onTriggerEnter?.Invoke();
        }
        else if (!useTag)
        {
            onTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (useTag && other.gameObject.CompareTag(tagToCheck))
        {
            onTriggerExit?.Invoke();
        }
        else if (!useTag)
        {
            onTriggerExit?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (useTag && other.gameObject.CompareTag(tagToCheck))
        {
            onTriggerStay?.Invoke();
        }
        else if (!useTag)
        {
            onTriggerStay?.Invoke();
        }
    }
}
