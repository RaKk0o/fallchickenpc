using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using FrenzyFactory.UI;

public class FinishLine : NetworkBehaviour
{
	private GameMaster _gameMaster;
	[SerializeField] private Transform victoryIsland;
	private bool isFirst = true;

	public override void OnNetworkSpawn()
	{
		_gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
				_gameMaster.DisplayScoreBoardClientRpc("Player" + other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
			//other.GetComponent<CharacterController>().enabled = false;
			//other.transform.position = victoryIsland.position;
			//other.GetComponent<CharacterController>().enabled = true;
			other.GetComponent<PlayerController>().Checkpoint = victoryIsland.position;
			other.GetComponent<PlayerController>().LoadCheckPoint();
			if (isFirst && IsOwner)
			{
				isFirst = false;
				_gameMaster.ToggleGameStatusServerRpc();
				_gameMaster.TimerServerRpc();
			}
		}
	}

}
