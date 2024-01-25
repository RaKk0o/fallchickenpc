using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBack : MonoBehaviour
{
    [SerializeField] float bounceForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();

            Vector3 direction = other.transform.position - transform.position;
            characterController.Move(direction * bounceForce);
        }
    }
}
