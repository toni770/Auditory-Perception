using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Minijoc_3 : MonoBehaviour
{

    enum Estats { Intro, Sequencia, Input, FiNivell, Final }
    Estats estatActual = Estats.Intro;

    bool correcte = false;

    int dificultat = 1;
    int nivell = 1;
    public int nivellMaxim = 5;

    AudioSource aS;

    public Text Puntuacio;

    public AudioClip encert, error;



    bool fi = false;

    // Variables comunes de tots els minijocs
    public AudioSource intro;
    public AudioSource outro;
    private bool minijocAcabat = false;
    private float timer;
    public Text interficie;
    public GameObject pauseMenu;
    public GameObject uiMinijoc;

    AudioClip[] sons = new AudioClip[4];
         
    // Variables especifiques de cada minijoc
    private int nTocsPantalla = 0;
    public int TocsPerGuanyar = 10;


    private int[] sonsBotons = new int[4];
    public AudioSource[] sonsDisponibles = new AudioSource[4];


    private int primerSoSeleccionat = -1;
    private int segonSoSeleccionat = -1;

    private int topLeft = 0;
    private int bottomLeft = 1;
    private int topRight = 2;
    private int bottomRight = 3;
    private int[] botons = new int[4];
    private bool[] botonsUtilitzats = new bool[] { false, false, false, false};
    private bool[] sonsUtilitzats = new bool[] { false, false, false, false };


    private bool reproduintSequencia = false;
    private bool jocPausat = false;

    private string textGuanyar = "Parella correcta!";
    private string textError = "Parella incorrecta, torna a intentar-ho";

    private string tipus = "processament";
    private string numTipus = "numProc";

    private float puntuacio = 100;
    public float penalitzacio = 10;

    int ultimBotoApretat;


    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();

        #region Generar i emmagatzemar els sons necessaris per aquest minijoc
        // So 1
        sons[0] = generadorFrequencies.Instance.generarSo(440, 1, "so1");

        sonsDisponibles[0].clip = sons[0] as AudioClip;

        // So 2
        sons[1] = generadorFrequencies.Instance.generarSo(220, 1, "so2");

        sonsDisponibles[1].clip = sons[1] as AudioClip;

        // So 3
        sons[2] = generadorFrequencies.Instance.generarSo(550, 1, "so3");

        sonsDisponibles[2].clip = sons[2] as AudioClip;


        // So 4
        sons[3] = generadorFrequencies.Instance.generarSo(700, 1, "so4");

        sonsDisponibles[3].clip = sons[3] as AudioClip;
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

        //Debug.Log("Primer so seleccionat: " + primerSoSeleccionat + " Segon so seleccionat: " + segonSoSeleccionat);

        // Comença el minijoc
        if (!jocPausat)
        {
            EstatsJoc();
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

        if (primerSoSeleccionat != -1 && segonSoSeleccionat != -1)
        {
            if (primerSoSeleccionat == segonSoSeleccionat)
            {
                estatActual = Estats.FiNivell;
                print("FI NIVELL");
                correcte = true;
            }
            else
            {
                estatActual = Estats.FiNivell;
                print("FI NIVELL");
                correcte = false;
            }
            primerSoSeleccionat = -1;
            segonSoSeleccionat = -1;

        }

        print("HOLA");

        #region Logica del minijoc

        if (InputManager.Instance.Tap)
        {
            interficie.text = "";
            Vector2 tapPos = InputManager.Instance.TapPos;

            #region Comprovar si el botó tocat es el correcte
            if (tapPos.x >= Screen.width / 2)
            {
                // RIGHT TOUCH
                if (tapPos.y >= Screen.height / 2)
                {

                    sonsDisponibles[topRight].Play();

                    if (ultimBotoApretat != topRight)
                    {
                        if (primerSoSeleccionat == -1)
                        {
                            primerSoSeleccionat = sonsBotons[topRight];
                        }
                        else if (segonSoSeleccionat == -1)
                        {
                            segonSoSeleccionat = sonsBotons[topRight];
                        }
                    }

                    ultimBotoApretat = topRight;
                }
                else
                {

                    sonsDisponibles[bottomRight].Play();
                    if (ultimBotoApretat != bottomRight)
                    {
                        if (primerSoSeleccionat == -1)
                        {
                            primerSoSeleccionat = sonsBotons[bottomRight];
                        }
                        else if (segonSoSeleccionat == -1)
                        {
                            segonSoSeleccionat = sonsBotons[bottomRight];
                        }
                    }

                    ultimBotoApretat = bottomRight;
                }
            }
            else
            {
                // LEFT TOUCH
                if (tapPos.y >= Screen.height / 2)
                {

                    sonsDisponibles[topLeft].Play();

                    if (ultimBotoApretat != topLeft)
                    {
                        if (primerSoSeleccionat == -1)
                        {
                            primerSoSeleccionat = sonsBotons[topLeft];
                        }
                        else if (segonSoSeleccionat == -1)
                        {
                            segonSoSeleccionat = sonsBotons[topLeft];
                        }
                    }

                    ultimBotoApretat = topLeft;
                }
                else
                {

                    sonsDisponibles[bottomLeft].Play();

                    if (ultimBotoApretat != bottomLeft)
                    {
                        if (primerSoSeleccionat == -1)
                        {
                            primerSoSeleccionat = sonsBotons[bottomLeft];
                        }
                        else if (segonSoSeleccionat == -1)
                        {
                            segonSoSeleccionat = sonsBotons[bottomLeft];
                        }
                    }

                    ultimBotoApretat = bottomLeft;
                }
            }
            #endregion
         #endregion
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
                    estatActual = Estats.Input;
                    ultimBotoApretat = -1;
                    primerSoSeleccionat = -1;
                    segonSoSeleccionat = -1;
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
                    SceneManager.LoadScene("minijoc4");
                }
                break;
        }
    }


    //Pasa a sonar la sequencia
    void SonarSequencia()
    {
        fi = false;

        reproduintSequencia = true;

        GenerarSequencia();

        StartCoroutine(PlaySequence(1));

        estatActual = Estats.Sequencia;
    }

    void GenerarSequencia()
    {

        for(int i=0;i<botonsUtilitzats.Length;i++)
        {
            botonsUtilitzats[i] = false;
            sonsUtilitzats[i] = false;
        }
        #region Generar una sequencia aleatoria de sons
        //for (int i = 0; i < botons.Length / 2; i++)
        //{
        int firstButton = Random.Range(0, botons.Length);
        while (botonsUtilitzats[firstButton])
        {
            firstButton = Random.Range(0, botons.Length);
        }

        
        int secondButton = Random.Range(0, botons.Length);

        while (secondButton == firstButton || botonsUtilitzats[secondButton])
        {
            secondButton = Random.Range(0, botons.Length);
        }

        botonsUtilitzats[firstButton] = botonsUtilitzats[secondButton] = true;

        int so = Random.Range(0, sonsDisponibles.Length);
        while (sonsUtilitzats[so])
        {
            so = Random.Range(0, sonsDisponibles.Length);
        }
        sonsBotons[firstButton] = sonsBotons[secondButton] = so;
        sonsUtilitzats[so] = true;
        //}


        for (int i = 0; i < botons.Length; i++)
        {
            if (!botonsUtilitzats[i])
            {
                sonsBotons[i] = Random.Range(0, sonsDisponibles.Length);
            }
            Debug.Log("sonsBotons[" + i + "] = " + sonsBotons[i]);
        }

        for (int i = 0; i < sonsDisponibles.Length; i++)
        {
            sonsDisponibles[i].clip = sons[sonsBotons[i]];
        }

        #endregion
    }

    IEnumerator PlaySequence(float seconds)
    {
        interficie.text = "";

        for (int i = 0; i < sonsDisponibles.Length; i++)
        {
            sonsDisponibles[i].Play();
            yield return new WaitForSeconds(seconds);
        }

        reproduintSequencia = false;
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
                print("FI");
                aS.Stop();
                outro.Play();
                estatActual = Estats.Final;
            }
            else
            {
                print("NextLEvel");
                fi = true;
                StartCoroutine(SeguentNivell());
               // SonarSequencia();
            }
        }
    }

    //Passa al seguent nivell
    IEnumerator SeguentNivell()
    {
        yield return new WaitForSeconds(3);
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