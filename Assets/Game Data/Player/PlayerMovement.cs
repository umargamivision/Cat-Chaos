using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    private CharacterController _characterController;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void HandleMovement(PlayerPhysics physics)
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = transform.right * input.x + transform.forward * input.z;

        if (physics.IsGrounded && Input.GetButtonDown("Jump"))
        {
            physics.AddJumpForce(jumpHeight);
        }
        _characterController.Move(move * movementSpeed * Time.deltaTime);
    }
    
}