using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public Transform target;
    public bool wantsToTarget = false;
    public float visionDistance = 10;
    public float visionAngle = 45;

    private List<TargetableThing> potentialTargets = new List<TargetableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

 
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        if (!wantsToTarget) target = null;

        cooldownScan -= Time.deltaTime;
        if (cooldownScan <= 0 || (target == null && wantsToTarget) ) ScanForTargets();

        cooldownPick -= Time.deltaTime;
        if (cooldownPick <= 0) PickATarget();


        if(target && CanSeeThing(target) == false) target = null;
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
