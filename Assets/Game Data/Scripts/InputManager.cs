using System;
using System.Collections.Generic;
using Ommy.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{
    [Header("Input Settings")]       // Reference to the joystick component
    public List<InputEvent> inputEvents = new List<InputEvent>();

    /// <summary>
    /// Gets axis input based on the current input method (mobile or keyboard).
    /// </summary>
    public float GetAxis(string axis)
    {
       return Input.GetAxis(axis);
    }

    private void OnEnable()
    {
        // Bind UI buttons at the start
        foreach (var inputEvent in inputEvents)
        {
            inputEvent.BindButton();
        }
    }

    private void Update()
    {
        // Check for keyboard or button inputs each frame
        foreach (var inputEvent in inputEvents)
        {
            inputEvent.CheckInput();
        }
    }

    [Serializable]
    public class InputEvent
    {
        [Header("Input Configuration")]
        public InputType inputType;      // Custom enum for input categories
        public Button button;           // UI Button reference
        public KeyCode keyCode;         // Keyboard key for the action
        public UnityEvent OnInvoke;     // Events to invoke when the input is triggered

        /// <summary>
        /// Binds the UI button's click event to the action.
        /// </summary>
        public void BindButton()
        {
            if (button != null)
            {
                button.onClick.AddListener(() => OnInvoke.Invoke());
            }
        }

        /// <summary>
        /// Checks both keyboard and UI button inputs for triggering the action.
        /// </summary>
        public void CheckInput()
        {
            // Check for keyboard input
            if (Input.GetKeyDown(keyCode))
            {
                OnInvoke.Invoke();
            }
        }
    }
}

public enum InputType
{
    Jump,
    Attack,
    Grab,
    Pause
}