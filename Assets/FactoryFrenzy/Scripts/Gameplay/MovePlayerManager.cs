using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovePlayerManager : MonoBehaviour
{
	private Vector3 _spawnPosition;

	public Vector3 GetFreeSpawnpoint()
	{
		Debug.Log("Looking for spawn");
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		foreach (GameObject spawn in spawnPoints)
		{
			if (spawn.CompareTag("SpawnPoint"))
			{
				SpawnLogic spawnStatus = spawn.GetComponent<SpawnLogic>();
				if (spawnStatus.IsOccupied.Value)
				{
					Debug.Log(spawn.gameObject.name + " occupied");
				}
				else
				{
					_spawnPosition = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 5.0f, spawn.transform.position.z);
					return _spawnPosition;
				}
			}

		}
		return _spawnPosition;
	}

	public void MovePlayer(Vector3 newPlayerPosition)
	{
		NetworkManager.Singleton.LocalClient.PlayerObject.transform.position  = newPlayerPosition;
	}
}
