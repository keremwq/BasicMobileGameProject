using UnityEngine;
using System.Collections;

public class PlayerLocalMotion : MonoBehaviour
{
    LayerManager layerManager;
    InputControl inputControl;
    AnimatorManager animatorManager;
    private Animator animator;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Animations")]
    public string currentAnimation;
    private int idleStatus;
    public bool changableAnimations = true;
    AnimationClip land;

    [Header("Stair Control")]
    [SerializeField] GameObject lowerDetect;
    [SerializeField] GameObject upperDetect;
    public float stepSmooth;

    [Header("Falling")]
    public float intAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public float maxDistance = 0.5f;
    public float minDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting = false;
    public bool isGrounded;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool cantMove = false;
<<<<<<< HEAD
    public bool isStair = false;
    private bool isDashing = false;
    private bool isRFalling = false;
=======
<<<<<<< HEAD
    public bool isStair = false;
    private bool isDashing = false;
    private bool isRFalling = false;
=======
    private bool isDashing = false;
>>>>>>> 0faebdf8a139479fe17335025d219e1d08de4359
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
    private string jumpOrFall = "Fall";

    [Header("Movement Speed")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump Properties")]
    public float jumpHeight = 3;
    public float jumpHeightFirst;
    public float jumpHeightSecond;
    public float gravityIntensity = -3;
    public int jumpCount;

    [Header("Dashing")]
    public float dashPower = 3;
    public float dashJumpPower;
    public float dashGroundPower;
    public float dashFallPower;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        layerManager = GetComponent<LayerManager>();
        inputControl = GetComponent<InputControl>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animatorManager = GetComponent<AnimatorManager>();
    }
    IEnumerator IdleStatus()
    {
        bool boolean = false;
        while (true)
        {
            yield return new WaitForSeconds(6);
            boolean = !boolean;
            animator.SetBool("idleBoolean", boolean);
        }
    }

    public void ResetIdleCoroutine()
    {
        StopCoroutine("IdleStatus");
        StartCoroutine("IdleStatus");
    }
    void Start()
    {
        StartCoroutine("IdleStatus");
    }

