using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public GameObject breakable;
    public UnityEvent onBreak;
    public void BreakObject()
    {
        onBreak.Invoke();
        Instantiate(breakable,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
