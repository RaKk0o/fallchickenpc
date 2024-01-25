using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private TurretControl turretControl;

	private void Start()
	{
        turretControl = GetComponentInParent<TurretControl>();
	}
	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turretControl.SetShootingState();
        }
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			turretControl.SetShootingState();
		}
	}
}
