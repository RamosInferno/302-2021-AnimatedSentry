using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public Transform target;
    public bool wantsToTarget = false;
    public bool wantsToAttack = false;
    public float visionDistance = 10;
    public float visionAngle = 45;

    private List<TargetableThing> potentialTargets = new List<TargetableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;

    public Transform armL;
    public Transform armR;
    float cooldownShoot = 0;

    public float roundsPerSecond = 0;

    private Vector3 startPosArmL;
    private Vector3 startPosArmR;

    public ParticleSystem prefabMuzzleFlash;
    public Transform handL;
    public Transform handR;

    CameraOrbit  camOrbit;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        startPosArmL = armL.localPosition;
        startPosArmR = armR.localPosition;

        camOrbit = Camera.main.GetComponentInParent<CameraOrbit>();
    }

 
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");
        wantsToAttack = Input.GetButton("Fire1");

        if (!wantsToTarget) target = null;

        cooldownScan -= Time.deltaTime;
        if (cooldownScan <= 0 || (target == null && wantsToTarget) ) ScanForTargets();

        cooldownPick -= Time.deltaTime;
        if (cooldownPick <= 0) PickATarget();

        if (cooldownShoot > 0) cooldownShoot -= Time.deltaTime;


        if(target && CanSeeThing(target) == false) target = null;

        SlideArmsHome();

        DoAttack();

    }
    private void SlideArmsHome()
    {
        armL.localPosition = AnimMath.Slide(armL.localPosition, startPosArmL, .01f);
        armR.localPosition = AnimMath.Slide(armR.localPosition, startPosArmR, .01f);
    }

    private void DoAttack()
    {
        if (cooldownShoot > 0) return;
        if (!wantsToTarget) return;
        if (!wantsToAttack) return;
        if (target == null) return;
        if (!CanSeeThing(target)) return;

        HealthSystem targetHealth = target.GetComponent<HealthSystem>();

        print("PEW");
        cooldownShoot = 1 / roundsPerSecond;

        // attack!

        camOrbit.Shake(.5f);

        if(handL)Instantiate(prefabMuzzleFlash, handL.position, handL.rotation);
        if(handR)Instantiate(prefabMuzzleFlash, handR.position, handR.rotation);


        // trigger arm animation
        armL.localEulerAngles += new Vector3(-20, 0, 0);
        armR.localEulerAngles += new Vector3(-20, 0, 0);


        armL.position += -armL.forward * .1f;
        armR.position += -armR.forward * .1f;

    }



    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false;

        Vector3 vToThing = thing.position - transform.position;

        // check distance
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false; // too far away to see

        // check direction
       if(Vector3.Angle(transform.forward, vToThing) > visionAngle) return false; // out of vision cone

        return true;
    }

    private void ScanForTargets()
    {
        cooldownScan = 1;


       TargetableThing[] things = GameObject.FindObjectsOfType<TargetableThing>();

        foreach(TargetableThing thing in things)
        {

            // if we can see it
            // add target to Potential target

            if (CanSeeThing(thing.transform))
            {
                potentialTargets.Add(thing);
            }
        }
    }

    void PickATarget()
    {

        cooldownPick = .25f;

        //if (target) return;
        target = null;

        float closesDistanceSoFar = 0;

        foreach(TargetableThing pt in potentialTargets)
        {
            float dd = (pt.transform.position - transform.position).sqrMagnitude;

            if(dd < closesDistanceSoFar || target == null)
            {
                target = pt.transform;
                closesDistanceSoFar = dd;
            }
        }

    }
}
