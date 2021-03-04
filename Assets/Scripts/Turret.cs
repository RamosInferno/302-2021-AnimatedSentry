using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject target;
    private bool targetLocked;

    public GameObject turretFollow;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;
    public float fireTimer;
    private bool shotReady;

    void Start()
    {
        shotReady = true;
    }


    void Update()
    {
        if (targetLocked)
        {
            turretFollow.transform.LookAt(target.transform);
            turretFollow.transform.Rotate(0, -90, 0);
        }

        if (shotReady)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Transform _bullet = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        _bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            target = other.gameObject;
            targetLocked = true;
        }
    }
}
