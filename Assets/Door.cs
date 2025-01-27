using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, ISwitch
{
    public bool hasKey = true;
    public bool isOpen = false;
    public bool localRotation;
    public bool triggerOtherDoor;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public Vector3 closeRotation = new Vector3(0, 0, 0);
    public UnityEvent onOpen, onClose;

    [InspectorButton]
    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    public void SetHasKey(bool value)
    {
        hasKey = value;
    }
    public void Open()
    {
        if (!hasKey)
        {
            Debug.Log("Cannot open the door without a key.");
            return;
        }

        // Add your door opening logic here
        if (!triggerOtherDoor) 
        {
            if(localRotation)transform.DOLocalRotate(openRotation, 1.0f); // Rotate door to open position
            else transform.DORotate(openRotation, 1.0f); // Rotate door to open position
        }
        Debug.Log("Door is opened");
        isOpen = true;
        onOpen.Invoke();
    }

    public void Close()
    {
        // Add your door closing logic here
        if (!triggerOtherDoor) 
        {
            if(localRotation)transform.DOLocalRotate(closeRotation, 1.0f); // Rotate door to close position
            else transform.DORotate(closeRotation, 1.0f); // Rotate door to close position
        }
        Debug.Log("Door is closed");
        isOpen = false;
        onClose.Invoke();
    }
}
public interface ISwitch
{
    public void Open();
    public void Close();
    public void Toggle();
}