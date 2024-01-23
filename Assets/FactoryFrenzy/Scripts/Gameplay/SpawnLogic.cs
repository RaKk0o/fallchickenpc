using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnLogic : NetworkBehaviour
{
    public NetworkVariable<bool> IsOccupied = new NetworkVariable<bool>();

	public override void OnNetworkSpawn()
	{
		if (IsServer)
		{
			IsOccupied.Value = false;
		}
		Debug.Log("Patate");
		IsOccupied.OnValueChanged += OnStateChange;
	}

	public override void OnNetworkDespawn()
	{
		IsOccupied.OnValueChanged -= OnStateChange;
	}

	private void OnStateChange(bool previous, bool current)
	{
		Debug.Log(current);
	}

	private void OnTriggerEnter(Collider collider)
	{
		Debug.Log("TriggerEnter");
		if (collider.gameObject.CompareTag("Player"))
		{
			ToggleSpawnPointServerRpc();
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		Debug.Log("TriggerExit");
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
