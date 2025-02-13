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
    public GameObject playerLookAt;
    [Header("Events")]
    public UnityEvent onTutorialStart;
    public UnityEvent onButtonsTutorialEnd;
    public UnityEvent onTutorialEnd;
    private void OnEnable() 
    {
        TimelineManager.Instance.onComplete.AddListener(StartIfCan);    
    }
    private void StartIfCan()
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
    private void StartTutorial()
    {
        onTutorialStart?.Invoke();
        Invoke(nameof(RotatePlayer),1f);
        Invoke(nameof(ActiveTutorialPanel),2f);
        //SetTutorialObjectsActive(true); // Enable tutorial UI elements
    }
    public void RotatePlayer()
    {
        PlayerController.Instance.SetLookAt(playerLookAt.transform);
    }
    public void ActiveTutorialPanel()
    {
        SetTutorialObjectsActive(true);
    }
    public void EndButtonsTutorial()
    {
        onButtonsTutorialEnd?.Invoke();
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
