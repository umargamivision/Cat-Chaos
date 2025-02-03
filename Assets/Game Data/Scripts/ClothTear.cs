using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClothTear : MonoBehaviour
{
    public GameObject tearObject;
    public AudioClip tearClip;
    public UnityEvent onTear;
    public UnityEvent onTearDelay;
    public void TearIt()
    {
        Invoke(nameof(OnTearDelay), 1f);
        AudioManager.Instance?.PlaySFX(tearClip);
        onTear.Invoke();
        tearObject.SetActive(true);
    }
    public void OnTearDelay()
    {
        onTearDelay.Invoke();
    }
}
