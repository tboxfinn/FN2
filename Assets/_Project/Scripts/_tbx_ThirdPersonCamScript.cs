using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class _tbx_ThirdPersonCamScript : NetworkBehaviour
{
    // Keybinds for switching camera styles
    [Header ("Keytbinds")]
    public KeyCode KeyCamera1;
    public KeyCode KeyCamera2;
    public KeyCode KeyCamera3;

    // References to player objects
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody playerRb;

    // Rotation speed for player object
    public float rotationSpeed;

    // Reference to combat look-at target
    public Transform combatLookAt;

    // Camera style game objects
    [Header("Camera Style")]
    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    // Current camera style
    public CameraStyle currentStyle;

    // Camera style enum
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    private void Start()
    {
        // Lock cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the priority of the free look components if the player is the owner
        if(IsOwner)
        {
            // Get the free look component from each camera
            var freeLookCam = thirdPersonCam.GetComponentInChildren<CinemachineFreeLook>();
            var combatFreeLookCam = combatCam.GetComponentInChildren<CinemachineFreeLook>();
            var topDownFreeLookCam = topDownCam.GetComponentInChildren<CinemachineFreeLook>();

            // Set the priority of the free look components
            freeLookCam.Priority = 10;
            combatFreeLookCam.Priority = 10;
            topDownFreeLookCam.Priority = 10;
        }
        
        if (!IsOwner)
        {
            // Get the free look component from each camera
            var freeLookCam = thirdPersonCam.GetComponentInChildren<CinemachineFreeLook>();
            var combatFreeLookCam = combatCam.GetComponentInChildren<CinemachineFreeLook>();
            var topDownFreeLookCam = topDownCam.GetComponentInChildren<CinemachineFreeLook>();

            // Set the priority of the free look components to 0
            freeLookCam.Priority = 0;
            combatFreeLookCam.Priority = 0;
            topDownFreeLookCam.Priority = 0;
        }
        
    }

    void Update()
    {
        // Only update if the player is the owner
        if (!IsOwner)
        {
            return;
        }
        
        // Switch camera style based on keybinds
        if (Input.GetKeyDown(KeyCamera1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCamera2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCamera3)) SwitchCameraStyle(CameraStyle.Topdown);

        // Rotate orientation to face player
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir;

        // Rotate player object based on camera style
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    // Switch camera style based on the selected style
    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        // Disable all camera style game objects
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        // Enable the selected camera style game object
        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);   

        // Set the current camera style
        currentStyle = newStyle;
    }
}