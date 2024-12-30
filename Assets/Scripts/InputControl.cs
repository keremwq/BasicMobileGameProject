using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerControls playerControls;
    PlayerLocalMotion playerLocalMotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;

    public bool canJump = true;
    public bool canDash = true;
    public bool jump_Input = false;
    private bool dash_Input = false;

    void Start()
    {

    }

    void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocalMotion = GetComponent<PlayerLocalMotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleDashInput();
        HandleJumpingInput();
        HandleMovementInput();
        HandleSprintingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocalMotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (b_Input && moveAmount > 0.5f)
        {
            playerLocalMotion.isSprinting = true;
        }
        else
        {
            playerLocalMotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            if (canJump)
            {
                canJump = false;
                playerLocalMotion.HandleJumping();
            }
            jump_Input = false;
        }
    }

    public void CanJumpTrigger()
    {
        StartCoroutine("MakeCanJump");
    }

    IEnumerator MakeCanJump()
    {
        yield return new WaitForSeconds(0.05f);
        canJump = true;
    }

    IEnumerator DashCooldown(float secondToWait)
    {

        yield return new WaitForSeconds(secondToWait);
        Debug.Log("Dash cooldown triggered!");
        canDash = true;
    }

    private void HandleDashInput()
    {
        if (dash_Input)
        {
            if (canDash)
            {
                playerLocalMotion.HandleDash();
                canDash = false;
                StartCoroutine("DashCooldown", 5.0f);
            }
        }

        dash_Input = false;
    }
}
