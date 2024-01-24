using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    private MovePlayerManager _playerManager;
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<MovePlayerManager>();
	}

	public override void OnNetworkSpawn()
	{
		StartCoroutine(MovePlayerCoroutine());
	}

    private IEnumerator MovePlayerCoroutine()
    {
        yield return new WaitForEndOfFrame();
		
		Vector3 position = _playerManager.GetFreeSpawnpoint();
		_playerManager.MovePlayer(position);

	}

}
