using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{  
    private PlayerTargeting playerTargeting;


    private Quaternion startingRotation;

    public bool lockRotationX;
    public bool lockRotationY;
    public bool lockRotationZ;

    void Start()
    {
        startingRotation = transform.localRotation;
        playerTargeting = GetComponentInParent<PlayerTargeting>();
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardsTarget();
    }

    private void TurnTowardsTarget()
    {
        if (playerTargeting && playerTargeting.target && playerTargeting.wantsToTarget) {
            Vector3 disToTarget = playerTargeting.target.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up);

            transform.rotation = targetRotation;

            Vector3 euler1 = transform.localEulerAngles;

            Quaternion preRot = transform.rotation;

            Vector3 euler2 = transform.localEulerAngles;

            if (lockRotationX) euler2.x = euler1.x;
            if (lockRotationY) euler2.y = euler1.y;
            if (lockRotationZ) euler2.z = euler1.z;


            transform.rotation = preRot;

            transform.rotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .1f);


            //transform.rotation = AnimMath.Slide(transform.rotation, targetRotation, .1f);
        }
        else
        {

            



            transform.localRotation = AnimMath.Slide(transform.localRotation, startingRotation, .05f);
        }
    }
}
