using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Introduccio : MonoBehaviour
{
    public string escena = "MenuInicial";
    AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!aS.isPlaying)
        {
            SceneManager.LoadScene(escena);
        }
    }
}
