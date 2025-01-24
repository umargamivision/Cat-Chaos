using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Events;

public class IntractableDetector : MonoBehaviour
{
    public LayerMask layerMask;
    [SerializeField] private Transform origin;

    [Header("Settings")]
    [SerializeField] private float detectionLength = 5f;
    private Ray ray;
    public UnityEvent<RaycastHit> onRayCast;
    public UnityEvent<IGrabbable> onGrabableHit;
    public UnityEvent<IDoor> onDoorDetected;
    private void Update()
    {
        PerformDetection();
    }
    private void PerformDetection()
    {
        RaycastHit raycastHit;
        ray.origin = origin.position;
        ray.direction = origin.forward;

        IDoor iDoor = null;
        IGrabbable iGrabbable = null;
        if (Physics.Raycast(ray, out raycastHit, detectionLength, layerMask))
        {
            iGrabbable = raycastHit.collider.GetComponent<IGrabbable>();
            onGrabableHit.Invoke(iGrabbable);

            iDoor = raycastHit.collider.GetComponent<IDoor>();
            onDoorDetected.Invoke(iDoor);
        }
        else
        {
            onGrabableHit.Invoke(iGrabbable);
            onDoorDetected.Invoke(iDoor);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * detectionLength);
    }
}
