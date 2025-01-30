using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    public SpecialItemType specialItemType;
    public int reactiveTimer = 30;
    public Collider itemCollider;
    public GameObject itemModel;
    public void GrabClick()
    {
        string placementName = specialItemType switch
        {
            SpecialItemType.Bee => "Bee_env",
            SpecialItemType.Fire => "Fire_env",
            SpecialItemType.Electric => "Gun_env",
            _ => throw new ArgumentOutOfRangeException()
        };
        AdsManager.ShowRewardedAd(OnGrabSuccess, placementName);
    }
    public void OnGrabSuccess()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        SpecialOffer.Instance.GrabItem(specialItemType);
        GetComponent<Collider>().enabled = true;
        StartCoroutine(ReactiveAfter(reactiveTimer));
    }
    IEnumerator ReactiveAfter(int seconds)
    {
        itemModel.SetActive(false);
        itemCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        itemModel.SetActive(true);
        itemCollider.enabled = true;
    }
}
