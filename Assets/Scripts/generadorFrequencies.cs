using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generadorFrequencies : MonoBehaviour
{
    public static generadorFrequencies Instance { set; get; }

    private int sampleFreq = 44000;
    private float frequency = 440;
    private float[] samples;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip generarSo(float frequencia, int temps, string nom)
    {
        samples = new float[44000 * temps];

        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(Mathf.PI * 2 * i * frequencia / sampleFreq);
        }

        AudioClip ac = AudioClip.Create(nom, samples.Length, 1, sampleFreq, false);
        ac.SetData(samples, 0);

        return ac;
    }
}
