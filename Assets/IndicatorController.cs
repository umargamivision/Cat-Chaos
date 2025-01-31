using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    public bool offWhenPlayerClose = true;
    public float closeDistanceRequired=1f;
    public float distanceToPlayer;
    private void Update() 
    {
        // i want to add intervals to slow down the update calls
        if(Time.frameCount%10!=0) return;
        if (offWhenPlayerClose)
        {
            distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
            if (distanceToPlayer <= closeDistanceRequired)
            {
                gameObject.SetActive(false);
            }
        }    
    }
}
