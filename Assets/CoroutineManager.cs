using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;
    private void Awake() 
    {
        Instance = this;
    }
    public Coroutine StartCor(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
