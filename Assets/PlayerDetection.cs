using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private TurretControl turretControl;
    // Start is called before the first frame update
    void Start()
    {
        turretControl = GetComponentInParent<TurretControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player detected");
        if (other.CompareTag("Player"))
        {
            turretControl.SetShootingState();
        }
    }
}
