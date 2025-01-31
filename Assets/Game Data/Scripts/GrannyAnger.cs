using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GrannyAnger : MonoBehaviour
{
    public int angerLevel;
    public int angerThresould;
    public Image fill;
    public UnityEvent onGrannyAnger;
    private void OnEnable() 
    {
        fill.fillAmount=0;    
        angerLevel=0;    
    }
    public void OnHitGranny()
    {
        angerLevel++;
        fill.DOFillAmount((float)angerLevel/angerThresould,0.5f);
        if(angerLevel>=angerThresould) 
        {
            onGrannyAnger.Invoke();
        }
    }
}
