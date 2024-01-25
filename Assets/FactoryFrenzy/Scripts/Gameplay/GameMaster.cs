using FrenzyFactory.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameMaster : NetworkBehaviour
{
	private NetworkVariable<bool> isFinished = new();
	private NetworkVariable<bool> isTimerRunning = new();
	private NetworkVariable<float> timeRemaining = new();

	[SerializeField] private TMP_Text timeText;
	[SerializeField] private TMP_Text scoreBoardText;

	[SerializeField] private GameObject timerDisplay;
	[SerializeField] private GameObject endOfGameDisplay;
	[SerializeField] private GameObject scoreBoardDisplay;

	private string playerScoreBoard;
	private int playerPosition;

	private void Start()
	{
		if (IsHost)
		{
			isFinished.Value = false;
			isTimerRunning.Value = false;
			timeRemaining.Value = 10.0f;
		}
	}

	private void Update()
	{
		if (isTimerRunning.Value && IsOwner)
		{
			if (timeRemaining.Value > 0)
			{
				timeRemaining.Value -= Time.deltaTime;
				DisplayTimerClientRpc(timeRemaining.Value);
			}
			else
			{
				timeRemaining.Value = 0;
				isTimerRunning.Value = false;
				EndGameServerRpc();
			}
		}
	}

	public override void OnNetworkSpawn()
	{
		isFinished.OnValueChanged += GameStatus;
		isTimerRunning.OnValueChanged += TimerStatus;
		timeRemaining.OnValueChanged += TimeGoingDown;
	}



	public override void OnNetworkDespawn()
	{
		isFinished.OnValueChanged -= GameStatus;
		isTimerRunning.OnValueChanged -= TimerStatus;
		timeRemaining.OnValueChanged -= TimeGoingDown;
	}

	private void GameStatus(bool previousValue, bool newValue)
	{
		isFinished.Value = newValue;
	}

	private void TimerStatus(bool previousValue, bool newValue)
	{
		isTimerRunning.Value = newValue;
	}

	private void TimeGoingDown(float previousValue, float newValue)
	{
		timeRemaining.Value = newValue;
	}

	[ServerRpc(RequireOwnership = false)]
	public void ToggleGameStatusServerRpc()
	{
		isFinished.Value = !isFinished.Value;
	}

	[ServerRpc]
	public void TimerServerRpc()
	{
		isTimerRunning.Value = true;
		TimerClientRpc();
	}

	[ClientRpc]
	private void TimerClientRpc()
	{
		Debug.Log("Timer Lancé");
		timerDisplay.SetActive(true);
	}


	[ClientRpc]
	private void DisplayTimerClientRpc(float timeToDisplay)
	{
		timeToDisplay += 1;
		float minutes = Mathf.FloorToInt(timeToDisplay / 60);
		float seconds = Mathf.FloorToInt(timeToDisplay % 60);
		timeText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
	}

	[ClientRpc]
	public void DisplayScoreBoardClientRpc(string playerName)
	{
		scoreBoardDisplay.SetActive(true);
		playerPosition++;
		playerScoreBoard += playerPosition +". " + playerName + "\n";
		scoreBoardText.SetText(playerScoreBoard);
	}

	[ServerRpc]
	private void EndGameServerRpc()
	{
		EndGameClientRpc();
		StartCoroutine(EndGameCoroutine(5.0f));
	}
	[ClientRpc]
	private void EndGameClientRpc()
	{
		endOfGameDisplay.SetActive(true);
	}

	private IEnumerator EndGameCoroutine(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		ConnectionApprovalHandler.instance.EndRound();
	}
}
