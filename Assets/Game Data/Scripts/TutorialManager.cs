using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using ControlFreak2;
using Ommy.SaveData;

public class TutorialManager : MonoBehaviour
{
    [Header("Dependencies")]
    public InputManager inputManager;

    [Header("Tutorial UI Elements")]
    public List<GameObject> tutorialEnabledObjects,tutorialDisabledObjects; // List of UI elements like joystick, touchpad, etc.

    [Header("Look Settings")]
    public float lookRequirement = 1f;
    private float currentLook;
    private bool lookTutCompleted;

    [Header("Movement Settings")]
    public float moveRequirement = 1f;
    private float currentMove;
    private bool moveTutCompleted;

    [Header("Events")]
    public UnityEvent onTutorialStart;
    public UnityEvent onLooked;
    public UnityEvent onMoved;
    public UnityEvent onTutorialEnd;

    private void Start()
    {
        if (SaveData.Instance.GameTutorial)
        {
            StartTutorial();
        }
        else
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (!SaveData.Instance.GameTutorial)return;
        if (!moveTutCompleted)
        {
            if (CheckMoved())
            {
                moveTutCompleted = true;
                onMoved.Invoke();
            }
        }
        else if (!lookTutCompleted)
        {
            if (CheckLooked())
            {
                lookTutCompleted = true;
                onLooked.Invoke();
                EndMoveLookTutorial();
            }
        }
    }

    private void StartTutorial()
    {
        onTutorialStart?.Invoke();
        SetTutorialObjectsActive(true); // Enable tutorial UI elements
    }

    private bool CheckLooked()
    {
        currentLook += Mathf.Abs(inputManager.GetAxis("Mouse X"));
        return currentLook >= lookRequirement;
    }

    private bool CheckMoved()
    {
        currentMove += Mathf.Abs(inputManager.GetAxis("Vertical"));
        return currentMove >= moveRequirement;
    }

    public void EndMoveLookTutorial()
    {
        SetTutorialObjectsActive(false); // Disable tutorial UI elements
        //enabled = false; // Disable script when tutorial is complete
    }
    public void EndTutorial()
    {
        onTutorialEnd?.Invoke();
        SaveData.Instance.GameTutorial = false;
        SaveSystem.SaveProgress();
    }
    private void SetTutorialObjectsActive(bool isActive)
    {
        foreach (var obj in tutorialEnabledObjects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
        foreach (var item in tutorialDisabledObjects)
        {
            if(item != null)
            {
                item.SetActive(!isActive);
            }
        }
    }
}
