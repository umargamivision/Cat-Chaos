using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Ommy.Audio;
public class CurrencyCollectionAnimation : MonoBehaviour
{
    public float cashSpeed;
    public float radius;
    public GameObject cashCollectMidPoint;
    public GameObject cashCollectBar;
    public List<GameObject> cashObjects;
    public UnityEvent onCashCollect, onCashCollectedComplete;
    [InspectorButton]
    public void CollectCash()
    {
        StartCoroutine(AnimateCash());
    }
    IEnumerator AnimateCash()
    {
        AudioManager.Instance?.PlaySFX(SFX.FishReward);
        cashObjects.ForEach(f=>
        {
            f.SetActive(true);
            f.transform.position = cashCollectMidPoint.transform.position;
        });
        var _sequence = DOTween.Sequence();
        _sequence = DOTween.Sequence();

        foreach (var cash in cashObjects)
        {
            var spreadRadius = transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle * radius);
            _sequence.Join(cash.transform.DOMove(spreadRadius, UnityEngine.Random.Range(0.2f, cashSpeed)));
        }

        _sequence.OnComplete(
            () =>
            {
                print("sequence completed");
                foreach (var cash in cashObjects)
                {
                    cash.transform.DOMove(cashCollectBar.transform.position, UnityEngine.Random.Range(0.2f, 0.6f)).OnComplete(() =>
                    {
                        cash.gameObject.SetActive(false);
                        DOTween.Kill(cashCollectBar.transform);
                        cashCollectBar.transform.localScale=Vector3.one;
                        cashCollectBar.transform.DOScale(cashCollectBar.transform.localScale * 1.2f, 0.2f).OnComplete(() =>
                        {
                            cashCollectBar.transform.DOScale(1, 0.2f);
                        });
                        onCashCollect.Invoke();
                    });
                }
            });
        onCashCollectedComplete.Invoke();
        yield return new WaitForSeconds(1.5f);
    }
}