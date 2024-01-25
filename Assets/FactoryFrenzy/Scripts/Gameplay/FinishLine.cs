using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using FrenzyFactory.UI;

public class FinishLine : NetworkBehaviour
{
	[SerializeField] GameMaster _gameMaster;
	private bool isFirst = true;

	public override void OnNetworkSpawn()
	{
		_gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && IsOwner)
		{
			_gameMaster.DisplayScoreBoardClientRpc("Player" + other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
			if (isFirst)
			{
				isFirst = false;
				_gameMaster.ToggleGameStatusServerRpc();
				_gameMaster.TimerServerRpc();
			}
		}
	}
	
}