    void DebugGame()
    {
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    public void HandleAllMovement()
    {
        StairControl();
        HandleFallingAndLanding();
        if (!cantMove && !isDashing)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    private void HandleMovement()
    {
        /*if (isJumping)
        {
            return;
        }*/
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

        if (!isFalling && !isJumping)
        {
            if (moveDirection.x == 0 && moveDirection.z == 0)
            {
                ChangeAnimation("Idle 2");
            }
            else
            {
                ChangeAnimation("Local Motion");
            }
        }
        Vector3 movementVelocity = playerRigidbody.linearVelocity;
        movementVelocity.x = moveDirection.x;
        movementVelocity.z = moveDirection.z;

        if (isDashing)
        {

        }

        playerRigidbody.linearVelocity = movementVelocity;

        isDashing = false;
    }

    private void HandleRotation()
    {
        /*if (isJumping)
        {
            return;
        }*/
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
        if (isDashing)
        {
            return;
        }
        if (isJumping) // Eğer zıplıyorsa düşme kontrolünü atla
        {
            if (playerRigidbody.linearVelocity.y <= 0) // Yükseklik düşmeye başladığında
            {

                jumpOrFall = "Falling_jump";
                changableAnimations = true;
            }
            else
            {
                return; // Yukarı doğru hareket devam ediyor
            }
        }
        else
        {
            jumpOrFall = "Falled";
        }
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;

        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 3f, groundLayer))
        {
<<<<<<< HEAD
            if (hit.distance > minDistance)
            {
                ChangeAnimation(jumpOrFall);
                isRFalling = true;
            }

        }
        else
        {
            ChangeAnimation(jumpOrFall);
            isRFalling = true;
        }

        if (!isGrounded && isRFalling)
        {
=======
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
            if (!isJumping) { dashPower = dashFallPower; }
            intAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * intAirTimer);
<<<<<<< HEAD
=======
<<<<<<< HEAD
        }
        Debug.DrawRay(raycastOrigin, Vector3.down * maxDistance, Color.green);


        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 3f, groundLayer))
        {
            if (hit.distance > minDistance)
            {
                ChangeAnimation(jumpOrFall);
                isRFalling = true;
            }

        }
        else
        {
            ChangeAnimation(jumpOrFall);
            isRFalling = true;
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
        }
        Debug.DrawRay(raycastOrigin, Vector3.down * maxDistance, Color.green);

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, maxDistance, groundLayer)) // Ground Controller
        {
<<<<<<< HEAD
=======
=======
        }

        if (Physics.SphereCast(raycastOrigin, 0.3f, Vector3.down, out hit, 0.5f, groundLayer)) // Ground Controller
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.green);
>>>>>>> 0faebdf8a139479fe17335025d219e1d08de4359
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
            if (!isGrounded)
            {
                isJumping = false;
                jumpCount = 0;
                dashPower = dashGroundPower;
                changableAnimations = true;
                inputControl.canJump = false;
<<<<<<< HEAD
                cantMove = true;
                inputControl.CanJumpTrigger();
                if (/*!isStair &&*/ isRFalling)
                    ChangeAnimation("Land", 0.1f, 0.4f - 0.3f);
=======
<<<<<<< HEAD
=======
                inputControl.CanJumpTrigger();
                ChangeAnimation("Land", 0.2f, 0.4f - 0.3f);
>>>>>>> 0faebdf8a139479fe17335025d219e1d08de4359
                cantMove = true;
                inputControl.CanJumpTrigger();
                if (/*!isStair &&*/ isRFalling)
                    ChangeAnimation("Land", 0.2f, 0.4f - 0.3f);
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
                else
                    cantMove = false;
                isRFalling = false;
            }

            intAirTimer = 0;
            isFalling = false;
            isGrounded = true;
            layerManager.isInteracting = false;
            Vector3 currentVelocity = playerRigidbody.linearVelocity;
            currentVelocity.x = 0;
            currentVelocity.z = 0;
            playerRigidbody.linearVelocity = currentVelocity;
        }
        else
        {
            ChangeAnimation(jumpOrFall);
            isFalling = true;
            isGrounded = false;
        }
    }


    public void HandleJumping()
    {
        isGrounded = false;
        isJumping = true;
        dashPower = dashJumpPower;
        jumpCount++;
        if (jumpCount == 1)
        {
            jumpHeight = jumpHeightFirst;
        }
        else if (jumpCount == 2)
        {
            jumpHeight = jumpHeightSecond;
        }
        if (!(jumpCount > 2))
        {
            StopCoroutine("waitTillAnimationFinishesCoroutine");
            Vector3 jumpingVelocity = moveDirection;
            float gravityCalculate = Mathf.Sqrt(-2 * jumpHeight * gravityIntensity);
            jumpingVelocity.y = gravityCalculate;
            playerRigidbody.linearVelocity = jumpingVelocity;
        }
        if (!isDashing && jumpCount == 1)
        {
            inputControl.canJump = true;
            ChangeAnimation("Jump");
        }

        else if (!isDashing && jumpCount == 2)
        {
            changableAnimations = true;
            inputControl.canJump = false;
            ChangeAnimation("Double Jump");
        }
        else if (jumpCount > 2)
        {
            jumpCount = 0;
        }
        changableAnimations = false;
    }

    public void HandleDash()
    {
        StartCoroutine("Dashing");
        ChangeAnimation("Dashing");
        changableAnimations = false;
        isDashing = true;
    }

    IEnumerator Dashing()
    {
        float elapsed = 0f;
        float duration = 0.4f;
        float dashPowerCut = 0;
        while (elapsed < duration)
        {
            // Dash gücünü yumuşakça artır
            dashPowerCut = Mathf.Lerp(0f, dashPower, elapsed / duration);
            playerRigidbody.AddForce(dashPowerCut * transform.forward.normalized);
            elapsed += Time.deltaTime;
            changableAnimations = true;
            ChangeAnimation("Dashing", 0.4f);
            yield return null;
        }
        changableAnimations = true;
        isDashing = false;
<<<<<<< HEAD
=======
    }

    public void HandleJumping()
    {
        isJumping = true;
        dashPower = dashJumpPower;
        jumpCount++;
        if (jumpCount == 1)
        {
            jumpHeight = jumpHeightFirst;
        }
        else if (jumpCount == 2)
        {
            jumpHeight = jumpHeightSecond;
        }
        if (!(jumpCount > 2))
        {
            StopCoroutine("waitTillAnimationFinishesCoroutine");
            Vector3 jumpingVelocity = moveDirection;
            float gravityCalculate = Mathf.Sqrt(-2 * jumpHeight * gravityIntensity);
            jumpingVelocity.y = gravityCalculate;
            playerRigidbody.linearVelocity = jumpingVelocity;
        }
        if (!isDashing && jumpCount == 1)
        {
            inputControl.canJump = true;
            ChangeAnimation("Jump");
        }

        else if (!isDashing && jumpCount == 2)
        {
            changableAnimations = true;
            inputControl.canJump = false;
            ChangeAnimation("Double Jump");
        }
        else if (jumpCount > 2)
        {
            jumpCount = 0;
        }
        changableAnimations = false;
    }

    public void HandleDash()
    {
        StartCoroutine("Dashing");
        ChangeAnimation("Dashing");
        changableAnimations = false;
        isDashing = true;
    }

    IEnumerator Dashing()
    {
        float elapsed = 0f;
        float duration = 0.4f;
        float dashPowerCut = 0;
        while (elapsed < duration)
        {
            // Dash gücünü yumuşakça artır
            dashPowerCut = Mathf.Lerp(0f, dashPower, elapsed / duration);
            playerRigidbody.AddForce(dashPowerCut * transform.forward.normalized);
            elapsed += Time.deltaTime;
            changableAnimations = true;
            ChangeAnimation("Dashing", 0.4f);
            yield return null;
        }
        changableAnimations = true;
        isDashing = false;
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
    }

    private void ChangeAnimation(string animation, float crossFade = 0.2f, float seconds = 0)
    {
        if (isDashing)
        {
            animation = "Dashing";
        }

        if (currentAnimation != animation && changableAnimations)
        {
            Debug.Log(currentAnimation);
            currentAnimation = animation;
            animator.CrossFade(animation, crossFade);
        }
        if (seconds != 0)
            waitTillAnimationFinishes(seconds);
    }

    private void ChangeAnimatorBoolean(string boolToChange, bool value)
    {
        animator.SetBool(boolToChange, value);
    }

    public void waitTillAnimationFinishes(float seconds)
    {
        changableAnimations = false;
        StartCoroutine("waitTillAnimationFinishesCoroutine", seconds);
    }

    IEnumerator waitTillAnimationFinishesCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        changableAnimations = true;
    }

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
    private void StairControl()
    {
        RaycastHit lowerHit;
        Debug.Log(isStair);
        Debug.DrawRay(lowerDetect.transform.position, lowerDetect.transform.TransformDirection(-Vector3.forward) * 0.3f, Color.red);
        if (Physics.Raycast(lowerDetect.transform.position, lowerDetect.transform.TransformDirection(-Vector3.forward), out lowerHit, 0.3f, groundLayer) && inputControl.moveAmount > 0)
        {
            RaycastHit upperHit;
            Debug.DrawRay(lowerDetect.transform.position, lowerDetect.transform.TransformDirection(-Vector3.forward) * 0.4f, Color.blue);
            if (!Physics.Raycast(upperDetect.transform.position, transform.TransformDirection(-Vector3.forward), out upperHit, 0.4f, groundLayer))
            {
                IsStairCheck();
                if (!isStair)
                    playerRigidbody.MovePosition(transform.position + stepSmooth * Vector3.up);
            }
        }
<<<<<<< HEAD
    }

    IEnumerator IsStairCheck()
    {
        isStair = true;
        yield return new WaitForSeconds(0.5f);
        isStair = false;
=======
>>>>>>> d3c017c4f06d38b5dab8822abd57f3efcf07307a
    }

    IEnumerator IsStairCheck()
    {
        isStair = true;
        yield return new WaitForSeconds(0.5f);
        isStair = false;
    }
=======

>>>>>>> 0faebdf8a139479fe17335025d219e1d08de4359

    void FixedUpdate()
    {
        DebugGame();
        if (!isGrounded)
        {
            playerRigidbody.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }
}
