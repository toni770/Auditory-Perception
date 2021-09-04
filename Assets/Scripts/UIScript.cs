using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    Transform buttons;
    Transform pauseMenu;
    public GameObject controlador;

    string actionPressed = "";
    int actionCount = 0;
    long actionTime = -1;
    public long doubleClickTime = 250;

    AudioSource aS;
    public AudioClip jugar, opcions, sortir, renaudar, menuInicial, botoActivat, idioma, grafics, vibracio, noDisponible;

    void Start()
    {
        aS = GetComponent<AudioSource>();
        buttons = transform.Find("Buttons");
        if (buttons) buttons.localScale = new Vector3( Screen.width / buttons.GetComponent<RectTransform>().rect.width, Screen.height / buttons.GetComponent<RectTransform>().rect.height, 1);

        pauseMenu = transform.Find("PauseMenu");
        if (pauseMenu)
        {
            Transform buttonsPause = pauseMenu.transform.Find("Buttons");
            buttonsPause.localScale = new Vector3(Screen.width / buttonsPause.GetComponent<RectTransform>().rect.width, Screen.height / buttonsPause.GetComponent<RectTransform>().rect.height, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actionPressed != "" && actionTime > -1)
        {
            long timeWhileClick = (long)(Time.realtimeSinceStartup * 1000f) - actionTime;
            if (actionCount >= 2 || timeWhileClick >= doubleClickTime)
            {
                if (actionCount >= 2) // double click
                {
                    onDoubleClick(actionPressed);
                }
                else // simple click
                {
                    onSimpleClick(actionPressed);
                }

                actionCount = 0;
                actionPressed = "";
                actionTime = -1;
            }
        }
    }

    public void onClickScreen(string action)
    {
        if (actionPressed != action)
        {
            actionPressed = action;
            actionTime = (long)(Time.realtimeSinceStartup * 1000f);
            actionCount = 0;
        }
        actionCount++;
    }

    void onSimpleClick(string action) // activar sons
    {
        aS.Stop();
        switch (action)
        {
            case "Jugar":
                aS.clip = jugar;
                break;
            case "Opcions":
                aS.clip = opcions;
                break;
            case "Sortir":
                aS.clip = sortir;
                break;
            case "Reanudar":
                aS.clip = renaudar;
                break;
            case "MenuInicial":
                aS.clip = menuInicial;
                break;
            case "Idioma":
                aS.clip = idioma;
                break;
            case "Grafics":
                aS.clip = grafics;
                break;
            case "Vibracio":
                aS.clip = vibracio;
                break;
        }
        aS.Play();
    }

    void onDoubleClick(string action) // fer la acció
    {
        aS.Stop();
        aS.clip = botoActivat;
        aS.Play();
        switch (action)
        {
            case "Jugar":
                // cridar al primer nivell
                SceneManager.LoadScene("IntroHistoria");
                break;
            case "Opcions":
                // obrir el menú de opcions
                SceneManager.LoadScene("MenuOpcions");
                break;
            case "Sortir":
                // tancar el joc
                Application.Quit();
                break;
            case "Reanudar":
                // tancar menu de pausa
                if (controlador.GetComponent<Minijoc_1>()) controlador.GetComponent<Minijoc_1>().continuarJoc();
                else if (controlador.GetComponent<Minijoc_2>()) controlador.GetComponent<Minijoc_2>().continuarJoc();
                else if(controlador.GetComponent<Minijoc_3>()) controlador.GetComponent<Minijoc_3>().continuarJoc();
                else if (controlador.GetComponent<Minijoc_4>()) controlador.GetComponent<Minijoc_4>().continuarJoc();
                //pauseMenu.gameObject.SetActive(false);
                break;
            case "MenuInicial":
                // cridar a menu inicial
                SceneManager.LoadScene("MenuInicial");
                break;
            case "Idioma":
                aS.Stop();
                aS.clip = noDisponible;
                aS.Play();
                break;
            case "Grafics":
                aS.Stop();
                aS.clip = noDisponible;
                aS.Play();
                break;
            case "Vibracio":
                aS.Stop();
                aS.clip = noDisponible;
                aS.Play();
                break;
        }
    }
}
