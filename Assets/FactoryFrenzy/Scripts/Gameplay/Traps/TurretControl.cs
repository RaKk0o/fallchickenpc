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

    private int _numberPlayerInside;

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
       head.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    void Chasing()
    {
		Vector3 relativePos = _Player.position - head.transform.position;
		Quaternion rotation = Quaternion.LookRotation(new Vector3 (relativePos.x, 0, relativePos.z), Vector3.up);
		head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    public void Shooting()
    {
        Debug.Log("Shooting");
		if (Time.time > nextFire)
		{
			nextFire = Time.time + 1f / fireRate;
			shoot();
		}
        SetChasingState();
	}
    public void SetChasingState()
    {
        currentState = TurretState.Chasing;
    }

    public void SetShootingState()
    {
		currentState = TurretState.Shooting;
	}
    void shoot()
    {
        GameObject clone = Instantiate(_bullet, canon.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * 500);
        Destroy(clone, 3);
    }

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Player detected");
		if (other.CompareTag("Player"))
		{
            _numberPlayerInside++ ;
			SetChasingState();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("Player detected");
		if (other.CompareTag("Player"))
		{
            _numberPlayerInside--;
            if (_numberPlayerInside == 0) currentState = TurretState.Idle;
		}
	}
}
