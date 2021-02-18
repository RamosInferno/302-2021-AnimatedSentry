using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public Transform target;
    public bool wantsToTarget = false;
    public float visionDistance = 10;
    private List<TargetableThing> potentialTargets = new List<TargetableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;

    void Start()
    {
        
    }

 
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime;
        if (cooldownScan <= 0) ScanForTargets();

        cooldownPick -= Time.deltaTime;
        if (cooldownPick <= 0) PickATarget();
    }

    private void ScanForTargets()
    {
        cooldownScan = 1;

       TargetableThing[] things = GameObject.FindObjectOfType<TargetableThing>();

        foreach(TargetableThing thing in things)
        {
           Vector3 disToThing = thing.transform.position - transform.position;


           if( disToThing.sqrmagnitude < visionDistance * visionDistance)
            {
               if( Vector3.Angle(transform.forward, disToThing) < 45 )
                {
                    potentialTargets.Add(thing);
                }
            }
        }
    }

    void PickATarget()
    {
        if (target) return;

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
