using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Netcode.NetworkManager;

namespace FrenzyFactory.UI
{

    public class ConnectionApprovalHandler : MonoBehaviour
    {
        public static ConnectionApprovalHandler instance;
        private NetworkManager m_NetworkManager;

        [SerializeField] private int MaxNumberOfPlayers = 10;
        private int _numberOfPlayers = 0;

        private bool gameInProgress;
        public int PlayerCount;
        public bool isFirstGame = true;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

			// Start is called before the first frame update
			void Start()
        {
            m_NetworkManager = GetComponent<NetworkManager>();
            if (m_NetworkManager != null)
            {
                m_NetworkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
                m_NetworkManager.ConnectionApprovalCallback += CheckApprovalCallback;
            }
            if (MaxNumberOfPlayers == 0)
            {
                MaxNumberOfPlayers++;
            }
        }

        public void StartGame(int playerCount)
        {
            gameInProgress = true;
            PlayerCount = playerCount;
			Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void EndRound()
        {
            gameInProgress = false;
            isFirstGame = false;
			Singleton.LocalClient.PlayerObject.Despawn();
			Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void CheckApprovalCallback(ConnectionApprovalRequest request, ConnectionApprovalResponse response)
        {
            bool isApproved = true;
            _numberOfPlayers++;
            if (_numberOfPlayers > MaxNumberOfPlayers)
            {
                isApproved = false;
                response.Reason = "Too many players in lobby.";
            }
            if (gameInProgress)
            {
                isApproved = false;
                response.Reason = "Game in progress.";
            }
            response.Approved = isApproved;
            response.Position = new Vector3(0, 3, 0);
        }

        private void OnClientDisconnectCallback(ulong clientID)
        {
            if (!m_NetworkManager.IsServer && m_NetworkManager.DisconnectReason != string.Empty && !m_NetworkManager.IsApproved)
            {
                Debug.Log($"Approval Declined Reason: {m_NetworkManager.DisconnectReason}");
            }
            _numberOfPlayers--;
        }
    }
}