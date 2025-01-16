using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0f;

    public void HandleLook()
    {
        float mouseX = ControlFreak2.CF2Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = ControlFreak2.CF2Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX, Space.World);
    }
}