using Cinemachine;
using Ommy.Singleton;
using UnityEngine;
using DG.Tweening;
public class PlayerController : Singleton<PlayerController>
{

    public CinemachineVirtualCamera playerCam;
    public FirstPersonController firstPersonController;
    public bool lookAtGranny;
    public Transform grannyAttackLookPoint;
    public Transform grannyCatchPoint;
    public void LookAtToGranny()
    {

    }

    public void GrannyCatch()
    {
        firstPersonController.enabled = false;

        // Change Aim to "Hard Look At"
        SetCameraAimToHardLookAt();

        // Set LookAt target
        playerCam.LookAt = grannyAttackLookPoint;

        // Move the player to the granny's catch point
        transform.DOMove(grannyCatchPoint.position, 1f).OnComplete(() =>
        {
            Debug.Log("Granny Catch completed!");
        });
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
        Debug.Log("Damage taken from granny");
    }
}