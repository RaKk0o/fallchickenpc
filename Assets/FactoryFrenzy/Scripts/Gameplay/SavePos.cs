using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SavePos : NetworkBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().Checkpoint = transform.position;
		}
	}
}
