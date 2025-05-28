using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public float mouseSensitivity = 2f; // Smooth sensitivity
    public Camera playerCamera;

    private Rigidbody rb;
    private float verticalLookRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!IsOwner)
        {
            playerCamera.enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        // Movement (WASD)
        float moveInput = Input.GetAxis("Vertical");   // W/S for forward/back
        float turnInput = Input.GetAxis("Horizontal"); // A/D for rotate left/right

        Vector3 move = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.position += move;

        float rotation = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    private void Update()
    {
        if (!IsOwner) return;

        // Rotate camera vertically with clamp
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }
}
