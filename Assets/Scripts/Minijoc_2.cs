using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Minijoc_2 : MonoBehaviour
{

    enum Estats { Intro, Sequencia, Input, FiNivell, Final }
    Estats estatActual = Estats.Intro;

    bool correcte = false;

    int dificultat = 1;
    int nivell = 1;
    public int nivellMaxim = 5;

    AudioSource aS;

    public Text Puntuacio;

    public AudioClip encert, error, beep;

    bool fi = false;

    // Variables comunes de tots els minijocs
    public AudioSource intro;
    public AudioSource outro;
    private bool minijocAcabat = false;
    private float timer;
    public Text interficie;
    // Variables especifiques de cada minijoc
    private int nTocsPantalla = 0;
    public GameObject pauseMenu;
    public GameObject uiMinijoc;

    AudioSource so1 = null;
    AudioSource so2 = null;
    AudioSource so3 = null;
    AudioSource so4 = null;
    private AudioSource[] sonsDisponibles = new AudioSource[4];


    private int nSonsSequencia;

    private int i = 0;

    private bool reproduintSequencia = false;
    private bool jocPausat = false;

    private string textGuanyar = "Nombre correcte";
    private string textError = "Nombre incorrecte";

    private string tipus = "percepcio";
    private string numTipus = "numPerc";

    private float puntuacio = 100;
    public float penalitzacio = 10;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        #region Generar i emmagatzemar els sons necessaris per aquest minijoc
        // So 1
        AudioClip so1_clip = generadorFrequencies.Instance.generarSo(440, 1, "so1");

        so1 = gameObject.AddComponent<AudioSource>();
        so1.clip = so1_clip as AudioClip;
        so1.volume = 0.3f;

        sonsDisponibles[0] = so1;

        // So 2
        AudioClip so2_clip = generadorFrequencies.Instance.generarSo(220, 1, "so2");

        so2 = gameObject.AddComponent<AudioSource>();
        so2.clip = so2_clip as AudioClip;
        so2.volume = 0.3f;

        sonsDisponibles[1] = so2;

        // So 3
        AudioClip so3_clip = generadorFrequencies.Instance.generarSo(550, 1, "so3");

        so3 = gameObject.AddComponent<AudioSource>();
        so3.clip = so3_clip as AudioClip;
        so3.volume = 0.3f;

        sonsDisponibles[2] = so3;

        // So 4
        AudioClip so4_clip = generadorFrequencies.Instance.generarSo(700, 1, "so4");

        so4 = gameObject.AddComponent<AudioSource>();
        so4.clip = so4_clip as AudioClip;
        so4.volume = 0.3f;

        sonsDisponibles[3] = so4;
        #endregion


        estatActual = Estats.Intro;
        dificultat = 1;
        nivell = 1;


        // Inicialitzar les interfícies
        interficie.text = "";

        // Reproduir el so generat
        //  StartCoroutine(PlaySequence(2));
        reproduintSequencia = false;
        timer = 0;

        // Inicialitzar les interfícies
        interficie.text = "";

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
            sonsDisponibles[3].Pause();
        }
        else
        {
            intro.UnPause();
            outro.UnPause();
            aS.UnPause();
            sonsDisponibles[0].UnPause();
            sonsDisponibles[1].UnPause();
            sonsDisponibles[2].UnPause();
            sonsDisponibles[3].UnPause();
        }
    }

    IEnumerator PlaySequence(float minSeconds, float maxSeconds)
    {
        reproduintSequencia = true;
        interficie.text = "";
     
        for(int i = 0; i < nSonsSequencia; i++)
        {
            sonsDisponibles[Random.Range(0,4)].Play();
            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));
        }

        reproduintSequencia = false;
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

        if (!outro.isPlaying)
        {
            // Comença el minijoc
            if (!jocPausat)
            {
                EstatsJoc();
            }
        }
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

        if (InputManager.Instance.Tap)
        {
            interficie.text = "";
            Vector2 tapPos = InputManager.Instance.TapPos;

            if (tapPos.x < Screen.width && tapPos.x >= (Screen.width - Screen.width / 3))
            {
                // RIGHT TOUCH
                Debug.Log("Right Touch");
                // SO DE SUMAR
                nTocsPantalla++;
                interficie.text = nTocsPantalla.ToString();
                aS.Stop();
                aS.clip = beep;
                aS.Play();
            }
            else if (tapPos.x > Screen.width / 3 && tapPos.x < (Screen.width - Screen.width / 3))
            {
                Debug.Log("Middle Touch");
                if (nTocsPantalla == nSonsSequencia)
                {
                    estatActual = Estats.FiNivell;
                    correcte = true;
                }
                else
                {
                    estatActual = Estats.FiNivell;
                    correcte = false;
                }
            }
            else if (tapPos.x > 0 && tapPos.x < Screen.width / 3)
            {
                // LEFT TOUCH
                Debug.Log("Left Touch");
                if (nTocsPantalla >= 1)
                {
                    // SO DE RESTAR
                    nTocsPantalla--;
                    interficie.text = nTocsPantalla.ToString();
                    aS.Stop();
                    aS.clip = beep;
                    aS.Play();
                }
                // else {So de error (no es pot restar més) ?}
            }

        }
    }

    void EstatsJoc()
    {
        switch (estatActual)
        {
            case Estats.Intro:
                if (!intro.isPlaying)
                {
                    estatActual = Estats.Input;
                    SonarSequencia();
                }
                break;

            case Estats.Sequencia:
                if (!reproduintSequencia)
                {
                    nTocsPantalla = 0;
                    estatActual = Estats.Input;
                    print("INPUT");
                }
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
                    SceneManager.LoadScene("minijoc3");
                }
                break;
        }
    }

    //Pasa a sonar la sequencia
    void SonarSequencia()
    {
        reproduintSequencia = true;

        nSonsSequencia = Random.Range(4, 10);

        StartCoroutine(PlaySequence(1f,2.5f));

        estatActual = Estats.Sequencia;
    }


    //Comprova si ha encertat i si s'ha acabat el videojoc
    void NivellAcabat()
    {
        if (!fi)
        {
            if (correcte)
            {
                // Actualitzar la interfície
                interficie.text = textGuanyar;
                SoResultat(true);
                dificultat++;
            }
            else
            {
                interficie.text = textError;
                SoResultat(false);
                puntuacio -= penalitzacio;
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
        yield return new WaitForSeconds(5);
        SonarSequencia();
        fi = false;
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
