using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIPauseScript : MonoBehaviour
{
    AudioSource aS;
    public AudioClip pausa;

    void OnEnable()
    {
        if (!aS) aS = GetComponent<AudioSource>();
        aS.clip = pausa;
        aS.Play();
    }
}
