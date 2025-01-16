using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerLook _look;
    private PlayerPhysics _physics;

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
        _physics = GetComponent<PlayerPhysics>();
    }

    void Update()
    {
        _look.HandleLook();
        _physics.ApplyGravity();
        _movement.HandleMovement(_physics);
    }
}