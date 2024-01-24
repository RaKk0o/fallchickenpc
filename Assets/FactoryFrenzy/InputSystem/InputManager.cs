using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Rendering.DebugUI;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	public MenuManager _menuManager;

	private void Start()
	{
		StartCoroutine(LateStart(1f));
	}

	IEnumerator LateStart(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		_menuManager = GameObject.Find("MENU").GetComponent<MenuManager>();
	}

#if ENABLE_INPUT_SYSTEM
	public void OnMove(CallbackContext context)
	{
		move = context.ReadValue<Vector2>();
	}

	public void OnLook(CallbackContext context)
	{
		if (cursorInputForLook)
		{
			look = context.ReadValue<Vector2>();
		}
	}
	public void OnJump(CallbackContext context)
	{
		if (!context.performed) return;
		jump = true;
	}

#endif

	public void OnMenuOpenClose(CallbackContext context)
	{
		if (context.performed) _menuManager.MenuOpenClose();
	}



	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
	
}
