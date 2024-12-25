using UnityEngine;

public class LayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Animator animator;
    InputControl inputControl;
    CameraManager cameraManager;
    PlayerLocalMotion playerLocalMotion;
    public bool isInteracting;
    private void Awake()
    {
        inputControl = GetComponent<InputControl>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocalMotion = GetComponent<PlayerLocalMotion>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inputControl.HandleAllInputs();
    }

    void FixedUpdate()
    {
        playerLocalMotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCMovement();
        isInteracting = animator.GetBool("isInteracting");
    }
}
