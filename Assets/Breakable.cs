using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject breakable;
    public void BreakObject()
    {
        Instantiate(breakable,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
