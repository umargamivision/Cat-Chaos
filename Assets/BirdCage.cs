using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class BirdCage : MonoBehaviour
{
    public Door door;
    public GameObject cageBird;
    public UnityEvent onBirdCatch;
    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.TryGetComponent<Item>(out Item item))
        {
            if(item.itemType==ItemType.bird)
            {
                OnBirdCatch(item);
            }
        }    
    }
    public void OnBirdCatch(Item birdItem)
    {
        birdItem.gameObject.SetActive(false);
        cageBird.SetActive(true);
        door.Close();
        onBirdCatch.Invoke();
    }

}
