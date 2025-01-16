using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;
    private CharacterController _characterController;
    public bool IsGrounded;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void ApplyGravity()
    {
        IsGrounded = _characterController.isGrounded;

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset velocity when grounded
        }

        velocity.y += gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }

    public void AddJumpForce(float jumpHeight)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}