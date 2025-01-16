using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;

public class Grabber : Singleton<Grabber>
{
    [Header("Item Detection")]
    [SerializeField] private Transform grabberPoint;
    [SerializeField] private Transform origin;

    [Header("Settings")]
    [SerializeField] private float detectionLength = 5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float grabDuration = 0.5f;
    public UnityEvent<bool> OnDetectGrabbable;

    private Grabbable currentGrabbable, detectedGrabbable;
    private Ray ray;
    private RaycastHit raycastHit;

    public Camera playerCamera;

    /// <summary>
    /// Throws the currently grabbed object toward a world position.
    /// </summary>
    /// <param name="worldPosition">The target position in world space.</param>
    public void ThrowGrabbableToward(Vector3 worldPosition)
    {
        if (currentGrabbable == null) return;

        Vector3 throwDirection = (worldPosition - grabberPoint.position).normalized;
        currentGrabbable.Throw(throwDirection, throwForce);
        ClearCurrentGrabbable();
    }

    /// <summary>
    /// Handles the logic when a grabbable object is picked up.
    /// </summary>
    /// <param name="grabbable">The grabbable object to pick up.</param>
    public void OnGrab(Grabbable grabbable)
    {
        if (grabbable == null) return;

        currentGrabbable = grabbable;
        currentGrabbable.Grab();
        currentGrabbable.transform.SetParent(grabberPoint);

        currentGrabbable.transform
            .DOMove(grabberPoint.position, grabDuration)
            .SetEase(Ease.InOutSine);

        currentGrabbable.transform
            .DORotate(grabberPoint.eulerAngles, grabDuration)
            .SetEase(Ease.InOutSine);
    }

    public void OnGrab()
    {
        OnGrab(detectedGrabbable);
    }

    private void Update()
    {
        PerformDetection();
        //InputThrowGrabbable();
    }
    private Vector2 initialTouchPosition; // Store the starting position of the touch
    public float maxSwipeThreshold = 50f; // Maximum distance to consider as a tap

    public void InputThrowGrabbable(Vector2 tapPos)
    {
        Ray screenRay = playerCamera.ScreenPointToRay(tapPos);

        // Raycast to determine the throw target
        if (Physics.Raycast(screenRay, out RaycastHit hitInfo))
        {
            // Throw toward the hit point (tap location)
            ThrowGrabbableToward(hitInfo.point);
        }
    }
    /// <summary>
    /// Performs the raycasting detection logic to identify grabbable objects.
    /// </summary>
    private void PerformDetection()
    {
        ray.origin = origin.position;
        ray.direction = origin.forward;

        if (Physics.Raycast(ray, out raycastHit, detectionLength))
        {
            if (raycastHit.collider.TryGetComponent(out Grabbable detectedGrabbable))
            {
                DetectedGrabbable(true);
                HandleDetectedGrabbable(detectedGrabbable);
            }
            else
            {
                DetectedGrabbable(false);
            }
        }
        else
        {
            DetectedGrabbable(false);
        }
    }

    public void DetectedGrabbable(bool detect)
    {
        OnDetectGrabbable.Invoke(detect);
    }

    /// <summary>
    /// Handles logic when a grabbable object is detected.
    /// </summary>
    /// <param name="grabbable">The detected grabbable object.</param>
    private void HandleDetectedGrabbable(Grabbable grabbable)
    {
        this.detectedGrabbable = grabbable;
        // Implementation for handling detected grabbable (e.g., highlighting or UI prompts).
    }

    /// <summary>
    /// Clears the reference to the current grabbable object.
    /// </summary>
    private void ClearCurrentGrabbable()
    {
        if (currentGrabbable == null) return;

        currentGrabbable.transform.SetParent(null);
        currentGrabbable = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * detectionLength);
    }
}