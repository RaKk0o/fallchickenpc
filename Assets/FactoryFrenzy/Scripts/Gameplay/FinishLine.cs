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
	[SerializeField] private Transform _victoryIsland;
	private bool isFirst = true;

	public override void OnNetworkSpawn()
	{
		_gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
		_victoryIsland = GameObject.Find("VictoryIsland").transform;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_gameMaster.DisplayScoreBoardClientRpc("Player" + other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
			other.GetComponent<PlayerController>().Checkpoint = _victoryIsland.position;
			//other.GetComponent<PlayerController>().LoadCheckPoint();
			if (isFirst)
			{
				isFirst = false;
				_gameMaster.ToggleGameFinishedServerRpc();
				_gameMaster.TimerServerRpc();
			}
		}
	}
	
}
