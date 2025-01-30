using System;
using System.Collections;
using System.Linq;
using Ommy.Audio;
using Ommy.SaveData;
using Ommy.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SpecialOffer : Singleton<SpecialOffer>
{
    public AudioClip offerClip;
    public bool canShowOffer;
    public bool readyToShow;
    public SpecialItemType specialItemType;
    public float showEvery = 5f;
    [TextArea(3, 10)]
    public string fireDis, electricDis, beeDis;
    public TMP_Text offerText;
    public GameObject offerPanel;
    public GameObject[] specialOfferObjs;
    public UnityEvent<SpecialItemType> onAvailOffer;

    public void RestartTimer()
    {
        showEvery = AdsManager.RC_Inter_Ingame_popup;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(ShowSpecialOffer());
    }
    public Coroutine coroutine;
    public IEnumerator ShowSpecialOffer()
    {
        while (true)
        {
            yield return new WaitForSeconds(showEvery);
            int randomIndex = UnityEngine.Random.Range(0, specialOfferObjs.Length);
            specialItemType = (SpecialItemType)randomIndex;
            readyToShow = true;
        }
    }
    private void Update()
    {
        if (canShowOffer && readyToShow)
        {
            readyToShow = false;
            ShowSpecialOffer(specialItemType);
        }
    }
    public void ShowSpecialOffer(SpecialItemType specialItemType)
    {
        if (canShowOffer)
        {
            Time.timeScale = 0;
            StopCoroutine(coroutine);
            offerText.text = specialItemType switch
            {
                SpecialItemType.Bee => beeDis,
                SpecialItemType.Fire => fireDis,
                SpecialItemType.Electric => electricDis,
                _ => throw new ArgumentOutOfRangeException()
            };
            AudioManager.Instance?.PlaySFX(offerClip);
            offerPanel.SetActive(true);
            specialOfferObjs.ToList().ForEach(x => x.SetActive(false));
            specialOfferObjs[(int)specialItemType].SetActive(true);
        }
    }
    public void AvailOffer()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        string placementName = specialItemType switch
        {
            SpecialItemType.Bee => "Bee_ingame_popup",
            SpecialItemType.Fire => "Fire_ingame_popup",
            SpecialItemType.Electric => "Gun_ingame_popup",
            _ => throw new ArgumentOutOfRangeException()
        };
        AdsManager.ShowRewardedAd(AvailOfferSuccess, placementName);
    }
    public void GrabItem(SpecialItemType specialItemType)
    {
        GamePlayManager.Instance.AvailSpecialOffer(specialItemType);
        onAvailOffer.Invoke(specialItemType);
    }
    public void AvailOfferSuccess()
    {
        Time.timeScale = 1;
        coroutine = StartCoroutine(ShowSpecialOffer());
        offerPanel.SetActive(false);
        GamePlayManager.Instance.AvailSpecialOffer(specialItemType);
        onAvailOffer.Invoke(specialItemType);
        Debug.Log("Special Offer Availed");
    }
    public void CancelOffer()
    {
        AudioManager.Instance?.PlaySFX(SFX.Click);
        ShowInterIfCan();
        Time.timeScale = 1;
        coroutine = StartCoroutine(ShowSpecialOffer());
        offerPanel.SetActive(false);
        Debug.Log("Special Offer Cancelled");
    }
    public void ShowInterIfCan()
    {
        int levelNo = SaveData.Instance.Level;
        bool all = AdsManager.RC_Inter_Ingame_popup == 0;
        bool odd = AdsManager.RC_Inter_Ingame_popup == 1;
        bool even = AdsManager.RC_Inter_Ingame_popup == 2;
        if (levelNo % 2 == 0 && even || levelNo % 2 != 0 && odd || all)
        {
            AdsManager.ShowInterstitilAd("Inter_Ingame_popup");
        }
    }
}
public enum SpecialItemType
{
    Bee,
    Fire,
    Electric,
}