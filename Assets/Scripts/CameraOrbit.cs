using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraOrbit : MonoBehaviour
{
    public PlayerMovement target;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSenstivityX = 10;
    public float cameraSenstivityY = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();

        transform.position = target.position.transform;



    }

    private void RotateCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSenstivityX;
        pitch += my * cameraSenstivityY;

        pitch = Mathf.Clamp(pitch, -89, 89);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
