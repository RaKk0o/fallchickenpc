using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnLogic : NetworkBehaviour
{
    public NetworkVariable<bool> IsOccupied = new NetworkVariable<bool>();

	private void Start()
	{
		if (IsHost)
		{
			IsOccupied.Value = false;
		}
	}

	public override void OnNetworkSpawn()
	{
		IsOccupied.OnValueChanged += OnStateChange;
	}

	public override void OnNetworkDespawn()
	{
		IsOccupied.OnValueChanged -= OnStateChange;
	}

	private void OnStateChange(bool previous, bool current)
	{
		IsOccupied.Value = current;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("Player") && IsOwner)
		{
			ToggleSpawnPointServerRpc();
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			ToggleSpawnPointServerRpc();
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void ToggleSpawnPointServerRpc()
	{
		// this will cause a replication over the network
		// and ultimately invoke `OnValueChanged` on receivers
		IsOccupied.Value = !IsOccupied.Value;
	}

}
