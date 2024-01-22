using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : NetworkBehaviour
{
	[SerializeField] private List<GameObject> _spawnPoints = new();
	[SerializeField] private GameObject _player;
	private int _clientId;


	public override void OnNetworkSpawn()
	{
		//StartCoroutine(PlacePlayerCoroutine());
		NetworkManager.Singleton.ConnectionApprovalCallback += SpawnPlayerObject;
	}

	private IEnumerator PlacePlayerCoroutine()
	{
		yield return new WaitForEndOfFrame();
		_clientId = (int)NetworkManager.Singleton.LocalClientId;
		Vector3 spawnPosition = new Vector3(_spawnPoints[_clientId].transform.position.x, _spawnPoints[_clientId].transform.position.y + 5.0f, _spawnPoints[_clientId].transform.position.z);
		//NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = spawnPosition;
		Debug.Log("Client Id : "+ _clientId + ". Position : " +spawnPosition);
	}

	private void SpawnPlayerObject(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		response.Approved = true;
		Vector3 spawnPosition = new Vector3(_spawnPoints[_clientId].transform.position.x, _spawnPoints[_clientId].transform.position.y + 5.0f, _spawnPoints[_clientId].transform.position.z);

		NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = spawnPosition;
		Debug.Log("Client Id : " + _clientId + ". Position : " + spawnPosition);
	}


}
