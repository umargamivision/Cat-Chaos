using Cinemachine;
using Ommy.Singleton;
using UnityEngine;
using DG.Tweening;
using Ommy.FadeSystem;
using System.Collections;
public class PlayerController : Singleton<PlayerController>
{
    public StateManager stateManager;
    public CinemachineVirtualCamera playerCam;
    public FirstPersonController firstPersonController;
    public Grabber grabber;
    public Transform grannyAttackLookPoint;
    public bool gotoGrannyHand;
    public Transform grannyCatchPoint;
    public Transform spawnPoint;
    private void OnEnable()
    {
        InputManager.Instance.inputEvents.Find((f) => f.inputType == InputType.Attack).OnInvoke.AddListener(Attack);
        InputManager.Instance.inputEvents.Find((f) => f.inputType == InputType.Grab).OnInvoke.AddListener(Grab);
    }
    public void Respawn()
    {
        firstPersonController.enabled = true;
        transform.SetPositionAndRotation(spawnPoint.position,spawnPoint.rotation);
        ScreenFader.Instance.FadeIn();
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
        transform.transform.DORotate(new Vector3(0, 0, 90), 0.5f).OnComplete(()=>
        {
            StartCoroutine(DeathAndRespawn());
        });
        Debug.Log("Damage taken from granny");
    }
    public IEnumerator DeathAndRespawn()
    {
        yield return ScreenFader.Instance.FadeOut();
        yield return new WaitForSeconds(1);
        Respawn();
    }
}