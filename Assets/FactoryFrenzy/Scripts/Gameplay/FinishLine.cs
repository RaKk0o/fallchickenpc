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
	public Transform cube;
	private bool isFirst = true;

	public override void OnNetworkSpawn()
	{
		_gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") )
		{
			_gameMaster.DisplayScoreBoardClientRpc("Player" + other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
			other.GetComponent<CharacterController>().enabled = false;
			other.transform.position = cube.position;
			other.GetComponent<CharacterController>().enabled = true;
			if (isFirst && IsOwner)
			{
				isFirst = false;
				_gameMaster.ToggleGameStatusServerRpc();
				_gameMaster.TimerServerRpc();
			}
		}
	}
	
}
