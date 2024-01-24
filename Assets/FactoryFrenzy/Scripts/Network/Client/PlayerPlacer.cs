using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlacer : NetworkBehaviour
{
    [SerializeField] private GameObject _player;


    public override void OnNetworkSpawn()
    {
		if (IsHost)	NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnPlayer;
    }

	public Vector3 GetFreeSpawnpoint()
	{
		Debug.Log("Looking for spawn");
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		foreach (GameObject spawn in spawnPoints)
		{
			if (spawn.CompareTag("SpawnPoint"))
			{
				SpawnLogic spawnStatus = spawn.GetComponent<SpawnLogic>();
				Debug.Log(spawnStatus.name + " " + spawnStatus.IsOccupied.Value);
				if (spawnStatus.IsOccupied.Value)
				{
					Debug.Log(spawn.gameObject.name + " occupied");
				}
				else
				{
					Vector3 spawnPosition = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 5.0f, spawn.transform.position.z);
					spawnStatus.IsOccupied.Value = true;
					Debug.Log(spawn.gameObject.name + " " + spawnPosition);
					return spawnPosition;
				}
			}

		}
		return new Vector3(0,5,0);
	}

	private void SpawnPlayer(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
		
		if (IsHost && sceneName == "Game")
		{
			foreach (ulong id in clientsCompleted)
			{
				Vector3 position = GetFreeSpawnpoint();
				Quaternion rotation = new Quaternion(0, 0, 0, 1);
				GameObject player = Instantiate(_player, position, rotation);
				player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
			}
		}
	}
}
