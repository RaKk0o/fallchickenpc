using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    Transform _Player;
    public Transform head, canon;
    public GameObject _bullet;
    public float fireRate;
    private float nextFire;
    public float rotationSpeed;

    private enum TurretState
    {
        Idle,
        Chasing,
        Shooting
    }
    private TurretState currentState = TurretState.Idle;

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        switch (currentState)
        {
               case TurretState.Idle:
                Idle();
                break;
               case TurretState.Chasing:
                Chasing();
                break;
               case TurretState.Shooting:
                Shooting();
                break;
        }     
    }
    void Idle()
    {
       // head.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    void Chasing()
    {
        head.LookAt(_Player);
        if (Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            shoot();
        }
        Debug.Log("Chasing");
    }
    void Shooting()
    {
        Debug.Log("Shooting");
        currentState = TurretState.Idle;
    }
    public void SetChasingState()
    {
        currentState = TurretState.Chasing;
    }
    void shoot()
    {
        GameObject clone = Instantiate(_bullet, canon.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * 500);
        Destroy(clone, 3);
    }
}
