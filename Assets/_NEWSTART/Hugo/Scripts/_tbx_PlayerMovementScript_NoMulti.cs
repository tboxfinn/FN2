using TMPro;
using UnityEngine;

public class _tbx_PlayerMovementScript_NoMulti : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float maxYSpeed;

    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Stun")]
    public float stunDuration;
    public bool isStunned;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    //find the animator component attached to the GameObject you are intending to animate.
    public Animator anim;

    [Header("Zone Check")]
    public LayerMask whatIsZone;
    bool isInZone;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        Walking,
        Sprinting,
        Idle,
        Air,
        UsingHabilidad
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        /*if (IsOwner)
        {
            GetComponent<NetworkTransform>().enabled = true;
        }
        else
        {
            GetComponent<NetworkTransform>().enabled = false;
        }*/
    }

    void Update()
    {
        if(_alx_GameManager.singleton.currentGameState == GameStates.inGame){
            //Ground Check
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            //Show the raycast on the scene view
            Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);

            //Zone Check
            isInZone = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsZone);
            Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);
            if (isInZone)
            {
                Debug.Log("Est� en la zona");
            }


            MyInput();
            SpeedControl();
            StateHandler();

            //Manejar el drag
            if (isGrounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0f;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When jump key is pressed
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded || Input.GetKey(jumpKey) && readyToJump && isInZone)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void StateHandler()
    {
        if (isGrounded || isInZone)
        {
            float velocityMagnitude = rb.velocity.magnitude;

            if (velocityMagnitude < sprintSpeed && velocityMagnitude >= walkSpeed)
            {
                //Mode - Walking
                state = MovementState.Walking;
                anim.SetBool("isWalking", true);
            }
            else if (velocityMagnitude >= sprintSpeed)
            {
                //Mode - Sprinting
                state = MovementState.Sprinting;  
            }
            else if(velocityMagnitude < walkSpeed)
            {
                //Mode - Idle
                state = MovementState.Idle;
                anim.SetBool("isWalking", false);
            }
        }
        else
        {
            //Mode - Air
            state = MovementState.Air; 
        }
    }

    private void MovePlayer()
    {
        //Calcular direccion de mov
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //On ground
        else if(isGrounded || isInZone)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //In air
        else if (!isGrounded && !isInZone)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        else if (OnSlope() && !isGrounded && !isInZone)
        {
            rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        //gravedad OFF en slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //Limitar Vel en slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Aqui limito la velocidad maxima
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        //limit y vel
        if (maxYSpeed !=0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
        
    }

    private void Jump()
    {
        exitingSlope = true;
        anim.SetTrigger("OnJump");
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_ySpeed;
    public TextMeshProUGUI text_mode;
}
