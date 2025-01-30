using Cinemachine;
using Ommy.Singleton;
using UnityEngine;
using DG.Tweening;
using Ommy.FadeSystem;
using System.Collections;
using System.ComponentModel.Design;
using System;
using Ommy.Audio;
public class PlayerController : Singleton<PlayerController>
{
    public StateManager stateManager;
    public CinemachineVirtualCamera playerCam;
    public FirstPersonController firstPersonController;
    public VaccumCleaner vaccumCleaner;
    public Grabber grabber;
    public IntractableDetector intractableDetector;
    public Transform grannyAttackLookPoint;
    public bool gotoGrannyHand;
    public Transform grannyCatchPoint;
    public Transform spawnPoint;
    [Header("Special Weapons")]
    public ElectricGun electricGun;
    public BeeComb beeComb;
    public FireSpray fireSpray;
    private void OnEnable()
    {
        InputManager.Instance.inputEvents.Find((f) => f.inputType == InputType.Attack).OnInvoke.AddListener(Attack);
        InputManager.Instance.inputEvents.Find((f) => f.inputType == InputType.Grab).OnInvoke.AddListener(Grab);
    }
    public void InputShootSpecialWeapon(Vector2 vector2)
    {
        if (electricGun.gameObject.activeSelf)
        {
            electricGun.Shoot();
        }
        if (fireSpray.gameObject.activeSelf)
        {
            fireSpray.Shoot();
        }
    }
    public void ActivateSpecialWeapon(SpecialItemType specialItemType)
    {
        switch (specialItemType)
        {
            case SpecialItemType.Electric:
                electricGun.SetActive(true);
                break;
            case SpecialItemType.Bee:
                beeComb.SetActive(true);
                break;
            case SpecialItemType.Fire:
                fireSpray.SetActive(true);
                break;
        }
    }
    public void ShootingSpecialWeapon(SpecialItemType specialItemType)
    {
        if (intractableDetector.currentDetectedObject == null) return;
        if (intractableDetector.currentDetectedObject.transform == GamePlayManager.Instance.grannyController.transform)
        {
            GamePlayManager.Instance.ShootGranny(specialItemType);
        }
    }
    public void Respawn()
    {
        firstPersonController.enabled = true;
        SetPositionAndRotation(spawnPoint);
        playerCam.transform.localEulerAngles = Vector3.right * 135;
        ScreenFader.Instance.FadeIn();
    }
    public void SetPositionAndRotation(Transform target)
    {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }
    public void SetLookAt(Transform target)
    {
        LookAt(target);
    }
    public void LookAt(Transform target)
    {
        firstPersonController.enabled = false;
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        //playerCam.transform.parent.localRotation = Quaternion.Euler(-50, 0, 0);
        playerCam.transform.parent.DOLocalRotate(Vector3.right*-50,0.5f);
        //transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        transform.DORotate(Vector3.up*rotation.eulerAngles.y,0.5f).OnComplete(()=>
        {
           firstPersonController.enabled = true;
        });
    }
    IEnumerator TimerAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
    public void Attack()
    {
        stateManager.ChangeState(stateManager.attack);
    }
    public void Grab()
    {
        grabber.OnGrab();
    }
    public void GrannyCatch()
    {
        AudioManager.Instance?.PlaySFX(SFX.PlayerDeath);
        firstPersonController.enabled = false;

        // Change Aim to "Hard Look At"
        SetCameraAimToHardLookAt();

        // Set LookAt target
        playerCam.LookAt = grannyAttackLookPoint;

        // Move the player to the granny's catch point
        if (gotoGrannyHand)
        {
            transform.DOMove(grannyCatchPoint.position, 1f).OnComplete(() =>
            {
                Debug.Log("Granny Catch completed!");
            });
        }
    }
    public void SetupVacuumCleaner(bool active)
    {
        vaccumCleaner.gameObject.SetActive(active);
    }
    private void SetCameraAimToHardLookAt()
    {
        var aim = playerCam.GetCinemachineComponent<CinemachineComposer>();
        if (aim != null)
        {
            // Remove Composer (Soft Aim) and replace with Hard Look At
            Destroy(aim);
        }

        // Add Hard Look At component
        playerCam.AddCinemachineComponent<CinemachineHardLookAt>();
    }
    public void TakeGrannyDamage()
    {
        //firstPersonController.enabled = false;
        playerCam.m_LookAt = null;
        transform.transform.DORotate(new Vector3(0, 0, 90), 0.5f).OnComplete(() =>
        {
            if(deathAndRespawnCoroutine!=null)
            {
                StopCoroutine(deathAndRespawnCoroutine);
            }
            deathAndRespawnCoroutine = StartCoroutine(DeathAndRespawn());
        });
        Debug.Log("Damage taken from granny");
    }
    public Coroutine deathAndRespawnCoroutine;
    public IEnumerator DeathAndRespawn()
    {
        yield return ScreenFader.Instance.FadeOut();
        yield return new WaitForSeconds(1);
        Respawn();
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var tearObj = hit.gameObject.GetComponent<ClothTear>();
        if(tearObj!=null)
        {
            if(stateManager.currentState==stateManager.attack)
            {
                tearObj.TearIt();
            }
        }
    }
}