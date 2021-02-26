using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraOrbit : MonoBehaviour
{
    public PlayerMovement moveScript;
    private PlayerTargeting targetScript;
    private Camera cam;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSenstivityX = 10;
    public float cameraSenstivityY = 10;

    private float shakeIntensity = 0;




    private void Start()
    {
        targetScript = moveScript.GetComponent<PlayerTargeting>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerOrbitCamera();

        transform.position = moveScript.transform.position;

        // if aiming set camera's rotation to look as target
        RotateCamToLookAtTarget();

        // "zoom" in the camera
        ZoomCamera();


        ShakeCamera();
    }

    public void Shake(float intensity = 1)
    {
            shakeIntensity += intensity;
    }

    private void ShakeCamera()
    {
        if (shakeIntensity < 0) shakeIntensity = 0;

        if (shakeIntensity > 0) shakeIntensity -= Time.deltaTime;
        else return; // shake intensity is 0

        // pick a small random rotation
        Quaternion targetRot = AnimMath.Lerp(Random.rotation, Quaternion.identity, .99f);

        //cam.transform.localRotation *= targetRot;
        cam.transform.localRotation = AnimMath.Lerp(cam.transform.localRotation, cam.transform.localRotation * targetRot, shakeIntensity * shakeIntensity);
    }


    private void ZoomCamera()
    {
        float dis = 10;
        if (IsTargeting()) dis = 3;

        cam.transform.localPosition = AnimMath.Slide(cam.transform.localPosition, new Vector3(0, 0, -dis), .001f);
    }

    private bool IsTargeting()
    {
        return (targetScript && targetScript.target != null && targetScript.wantsToTarget);
    }


    private void RotateCamToLookAtTarget()
    {
        // if targeting, set rotation to look at target

        if(targetScript && targetScript.target != null && targetScript.wantsToTarget)
        {
            // if targeting, set rotation to look at target

            Vector3 vToTarget = targetScript.target.position - cam.transform.position;

            Quaternion targetRot = Quaternion.LookRotation(vToTarget, Vector3.up);
        }
        else
        {
            // if NOT targeting

            cam.transform.localRotation = AnimMath.Slide(cam.transform.localRotation, Quaternion.identity, .001f);
        }
      
    }


    private void PlayerOrbitCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSenstivityX;
        pitch += my * cameraSenstivityY;


        if (IsTargeting())
        {
            pitch = Mathf.Clamp(pitch, 15, 60);

            float playerYaw = moveScript.transform.eulerAngles.y;

            yaw = Mathf.Clamp(yaw, playerYaw - 40, playerYaw + 40);
        }
        else
        {
            pitch = Mathf.Clamp(pitch, -10, 89);
        }

        
        transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);
    }
}
