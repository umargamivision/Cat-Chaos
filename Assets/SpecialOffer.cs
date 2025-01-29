using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpecialOffer : MonoBehaviour
{
    public bool canShowOffer;
    public bool readyToShow;
    public SpecialItemType specialItemType;
    public float showEvery = 5f;
    public GameObject offerPanel;
    public GameObject[] specialOfferObjs;
    public UnityEvent<SpecialItemType> onAvailOffer;
    public void Start()
    {
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
        if(canShowOffer && readyToShow)
        {
            readyToShow = false;
            ShowSpecialOffer(specialItemType);
        }
    }
    public void ShowSpecialOffer(SpecialItemType specialItemType)
    {
        if(canShowOffer)
        {
            Time.timeScale = 0;
            StopCoroutine(coroutine);
            offerPanel.SetActive(true);
            specialOfferObjs.ToList().ForEach(x => x.SetActive(false));
            specialOfferObjs[(int)specialItemType].SetActive(true);
        }
    }
    public void AvailOffer()
    {
        Time.timeScale = 1;
        coroutine = StartCoroutine(ShowSpecialOffer());
        offerPanel.SetActive(false);
        onAvailOffer.Invoke(specialItemType);
        Debug.Log("Special Offer Availed");
    }
    public void CancelOffer()
    {
        Time.timeScale = 1;
        coroutine = StartCoroutine(ShowSpecialOffer());
        offerPanel.SetActive(false);
        Debug.Log("Special Offer Cancelled");
    }
}
    public enum SpecialItemType
    {
        Bee,
        Fire,
        Electric,
    }