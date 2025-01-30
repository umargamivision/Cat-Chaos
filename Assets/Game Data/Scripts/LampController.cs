using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LampController : MonoBehaviour, ISwitch
{
    public UnityEvent onOpen, onClose;
    public bool isOn = true;
    public void Close()
    {
        isOn = false;
        Debug.Log("Lamp is off");
        onClose.Invoke();
    }

    public void Open()
    {
        isOn = true;
        Debug.Log("Lamp is on");
        onOpen.Invoke();
    }

    public void Toggle()
    {
        if (isOn)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
