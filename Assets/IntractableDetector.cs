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
    public UnityEvent<ClothTear> onClothTearDetected;
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
        ClothTear clothTear = null;

        if (Physics.Raycast(ray, out raycastHit, detectionLength, layerMask))
        {
            iGrabbable = raycastHit.collider.GetComponent<IGrabbable>();
            onGrabableHit.Invoke(iGrabbable);

            iDoor = raycastHit.collider.GetComponent<IDoor>();
            onDoorDetected.Invoke(iDoor);
        
            clothTear = raycastHit.collider.GetComponent<ClothTear>();
            onClothTearDetected.Invoke(clothTear);
        }
        else
        {
            onGrabableHit.Invoke(iGrabbable);
            onDoorDetected.Invoke(iDoor);
            onClothTearDetected.Invoke(clothTear);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * detectionLength);
    }
}
