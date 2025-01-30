using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using DG.Tweening;
using Ommy.Audio;
using UnityEngine;
using UnityEngine.Events;

public class ElectricGun : MonoBehaviour
{
    public Vector3 activePos, deactivePos;
    public float deactiveAfterShoot=1f;
    public ParticleSystem vfx;
    public AudioClip shootSound;
    public UnityEvent<SpecialItemType> onShoot;
    [InspectorButton]
    public void Shoot()
    {
        AudioManager.Instance?.PlaySFX(shootSound);
        vfx.Play(true);
        StartCoroutine(DeactivateAfterShoot());
        PlayerController.Instance.ShootingSpecialWeapon(SpecialItemType.Electric);
    }
    [InspectorButton]
    public void SetActive(bool active)
    {
        if(active)gameObject.SetActive(active);

        transform.DOLocalMove(active ? activePos : deactivePos, 0.5f).OnComplete(
        () =>
            gameObject.SetActive(active)
        );
    }
    private IEnumerator DeactivateAfterShoot()
    {
        yield return new WaitForSeconds(deactiveAfterShoot);
        SetActive(false);
    }
}
