using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
	[SerializeField] private Transform _spawnPoints;
	[SerializeField] private SpawnLogic _spawnLogic;
	private Vector3 _spawnPosition;

	public Vector3 GetFreeSpawnpoint()
	{
        foreach (Transform spawn in _spawnPoints)
		{
			if (spawn.CompareTag("SpawnPoint"))
			{
				Debug.Log(spawn.name +" "+_spawnLogic.IsOccupied.Value);
				if (_spawnLogic.IsOccupied.Value)
				{
					Debug.Log(spawn.gameObject.name + " occupied");
				}
				else
				{
					_spawnPosition = new Vector3(spawn.position.x, spawn.position.y + 5.0f, spawn.position.z);
					Debug.Log(spawn.gameObject.name + " " + _spawnPosition);
					return _spawnPosition;
				}
			}
			
		}
		return _spawnPosition;
	}
}
