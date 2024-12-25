using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputControl inputControl;
    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayers;
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;
    public float cameraFollowSpeed = 0.05f;
    public float cameraLookSpeed = 0.1f;
    public float cameraPivotSpeed = 0.1f;
    public float cameraCollisionRadius = 2f;
    public float cameraCollsionOffset = 0.2f;
    public float minimiumCollsionOffset = 0.2f;
    public float lookAngle;
    public float pivotAngle;

    private void Awake()
    {
        inputControl = FindObjectOfType<InputControl>();
        targetTransform = FindObjectOfType<LayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }
    public void HandleAllCMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        lookAngle = lookAngle + (inputControl.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputControl.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, -45f, 45f);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;

        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition -= -distance + cameraCollsionOffset;
        }

        if (Mathf.Abs(targetPosition) < minimiumCollsionOffset)
        {
            targetPosition = targetPosition - minimiumCollsionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
