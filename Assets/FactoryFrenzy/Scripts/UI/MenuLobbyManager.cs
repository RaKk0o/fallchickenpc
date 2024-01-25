using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FrenzyFactory.UI { 
	public class MenuLobbyManager : NetworkBehaviour
	{
		[SerializeField] private LobbyPlayerCard[] lobbyPlayerCards;
		[SerializeField] private Button startGameButton;
		[SerializeField] private TMP_Text playerCountText;
		private NetworkList<LobbyPlayerState> lobbyPlayers;

		private void Awake()
		{
			lobbyPlayers = new NetworkList<LobbyPlayerState>();
			Cursor.lockState = CursorLockMode.None;
		}

		public override void OnNetworkSpawn()
		{
			if (IsClient)
			{
				lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
			}
			if (IsServer)
			{
				startGameButton.gameObject.SetActive(true);

				NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
				NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

				foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
				{
					HandleClientConnected(client.ClientId);
				}
			}
		}

		public override void OnDestroy()
		{
			lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

			if (NetworkManager.Singleton)
			{
				NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
				NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
			}
			lobbyPlayers?.Dispose();
		}

		private bool IsEveryoneReady()
		{
			if (lobbyPlayers.Count < 2)
			{
				return false;
			}
			foreach (var player in lobbyPlayers)
			{
				if (!player.IsReady) return false;
			}
			return true;
		}

		private void HandleClientConnected(ulong clientId)
		{
			lobbyPlayers.Add(new LobbyPlayerState(
				clientId,
				new String("Player"+lobbyPlayers.Count),
				false));
		}

		private void HandleClientDisconnect(ulong clientId)
		{
			for (int i = 0; i < lobbyPlayers.Count; i++)
			{
				if (lobbyPlayers[i].ClientId == clientId)
				{
					lobbyPlayers.RemoveAt(i);
					break;
				}
			}
		}

		[ServerRpc(RequireOwnership = false)]
		private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
		{
			for (int i = 0; i < lobbyPlayers.Count; i++)
			{
				if (lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
				{
					lobbyPlayers[i] = new LobbyPlayerState(
						lobbyPlayers[i].ClientId,
						lobbyPlayers[i].PlayerName,
						!lobbyPlayers[i].IsReady);
				}
			}
		}

		[ServerRpc(RequireOwnership = false)]
		public void StartGameServerRpc(ServerRpcParams serverRpcParams = default)
		{
			if (serverRpcParams.Receive.SenderClientId != NetworkManager.Singleton.LocalClientId) { return; }
			if (!IsEveryoneReady()) { return; }
			ConnectionApprovalHandler.instance.StartGame(lobbyPlayers.Count);
		}

		public void OnLeaveClicked()
		{
			NetworkManager.Singleton.Shutdown();
			SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
		}

		public void OnReadyClicked()
		{
			ToggleReadyServerRpc();
		}

		public void OnStartGameClicked()
		{
			StartGameServerRpc();
		}

		private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
		{
			playerCountText.SetText("Player Count : " + lobbyPlayers.Count.ToString());
			for (int i = 0; i < lobbyPlayerCards.Length; i++)
			{
				if (lobbyPlayers.Count > i)
				{
					lobbyPlayerCards[i].UpdateDisplay(lobbyPlayers[i]);
				}
				else
				{
					lobbyPlayerCards[i].DisableDisplay();
				}

				if (IsHost)
				{
					startGameButton.interactable = IsEveryoneReady();
				}
			}
		}

		

		
	}
}
