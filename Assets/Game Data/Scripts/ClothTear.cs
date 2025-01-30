using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClothTear : MonoBehaviour
{
    public GameObject tearObject;
    public UnityEvent onTear;
    public void TearIt()
    {
        onTear.Invoke();
        tearObject.SetActive(true);
    }
}
