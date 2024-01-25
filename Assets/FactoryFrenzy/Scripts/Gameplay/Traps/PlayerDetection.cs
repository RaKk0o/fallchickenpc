using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDetection : NetworkBehaviour
{
    [SerializeField] private TurretControl turretControl;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turretControl.SetShootingState();
        }
    }
}
