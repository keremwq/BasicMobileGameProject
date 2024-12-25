using UnityEngine;

public class PlayerLocalMotion : MonoBehaviour
{
    LayerManager layerManager;
    InputControl inputControl;
    AnimatorManager animatorManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float intAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.2f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting = false;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speed")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -3;
    private void Awake()
    {
        layerManager = GetComponent<LayerManager>();
        inputControl = GetComponent<InputControl>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animatorManager = GetComponent<AnimatorManager>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (layerManager.isInteracting)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
        {
            return;
        }
        moveDirection = cameraObject.forward * inputControl.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputControl.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputControl.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }


        Vector3 movementVelocity = moveDirection;
        //playerRigidbody.linearVelocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
        {
            return;
        }
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputControl.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputControl.horizontalInput;

        if (targetDirection != Vector3.zero)
        {
            targetDirection.Normalize();
            targetDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
        }
    }

    private void HandleFallingAndLanding()
    {

        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        rayCastHeightOffset = 0.5f; // Karakter boyunun yarısı civarı
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!layerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            intAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * intAirTimer);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f, groundLayer) && !isJumping)
        {
            Debug.DrawRay(transform.position, hit.point, Color.green);
            if (!isGrounded && layerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", false);
            }

            intAirTimer = 0;
            isGrounded = true;
            layerManager.isInteracting = false;
            //playerRigidbody.linearVelocity = Vector3.zero;
        }
        else
        {
            isGrounded = false;
        }

    }

    public void HandleJumping()
    {
        isJumping = true;
        Vector3 direct = moveDirection;
        direct.y = jumpHeight * gravityIntensity;
        playerRigidbody.linearVelocity = direct;
        Debug.Log(playerRigidbody.linearVelocity);
        isJumping = false;
    }

    void FixedUpdate()
    {
        Debug.Log(playerRigidbody.linearVelocity);
    }
}
