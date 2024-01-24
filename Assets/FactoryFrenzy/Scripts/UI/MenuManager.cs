using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvasGO;
	[SerializeField] private GameObject _mainMenuFirst;
	private bool isMenuOpen;

	private void Start()
	{
		_mainMenuCanvasGO.SetActive(false);
		isMenuOpen = false;
	}

	public void MenuOpenClose()
	{
		if (!isMenuOpen)
		{
			OpenMenu();
		}
		else
		{
			CloseMenu();
		}		
	}

	private void OpenMenu()
	{
		isMenuOpen = true;
		Cursor.lockState = CursorLockMode.None;
		_mainMenuCanvasGO.SetActive(true);

		EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
	}

	private void CloseMenu()
	{
		isMenuOpen = false;
		Cursor.lockState = CursorLockMode.Locked;
		_mainMenuCanvasGO.SetActive(false);

		EventSystem.current.SetSelectedGameObject(null);

	}

	public void OnResumePress()
	{
		CloseMenu();
	}

	public void OnQuitPress()
	{
		NetworkManager.Singleton.Shutdown();
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
