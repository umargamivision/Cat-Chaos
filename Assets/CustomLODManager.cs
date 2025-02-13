using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLOD : MonoBehaviour
{
    public MeshRenderer[] meshRenderers;
    public Collider enableTrigger, exitTrigger;
    public void EnableLOD()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }
    public void DisableLOD()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }
}
