using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] private Transform pointDebut;
	[SerializeField] private Transform pointFin;
	[SerializeField] private float tempsDeplacement;


    // Update is called once per frame
    void Update()
    {

        float f = Mathf.PingPong(NetworkManager.Singleton.LocalTime.TimeAsFloat, tempsDeplacement) / tempsDeplacement;
        transform.position = Vector3.Lerp(pointDebut.position, pointFin.position, f);

        if (f >= 0.5f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			other.transform.parent = transform.parent;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			other.transform.parent = null;
        }
    }
}