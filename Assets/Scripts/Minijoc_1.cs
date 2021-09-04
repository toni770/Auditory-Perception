using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Minijoc_1 : MonoBehaviour
{

    enum Estats { Intro, Prova, Sequencia, Input, FiNivell, Final}
    Estats estatActual = Estats.Intro;

    int dificultat = 1;
    int nivell = 1;
    public int nivellMaxim = 5;

    public float tempsProva = 10;
    float contadorProva = 0;

    bool correcte = false;

    int numSeq = 1;

    bool fi = false;

    int soApretat = 0;

    public Text Puntuacio;

    AudioSource aS;

    public AudioClip encert, error, contaEnrera;

    // Variables comunes de tots els minijocs
    public AudioSource intro;
    public AudioSource outro;
    private bool minijocAcabat = false;
    private float timer;
    public Text interficie;
    public GameObject pauseMenu;
    public GameObject uiMinijoc;

    // Variables especifiques de cada minijoc
    private int nTocsPantalla = 0;
    public int TocsPerGuanyar = 10;
    AudioSource so1 = null;
    AudioSource so2 = null;
    AudioSource so3 = null;
    AudioSource so4 = null;

    private int[] sequenciaSons;
    public AudioSource[] sonsDisponibles = new AudioSource[4];


    private int inputs = 0;
    private int topLeft = 0;
    private int bottomLeft = 1;
    private int topRight = 2;
    private int bottomRight = 3;

    private bool reproduintSequencia = false;
    private bool jocPausat = false;

    private string textGuanyar = "Seqüència correcta!";
    private string textError = "Seqüència incorrecta!";

    private string tipus = "memoria";
    private string numTipus = "numMem";

    private float puntuacio = 100;
    public float penalitzacio = 10;


    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        #region Generar i emmagatzemar els sons necessaris per aquest minijoc
        // So 1
        AudioClip so1_clip = generadorFrequencies.Instance.generarSo(440, 1, "so1");
        
        sonsDisponibles[0].clip = so1_clip as AudioClip; 

        // So 2
        AudioClip so2_clip = generadorFrequencies.Instance.generarSo(220, 1, "so2");

        sonsDisponibles[1].clip = so2_clip as AudioClip;

        // So 3
        AudioClip so3_clip = generadorFrequencies.Instance.generarSo(550, 1, "so3");

        sonsDisponibles[2].clip = so3_clip as AudioClip;
      

        // So 4
        AudioClip so4_clip = generadorFrequencies.Instance.generarSo(700, 1, "so4");

        sonsDisponibles[3].clip = so4_clip as AudioClip;
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

        sequenciaSons = new int[nivellMaxim + 5];

        Puntuacio.text = puntuacio.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.SwipeLeft) pausarJoc();
        
        // Comença el minijoc
        if (!jocPausat)
        {
            EstatsJoc();
        }

    }

    ////////////////////////////////////SONS////////////////////////////////////
    void GenerarSequencia(int n)
    {
        numSeq = n;

        for (int i = 0; i < numSeq; i++)
        {
            // Escollir un so aleatori
            sequenciaSons[i] = Random.Range(0, 4);
            print(sequenciaSons[i]);
        }

    }

    IEnumerator PlaySequence(float seconds)
    {
        interficie.text = "";

        for (int i = 0; i < numSeq; i++)
        {
            sonsDisponibles[sequenciaSons[i]].Play();
            yield return new WaitForSeconds(seconds);
        }

        reproduintSequencia = false;       
    }

    IEnumerator PlaySound(int i, float seconds)
    {
        sonsDisponibles[i].Play();
        yield return new WaitForSeconds(seconds);
    }


    ////////////////////////////////////PAUSA////////////////////////////////////
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



    ////////////////////////////////////JOC////////////////////////////////////
    void EstatsJoc()
    {
        switch (estatActual)
        {
            case Estats.Intro:
                if (!intro.isPlaying)
                {
                    estatActual = Estats.Prova;
                   // SonarSequencia();
                }
                break;

            case Estats.Prova:
                contadorProva += Time.deltaTime;

                if (contadorProva > tempsProva && !aS.isPlaying)
                { 
                    SonarSequencia();
                }
                else if (contadorProva + 2 > tempsProva && !aS.isPlaying)
                {
                    aS.clip = contaEnrera;
                    aS.Play();
                }
                else
                {
                    if (InputManager.Instance.Tap)
                    {
                        Botons();
                    }
                }
                break;
            case Estats.Sequencia:
                if (!reproduintSequencia)
                {
                    inputs = 0;
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
                    SceneManager.LoadScene("minijoc2");
                }
                break;
        }
    }

    //Si apreta un boto, sona el so corresponent
    void Botons()
    {
        interficie.text = "";
        Vector2 tapPos = InputManager.Instance.TapPos;

        if (tapPos.x >= Screen.width / 2)
        {
            // RIGHT TOUCH
            if (tapPos.y >= Screen.height / 2)
            {
                Debug.Log("RIGHT TOP TOUCH");
                soApretat = topRight;
            }
            else
            {
                Debug.Log("RIGHT BOTTOM TOUCH");
                soApretat = bottomRight;
            }
        }
        else
        {
            // LEFT TOUCH
            if (tapPos.y >= Screen.height / 2)
            {
                Debug.Log("LEFT TOP TOUCH");
                soApretat = topLeft;
            }
            else
            {
                Debug.Log("LEFT BOTTOM TOUCH");
                soApretat = bottomLeft;
            }
        }

        StartCoroutine(PlaySound(soApretat, 2));
        

    }

    //Comprova si el boto entrat es correcte
    void Input()
    {
        #region Swipe Left : Pausar el joc
        if (InputManager.Instance.SwipeLeft)
        {
            Debug.Log("Pausa");
            pausarJoc();
        }
        #endregion

        #region Objectiu completat?
        if (inputs >= numSeq)
        {
            estatActual = Estats.FiNivell;
            correcte = true;
        }
        #endregion

        if (!reproduintSequencia && InputManager.Instance.Tap)
        {
            Botons();

            if (sequenciaSons[inputs] == soApretat)
            {
                Debug.Log("R GOOD");
                inputs++;
            }
            else
            {
                Debug.Log("R WRONG");
                inputs = 0;
                estatActual = Estats.FiNivell;
                correcte = false;
            }
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

    void PausarSons(bool pausar)
    {
        if(pausar)
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

    void SoResultat(bool encertat)
    {
        aS.Stop();
        if(encertat)
        {
            aS.clip = encert;
        }
        else
        {
            aS.clip = error;
        }
        aS.Play();
    }
    //Passa al seguent nivell
    IEnumerator SeguentNivell()
    {
        yield return new WaitForSeconds(5);
        SonarSequencia();
        fi = false;
    }

    //Pasa a sonar la sequencia
    void SonarSequencia()
    {
        reproduintSequencia = true;
        GenerarSequencia(dificultat + 2);
        StartCoroutine(PlaySequence(2));
        estatActual = Estats.Sequencia;
    }

    //Guardar els punts per mes endevant
    void GuardarPunts (float puntuacio)
    {
        PlayerPrefs.SetFloat(tipus, PlayerPrefs.GetFloat(tipus, 0) + puntuacio);
        PlayerPrefs.SetInt(numTipus, PlayerPrefs.GetInt(numTipus, 0) + 1);
    }
}
