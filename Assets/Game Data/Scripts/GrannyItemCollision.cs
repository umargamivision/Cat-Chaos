using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrannyItemCollision : MonoBehaviour
{
    public UnityEvent<ItemType> onBallHit;
    public void CollideWith(ItemType itemType)
    {
        if(itemType == ItemType.ball)
        {
            onBallHit.Invoke(itemType);
        }
    }
}
