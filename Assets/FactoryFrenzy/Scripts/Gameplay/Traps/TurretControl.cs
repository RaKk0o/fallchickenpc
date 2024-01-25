using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TurretControl : NetworkBehaviour
{
    [SerializeField] private Transform _Player;
	[SerializeField] private Transform head, canon;
	[SerializeField] private GameObject _bullet;
	[SerializeField] private float fireRate;
	[SerializeField] private float nextFire;
	[SerializeField] private float rotationSpeed;

    private List<Transform> _transformPlayerInside = new();
    private enum TurretState
    {
        Idle,
        Chasing,
        Shooting
    }
    private TurretState currentState = TurretState.Idle;

    private void Update()
    {
        switch (currentState)
        {
               case TurretState.Idle:
                Idle();
                break;
               case TurretState.Chasing:
				ChasingClientRpc();
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

	[ClientRpc]
	void ChasingClientRpc()
    {
		Vector3 relativePos = _transformPlayerInside[0].position - head.transform.position;
		Quaternion rotation = Quaternion.LookRotation(new Vector3 (relativePos.x, 0, relativePos.z), Vector3.up);
		head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    public void Shooting()
    {
		if (Time.time > nextFire)
		{
			nextFire = Time.time + 1f / fireRate;
			shootServerRpc();
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

    [ServerRpc(RequireOwnership = false)]
    void shootServerRpc()
    {
        if (IsOwner)
        {
			GameObject clone = Instantiate(_bullet, canon.position, head.rotation);
			clone.GetComponent<NetworkObject>().Spawn();
			clone.GetComponent<Rigidbody>().AddForce(head.forward * 500);
			Destroy(clone, 3);
		}
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_transformPlayerInside.Add(other.transform);
            Debug.Log(_transformPlayerInside[0].GetComponent<NetworkObject>().OwnerClientId);

			SetChasingState();
		}
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
            _transformPlayerInside.Remove(other.transform);
            if (_transformPlayerInside.Count == 0) currentState = TurretState.Idle;
		}
	}
}
