using System;
using UnityEngine;
using UnityEngine.Events;

public class TapDetector : MonoBehaviour
{
    [Header("Tap Detection Settings")]
    [SerializeField] private float maxTapThreshold = 50f; // Maximum distance for a tap in pixels
    [SerializeField] private float maxTapTime = 0.2f; // Maximum time for a tap in seconds

    [Header("Events")]
    public UnityEvent<Vector2> OnTap; // Invoked with screen position of the tap

    private Vector2 initialTouchPosition;
    private float touchStartTime;

    private bool isTouching;

    private void Update()
    {
        DetectTap();
    }

    private void DetectTap()
    {
        if (Input.GetMouseButtonDown(0)) // Touch or mouse button pressed
        {
            isTouching = true;
            initialTouchPosition = Input.mousePosition;
            touchStartTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && isTouching) // Touch or mouse button released
        {
            isTouching = false;

            // Check if the gesture qualifies as a tap
            Vector2 finalTouchPosition = Input.mousePosition;
            float distance = Vector2.Distance(initialTouchPosition, finalTouchPosition);
            float duration = Time.time - touchStartTime;

            if (distance <= maxTapThreshold && duration <= maxTapTime)
            {
                OnTap?.Invoke(finalTouchPosition); // Trigger tap event with the tap position
            }
        }
    }
}