using UnityEngine;
using System.Collections;
public class GroundControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private PlayerLocalMotion controllerScript;
    [SerializeField]
    private Camera cameraSettings;
    private float oldSpeed;
    private float oldSprintSpeed;
    private float oldJumpHeight;
    private bool isBlueBoxActive = true;
    private bool isRedBoxActive = true;
    void Start()
    {
        controllerScript = GetComponent<PlayerLocalMotion>();
        oldSpeed = controllerScript.runningSpeed;
        oldSprintSpeed = controllerScript.sprintingSpeed;
        //oldJumpHeight = controllerScript.JumpHeight;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("BlueBox") && isBlueBoxActive)
        {
            EffectBlue();
            StartCoroutine(CooldownBlue(15));
            StartCoroutine(CooldownBlueEffect(10));
        }
    }

    IEnumerator CooldownBlue(int secondsToWait)
    {
        isBlueBoxActive = false;
        yield return new WaitForSeconds(secondsToWait);
        isBlueBoxActive = true;

    }

    IEnumerator CooldownBlueEffect(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        NormalizeBlue();
    }

    void NormalizeBlue()
    {
        controllerScript.walkingSpeed = oldSpeed;
        controllerScript.sprintingSpeed = oldSprintSpeed;
        //StartCoroutine(ChangeFov(40f, 0.2f));
    }

    void EffectBlue()
    {
        controllerScript.runningSpeed = 8f;
        controllerScript.sprintingSpeed = 12f;
        //StartCoroutine(ChangeFov(45f, 0.2f));
    }

    /*void NormalizeRed()
    {
        controllerScript.JumpHeight = oldJumpHeight;
    }

    void EffectRed()
    {
        controllerScript.JumpHeight = oldJumpHeight * 2;
    }*/

}
