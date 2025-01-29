using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;

public class Grabber : Singleton<Grabber>
{
    [Header("Item Detection")]
    [SerializeField] private Transform grabberPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float grabDuration = 0.5f;
    public UnityEvent<bool> OnDetectGrabbable;

    public Grabbable currentGrabbable, detectedGrabbable;

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
        if (currentGrabbable != null) currentGrabbable.Throw(initialTouchPosition,throwForce);
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
    private Vector2 initialTouchPosition; // Store the starting position of the touch
    public float maxSwipeThreshold = 50f; // Maximum distance to consider as a tap

    public void InputThrowGrabbable(Vector2 tapPos)
    {
        if (currentGrabbable == null) return;
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
    public void PerformDetection(IGrabbable grabbable)
    {
        if(grabbable!=null)
        {
            detectedGrabbable = grabbable as Grabbable;
            DetectedGrabbable(true);
        }
        else
        {
            DetectedGrabbable(false);
        }
    }

    public void DetectedGrabbable(bool detect)
    {
        if (detectedGrabbable)
        {
            detectedGrabbable.OnFocus(detect);
        }
        OnDetectGrabbable.Invoke(detect);
    }
    private void ClearCurrentGrabbable()
    {
        if (currentGrabbable == null) return;

        currentGrabbable.transform.SetParent(null);
        currentGrabbable = null;
    }
}