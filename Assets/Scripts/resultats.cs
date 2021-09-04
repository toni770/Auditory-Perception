using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class resultats : MonoBehaviour
{
    public Text memoriaTxt, percepcioTxt, processamentTxt, totalTxt;
    float memoria, percepcio, processament, tota;

    AudioSource aS;
    public AudioClip aDescripcio, aMemoria, aPercepcio, aProcessament, aTotal, aMoltAlta, aAlta, aNormal, aBaixa, aMoltBaixa;
    AudioClip[] sonsReproduir = new AudioClip[9];
    int soReproduit = 0;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        MostrarResultats();
    }

    private void Update()
    {
        #region Swipe Left : Repetir Sequencia
        if (InputManager.Instance.SwipeLeft)
        {
            Debug.Log("Repetir resultats");
            aS.Stop();
            soReproduit = 0;
        }
        #endregion

        if (!aS.isPlaying && soReproduit < 9)
        {
            aS.Stop();
            aS.clip = sonsReproduir[soReproduit];
            soReproduit++;
            aS.Play();
        }
    }

    private void GenerarDescripcio()
    {
        sonsReproduir[0] = aDescripcio;

        sonsReproduir[1] = aMemoria;
        if (memoria > 100 - ((100 / 5) * 1)) sonsReproduir[2] = aMoltAlta;
        else if (memoria > 100 - ((100 / 5) * 2)) sonsReproduir[2] = aAlta;
        else if (memoria > 100 - ((100 / 5) * 3)) sonsReproduir[2] = aNormal;
        else if (memoria > 100 - ((100 / 5) * 4)) sonsReproduir[2] = aBaixa;
        else sonsReproduir[2] = aMoltBaixa;

        sonsReproduir[3] = aPercepcio;
        if (percepcio > 100 - ((100 / 5) * 1)) sonsReproduir[4] = aMoltAlta;
        else if (percepcio > 100 - ((100 / 5) * 2)) sonsReproduir[4] = aAlta;
        else if (percepcio > 100 - ((100 / 5) * 3)) sonsReproduir[4] = aNormal;
        else if (percepcio > 100 - ((100 / 5) * 4)) sonsReproduir[4] = aBaixa;
        else sonsReproduir[4] = aMoltBaixa;

        sonsReproduir[5] = aProcessament;
        if (processament > 100 - ((100 / 5) * 1)) sonsReproduir[6] = aMoltAlta;
        else if (processament > 100 - ((100 / 5) * 2)) sonsReproduir[6] = aAlta;
        else if (processament > 100 - ((100 / 5) * 3)) sonsReproduir[6] = aNormal;
        else if (processament > 100 - ((100 / 5) * 4)) sonsReproduir[6] = aBaixa;
        else sonsReproduir[6] = aMoltBaixa;

        sonsReproduir[7] = aTotal;
        if (tota > 100 - ((100 / 5) * 1)) sonsReproduir[8] = aMoltAlta;
        else if (tota > 100 - ((100 / 5) * 2)) sonsReproduir[8] = aAlta;
        else if (tota > 100 - ((100 / 5) * 3)) sonsReproduir[8] = aNormal;
        else if (tota > 100 - ((100 / 5) * 4)) sonsReproduir[8] = aBaixa;
        else sonsReproduir[8] = aMoltBaixa;
    }

    public void MostrarResultats()
    {
        memoria = PlayerPrefs.GetFloat("memoria", 0) / PlayerPrefs.GetInt("numMem",1);
        percepcio = PlayerPrefs.GetFloat("percepcio", 0) / PlayerPrefs.GetInt("numPerc", 1);
        processament = PlayerPrefs.GetFloat("processament", 0) / PlayerPrefs.GetInt("numProc", 1);

        print(PlayerPrefs.GetFloat("memoria", 0));
        tota = (memoria + percepcio + processament) / 3;

        GenerarDescripcio();

        memoriaTxt.text = memoria.ToString();
        percepcioTxt.text = percepcio.ToString();
        processamentTxt.text = processament.ToString();

        totalTxt.text = tota.ToString();

        ResetPuntuacions();
    }

    void ResetPuntuacions()
    {
        PlayerPrefs.SetFloat("memoria", 0);
        PlayerPrefs.SetFloat("numMem", 0);
        PlayerPrefs.SetFloat("percepcio", 0);
        PlayerPrefs.SetFloat("numPerc", 0);
        PlayerPrefs.SetFloat("processament", 0);
        PlayerPrefs.SetFloat("numProc", 0);
    }
}
