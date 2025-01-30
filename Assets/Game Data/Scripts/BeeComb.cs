using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BeeComb : MonoBehaviour
{
    public Grabber grabber;
    public Item itemPrefab;
    public Item item;
    [InspectorButton]
    public void SetActive(bool active)
    {
        item = Instantiate(itemPrefab,transform.position,transform.rotation);
        grabber.OnGrab(item.grabbable);
        if(active)gameObject.SetActive(active);
    }
}
