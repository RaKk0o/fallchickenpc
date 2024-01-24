using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLobbyManager : MonoBehaviour
{
	public void OnLaunchPress()
	{
		NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}

	public void OnQuitPress()
	{
		NetworkManager.Singleton.Shutdown();
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
