using UnityEngine;
using Unity.Netcode;

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
