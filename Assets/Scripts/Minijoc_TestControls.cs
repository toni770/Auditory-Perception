using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Minijoc_TestControls : MonoBehaviour
{
    // Variables comunes de tots els minijocs
    public AudioSource intro;
    public AudioSource outro;
    private bool minijocAcabat = false;
    private float timer;

    //public Canvas interficie;

    // Variables especifiques de cada minijoc
    private int nTocsPantalla = 0;
    public int TocsPerGuanyar = 10;
    //private Text nTocsPantalla_UI;
    public Text interficie;

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
        interficie.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!outro.isPlaying)
        {
            // Comença el minijoc

            if (InputManager.Instance.Tap)
            {
                interficie.text = "TAP";
            }
            if (InputManager.Instance.SwipeLeft)
            {
                interficie.text = "SCROLL LEFT";
            }
            if (InputManager.Instance.SwipeRight)
            {
                interficie.text = "SCROLL RIGHT";
            }
            if (InputManager.Instance.SwipeUp)
            {
                interficie.text = "SCROLL UP";
            }
            if (InputManager.Instance.SwipeDown)
            {
                interficie.text = "SCROLL DOWN";
            }
        }
    }
}

