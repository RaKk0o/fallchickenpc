using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBackBumper : MonoBehaviour
{
    [SerializeField] float bounceForce;
    [SerializeField] string playerTag;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
        }
    }
}
