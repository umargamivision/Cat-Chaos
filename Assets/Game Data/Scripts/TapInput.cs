using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class TapDetector : MonoBehaviour
{
    [Header("Tap Detection Settings")]
    [SerializeField] private float maxTapThreshold = 50f; // Maximum distance for a tap in pixels
    [SerializeField] private float maxTapTime = 0.2f; // Maximum time for a tap in seconds

    [Header("Ignored UI Elements")]
    [SerializeField] private List<GameObject> ignoredUIElements; // List of UI elements to ignore

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
        if (Input.touchCount > 0) // Check for touch input
        {
            Touch touch = Input.GetTouch(0);

            if (IsTouchOverIgnoredUI(touch.position))
                return; // Ignore tap if over specific UI element

            switch (touch.phase)
            {
                case TouchPhase.Began: // Touch started
                    isTouching = true;
                    initialTouchPosition = touch.position;
                    touchStartTime = Time.time;
                    break;

                case TouchPhase.Ended: // Touch ended
                    if (isTouching)
                    {
                        isTouching = false;

                        Vector2 finalTouchPosition = touch.position;
                        float distance = Vector2.Distance(initialTouchPosition, finalTouchPosition);
                        float duration = Time.time - touchStartTime;

                        if (distance <= maxTapThreshold && duration <= maxTapTime)
                        {
                            OnTap?.Invoke(finalTouchPosition); // Trigger tap event with the tap position
                        }
                    }
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(0)) // Mouse input for editor or desktop
        {
            if (IsTouchOverIgnoredUI(Input.mousePosition))
                return; // Ignore click if over specific UI element

            isTouching = true;
            initialTouchPosition = Input.mousePosition;
            touchStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0) && isTouching) // Mouse button released
        {
            if (IsTouchOverIgnoredUI(Input.mousePosition))
                return; // Ignore click if over specific UI element

            isTouching = false;

            Vector2 finalTouchPosition = Input.mousePosition;
            float distance = Vector2.Distance(initialTouchPosition, finalTouchPosition);
            float duration = Time.time - touchStartTime;

            if (distance <= maxTapThreshold && duration <= maxTapTime)
            {
                OnTap?.Invoke(finalTouchPosition); // Trigger tap event with the tap position
            }
        }
    }

    private bool IsTouchOverIgnoredUI(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (ignoredUIElements.Contains(result.gameObject))
            {
                return true; // Touch is over an ignored UI element
            }
        }

        return false;
    }
}