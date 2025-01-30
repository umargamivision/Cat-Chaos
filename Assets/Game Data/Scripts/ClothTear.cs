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
    public void TearIt()
    {
        AudioManager.Instance?.PlaySFX(tearClip);
        onTear.Invoke();
        tearObject.SetActive(true);
    }
}
