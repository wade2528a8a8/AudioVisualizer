using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeSound : MonoBehaviour
{

    private AudioSource m_source;

    //public float rmsValue; //rms is the average power output of the sound
    //public float dbValue; //the sound value during that exact frame

    public float[] m_spectrums;
    private const int SAMPLE_SIZE = 512;


    private float m_sampleRate;

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        m_spectrums = new float[SAMPLE_SIZE];
        m_sampleRate = AudioSettings.outputSampleRate;

    }


    private void Update()
    {
        m_source.GetSpectrumData(m_spectrums, 0, FFTWindow.BlackmanHarris);
    }

}
