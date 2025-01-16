using System.Collections;
using System.Collections.Generic;
using Ommy.Singleton;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    [SerializeField] private bool lockCursor = true;

    private new void Awake()
    {
        base.Awake(); // Ensure Singleton initialization if needed.
        Application.targetFrameRate = 30;
    }

    private void Start()
    {
        SetCursorLockState(lockCursor);
    }

    /// <summary>
    /// Sets the cursor lock state based on the given value.
    /// </summary>
    /// <param name="shouldLock">True to lock the cursor, false to unlock.</param>
    public void SetCursorLockState(bool shouldLock)
    {
        ControlFreak2.CFCursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        ControlFreak2.CFCursor.visible = !shouldLock;
    }

    /// <summary>
    /// Toggles the cursor lock state at runtime.
    /// </summary>
    public void ToggleCursorLockState()
    {
        lockCursor = !lockCursor;
        SetCursorLockState(lockCursor);
    }
}    
