using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TapDetector : MonoBehaviour
{
    [Header("Tap Detection Settings")]
    [SerializeField] private float maxTapThreshold = 50f;
    [SerializeField] private float maxTapTime = 0.2f;
    
    [Header("Tap Area")]
    public RectTransform tapArea; // The UI area where taps are detected

    [Header("Multitouch")]
    public bool enableMultiTouch = false; // Toggle from the Inspector

    [Header("Events")]
    public UnityEvent<Vector2> OnTap;

    private Vector2 initialTouchPosition;
    private float touchStartTime;
    private bool isTouching;

    private void Update()
    {
        DetectTap();
    }

    private void DetectTap()
    {
        if (enableMultiTouch && Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                HandleTouch(touch);
            }
        }
        else if (Input.touchCount > 0)
        {
            HandleTouch(Input.GetTouch(0));
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouch(Touch touch)
    {
        if (!IsTouchOnTapArea(touch.position)) return;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                isTouching = true;
                initialTouchPosition = touch.position;
                touchStartTime = Time.time;
                break;

            case TouchPhase.Ended:
                if (isTouching)
                {
                    isTouching = false;
                    float distance = Vector2.Distance(initialTouchPosition, touch.position);
                    float duration = Time.time - touchStartTime;

                    if (distance <= maxTapThreshold && duration <= maxTapTime)
                    {
                        OnTap?.Invoke(touch.position);
                    }
                }
                break;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsTouchOnTapArea(Input.mousePosition)) return;

            isTouching = true;
            initialTouchPosition = Input.mousePosition;
            touchStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0) && isTouching)
        {
            if (!IsTouchOnTapArea(Input.mousePosition)) return;

            isTouching = false;
            float distance = Vector2.Distance(initialTouchPosition, Input.mousePosition);
            float duration = Time.time - touchStartTime;

            if (distance <= maxTapThreshold && duration <= maxTapTime)
            {
                OnTap?.Invoke(Input.mousePosition);
            }
        }
    }

    private bool IsTouchOnTapArea(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = screenPosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (results.Count > 0 && results[0].gameObject == tapArea.gameObject)
            {
                return true;
            }
        }
        return false;
    }
}