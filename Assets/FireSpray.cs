using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FireSpray : MonoBehaviour
{
    public Vector3 activePos, deactivePos;
    public float shootingTime=1f;
    public float deactiveAfterShoot=1f;
    public ParticleSystem vfx;
    public UnityEvent<SpecialItemType> onShoot;
    [InspectorButton]
    public void Shoot()
    {
        vfx.Play(true);
        StartCoroutine(DeactivateAfterShoot());
        StartCoroutine(Shooting());
    }
    IEnumerator Shooting()
    {
        float _shootingTime= this.shootingTime;
        while(_shootingTime>0)
        {
            _shootingTime-=Time.deltaTime;
            yield return null;
            PlayerController.Instance.ShootingSpecialWeapon(SpecialItemType.Fire);
        }
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
