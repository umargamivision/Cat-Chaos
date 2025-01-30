using UnityEngine;
using Cursor = UnityEngine.Cursor;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public StateManager stateManager;
    public bool lockCurser;
    [SerializeField]
    private float m_MouseSensitivity = 100f;
    [SerializeField]
    private float m_MovementSpeed = 5f;
    [SerializeField]
    private float m_JumpHeight = 2f;
    [SerializeField]
    private float m_Gravity = -9.81f;
    [SerializeField]
    private Transform m_PlayerCamera = null;
    [SerializeField]
    private bool m_MoveWithMouse = true;
    public Vector2 lookClamp = new Vector2(-90, 90);
    private CharacterController m_CharacterController;
    private float m_XRotation = -50;
    private Vector3 m_Velocity;
    private bool m_IsGrounded;
    private void OnEnable()
    {
        InputManager.Instance.inputEvents.Find((f) => f.inputType == InputType.Jump).OnInvoke.AddListener(Jump);
    }
    void Start()
    {
#if ENABLE_INPUT_SYSTEM
        Debug.Log("The FirstPersonController uses the legacy input system. Please set it in Project Settings");
        m_MoveWithMouse = false;
#endif
        if (m_MoveWithMouse)
        {
            Cursor.lockState = lockCurser ? CursorLockMode.Locked : CursorLockMode.Confined;
        }
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Look();
        if (Move())
        {
            stateManager.ChangeState(stateManager.walk);
        }
        else
        {
            stateManager.ChangeState(stateManager.idle);
        }
    }

    private void Look()
    {
        Vector2 lookInput = GetLookInput();
        //if(lookInput.x==0 || lookInput.y==0) return;

        m_XRotation -= lookInput.y;
        m_XRotation = Mathf.Clamp(m_XRotation, lookClamp.x, lookClamp.y);

        m_PlayerCamera.localRotation = Quaternion.Euler(m_XRotation, 0, 0);
        transform.Rotate(Vector3.up * lookInput.x, Space.World);
    }
    public void LookAt(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        m_PlayerCamera.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }
    private bool Move()
    {
        bool isMoving;
        // Check if the player is grounded
        m_IsGrounded = m_CharacterController.isGrounded;
        if (m_IsGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -2f; // Slight negative value to keep grounded state consistent
        }

        // Get movement input
        Vector3 movementInput = GetMovementInput();
        isMoving = movementInput.magnitude > 0;
        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.z;

        m_CharacterController.Move(move * m_MovementSpeed * Time.deltaTime);

        // Jump logic
        //Jump();

        // Apply gravity
        m_Velocity.y += m_Gravity * Time.deltaTime;

        // Apply velocity to the character controller
        m_CharacterController.Move(m_Velocity * Time.deltaTime);
        return isMoving;
    }
    public void Jump()
    {
        if (m_IsGrounded)
        {
            stateManager.ChangeState(stateManager.jump);
            m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
        }
    }
    private Vector2 GetLookInput()
    {
        float mouseX = 0;
        float mouseY = 0;
        if (m_MoveWithMouse)
        {
            mouseX = InputManager.Instance.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;
            mouseY = InputManager.Instance.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;
        }
        return new Vector2(mouseX, mouseY);
    }

    private Vector3 GetMovementInput()
    {
        float x = 0;
        float z = 0;
        if (m_MoveWithMouse)
        {
            x = InputManager.Instance.GetAxis("Horizontal");
            z = InputManager.Instance.GetAxis("Vertical");
        }

        return new Vector3(x, 0, z);
    }
}