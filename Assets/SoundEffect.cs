using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource source;
    public Collider bumper;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        bumper = GetComponent<Collider>();
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            source.Play();
        }
    }
}
