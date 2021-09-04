using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Minijoc_4 : MonoBehaviour
{

    enum Estats { Intro, Sequencia, Input, FiNivell, Final }
    Estats estatActual = Estats.Intro;

    public Text Puntuacio;

    public AudioClip encert, error;

    bool correcte = false;

    int dificultat = 0;
    int nivell = 1;
    public const int nivellMaxim = 10;

    bool fi = false;

    AudioSource aS;

    private string tipus = "percepcio";
    private string numTipus = "numPerc";

    // Variables comunes de tots els minijocs
    public AudioSource intro;
    public AudioSource outro;
    private bool minijocAcabat = false;
    private float timer;
    public GameObject pauseMenu;
    public GameObject uiMinijoc;



    // Variables especifiques de cada minijoc
    int encerts;

    int totalTap = 0;
    int tapToPause = 2;
    long actionTime = -1;
    public long doubleClickTime = 250;
    private bool jocPausat = false;

    AudioSource so1 = null;
    AudioSource so2 = null;
    AudioSource so3 = null;

    private float puntuacio = 100;
    public float penalitzacio = 10;


    private int[] sequenciaSons = new int[nivellMaxim];
    private AudioSource[] sonsDisponibles = new AudioSource[3];

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();

        #region Generar i emmagatzemar els sons necessaris per aquest minijoc
        // So 1
        AudioClip so1_clip = generadorFrequencies.Instance.generarSo(800, 1, "so1");

        so1 = gameObject.AddComponent<AudioSource>();
        so1.clip = so1_clip as AudioClip;

        so1.volume = 0.3f;

        sonsDisponibles[0] = so1;

        // So 2
        AudioClip so2_clip = generadorFrequencies.Instance.generarSo(550, 1, "so2");

        so2 = gameObject.AddComponent<AudioSource>();
        so2.clip = so2_clip as AudioClip;

        so2.volume = 0.3f;

        sonsDisponibles[1] = so2;

        // So 3
        AudioClip so3_clip = generadorFrequencies.Instance.generarSo(220, 1, "so3");

        so3 = gameObject.AddComponent<AudioSource>();
        so3.clip = so3_clip as AudioClip;

        so3.volume = 0.3f;

        sonsDisponibles[2] = so3;
        #endregion

        sequenciaSons[0] = 1;


        #region Generar una sequencia aleatoria de sons
        for (int i = 1; i < nivellMaxim; i++)
        {

            // Escollir un so aleatori
            sequenciaSons[i] = Random.Range(0, 3);
        }
        #endregion

        //outro.Play();

    }


    void PausarSons(bool pausar)
    {
        if (pausar)
        {
            intro.Pause();
            outro.Pause();
            aS.Pause();
            sonsDisponibles[0].Pause();
            sonsDisponibles[1].Pause();
            sonsDisponibles[2].Pause();
        }
        else
        {
            intro.UnPause();
            outro.UnPause();
            aS.UnPause();
            sonsDisponibles[0].UnPause();
            sonsDisponibles[1].UnPause();
            sonsDisponibles[2].UnPause();
        }
    }

    IEnumerator PlaySound(int i, float seconds)
    {
        sonsDisponibles[sequenciaSons[i]].Play();
        yield return new WaitForSeconds(seconds);
    }

    public void pausarJoc()
    {
        jocPausat = true;
        PausarSons(jocPausat);
        pauseMenu.gameObject.SetActive(true);
        uiMinijoc.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void continuarJoc()
    {
        jocPausat = false;
        PausarSons(jocPausat);
        pauseMenu.gameObject.SetActive(false);
        uiMinijoc.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (InputManager.Instance.SwipeLeft) pausarJoc();

        if (!jocPausat)
        {
            EstatsJoc();
        }
    }

    void EstatsJoc()
    {
        switch (estatActual)
        {
            case Estats.Intro:
                if (!intro.isPlaying)
                {
                    estatActual = Estats.Sequencia;   
                }
                break;

            case Estats.Sequencia:
                StartCoroutine(PlaySound(dificultat, 2));
                estatActual = Estats.Input;
                print("INPUT");
     
                break;
            case Estats.Input:
                Input();
                break;

            case Estats.FiNivell:
                NivellAcabat();
                break;

            case Estats.Final:
                if (!outro.isPlaying)
                {
                    GuardarPunts(puntuacio);
                    SceneManager.LoadScene("Resultats");
                }
                break;
        }
    }

    //Comprova si ha encertat i si s'ha acabat el videojoc
    void NivellAcabat()
    {
        if (!fi)
        {
            if (correcte)
            {
                // Actualitzar la interfície
                SoResultat(true);
                dificultat++;
            }
            else
            {
                SoResultat(false);
                puntuacio -= penalitzacio;
                if (puntuacio < 0) puntuacio = 0;
                Puntuacio.text = puntuacio.ToString();
            }


            nivell++;
            if (nivell > nivellMaxim)
            {
                aS.Stop();
                outro.Play();
                estatActual = Estats.Final;
            }
            else
            {
                fi = true;
                StartCoroutine(SeguentNivell());
               
            }
        }
    }

    //Passa al seguent nivell
    IEnumerator SeguentNivell()
    {
        yield return new WaitForSeconds(1);
        estatActual = Estats.Sequencia;
        fi = false;
    }

    void Input()
    {
        #region Swipe Left : Pausar el joc
        if (InputManager.Instance.SwipeLeft)
        {
            Debug.Log("Pausa");
            pausarJoc();
        }
        #endregion

        if (InputManager.Instance.SwipeUp)
        {
            correcte = sequenciaSons[dificultat] == 0;

            totalTap = 0;
            estatActual = Estats.FiNivell;
        }
        else if (InputManager.Instance.SwipeRight)
        {
            correcte = sequenciaSons[dificultat] == 1;

            totalTap = 0;
            estatActual = Estats.FiNivell;
        }
        else if (InputManager.Instance.SwipeDown)
        {
            correcte = sequenciaSons[dificultat] == 2;

            totalTap = 0;
            estatActual = Estats.FiNivell;
        }
    }

    void SoResultat(bool encertat)
    {
        aS.Stop();
        if (encertat)
        {
            aS.clip = encert;
        }
        else
        {
            aS.clip = error;
        }
        aS.Play();
    }

    void GuardarPunts(float puntuacio)
    {
        PlayerPrefs.SetFloat(tipus, PlayerPrefs.GetFloat(tipus, 0) + puntuacio);
        PlayerPrefs.SetInt(numTipus, PlayerPrefs.GetInt(numTipus, 0) + 1);
    }
}