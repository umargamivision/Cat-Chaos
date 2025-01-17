using Cinemachine;
using Ommy.Singleton;
using UnityEngine;
public class PlayerController : Singleton<PlayerController>
{
    public CinemachineVirtualCamera playerCam;
    public FirstPersonController firstPersonController;
}