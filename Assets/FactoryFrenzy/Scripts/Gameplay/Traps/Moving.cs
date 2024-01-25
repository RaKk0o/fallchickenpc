using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Transform pointDebut;
    public Transform pointFin;
    public float tempsDeplacement;

    private bool joueurPresent = false;
    private Transform joueur;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (joueurPresent)
        {
            joueur.parent = transform;
        }

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
            joueurPresent = true;
            joueur = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurPresent = false;
            joueur.parent = null;
            joueur = null;
        }
    }
}