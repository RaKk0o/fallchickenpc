using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    Transform _Player;
    float dist;
    public float howClose;
    public Transform head, canon;
    public GameObject _bullet;
    public float fireRate;
    private float nextFire;

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        dist = Vector3.Distance(_Player.position, transform.position);
        if (dist < howClose)
        {
            head.LookAt(_Player);
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        GameObject clone = Instantiate(_bullet, canon.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * 500);
        Destroy(clone, 3);
    }
}
