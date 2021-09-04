using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Minijoc_Base : MonoBehaviour
{
    // Variables comunes de tots els minijocs
    public AudioSource intro;
    public AudioSource outro;
    private bool minijocAcabat = false;
    private float timer;

    //public Canvas interficie;
    public Text interficie;

    // Variables especifiques de cada minijoc
    private int nTocsPantalla = 0;
    public int TocsPerGuanyar = 10;
    //private Text nTocsPantalla_UI;

    // Start is called before the first frame update
    void Start()
    {
        //outro.Play();
        outro.Play();
        //nTocsPantalla_UI = interficie.GetComponent<Text>();
        //if(nTocsPantalla_UI != null) {
        //    nTocsPantalla_UI.text = nTocsPantalla.ToString();
        //}
        //else
        //{
        //    Debug.Log("Error! No s'ha trobat el text");
        //}
        interficie.text = nTocsPantalla.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (! outro.isPlaying)
        {
            // Comença el minijoc
            if(nTocsPantalla >= TocsPerGuanyar)
            {
                minijocAcabat = true;
            }
            
            if (InputManager.Instance.Tap && ! minijocAcabat)
            {
                nTocsPantalla++;
                interficie.text = nTocsPantalla.ToString();
            }

            if (minijocAcabat)
            {
                timer = timer + Time.deltaTime;
                //intro.PlayOneShot(intro.clip, 1.0f);
                if (! intro.isPlaying)
                {
                    intro.Play();                    
                }

                if (timer >= intro.clip.length)
                {
                    SceneManager.LoadScene("testControls");
                }
            }

        }
    }
}