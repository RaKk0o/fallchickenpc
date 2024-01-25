using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUp : MonoBehaviour
{
    public float trampolineForce;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.Move(Vector3.up * trampolineForce * Time.deltaTime);
            }
        }
    }
}
