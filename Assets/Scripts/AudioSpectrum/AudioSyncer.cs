using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{

    public float bias;          //bias which determines  spectrum value is going to trigger a beat
    public float timeStep;      //time step which determines  min interval between each beat
    public float timeToBeat;    //time to beat which determines how much time brfore the visualization completes (花多少時間到達)
    public float restSmoothTime;//rest smooth time which determines how fast the object goes to rest after a beat

    private float m_previousAudioValue;
    private float m_audioValue;
    private float m_timer;

    protected bool m_isBeat;

    public virtual void OnBeat()
    {
        m_timer = 0;
        m_isBeat = true;
    }

    public virtual void OnUpdate()
    {
        m_previousAudioValue = m_audioValue;
        m_audioValue = AudioSpectrum.SpectrumValue;

        if (m_previousAudioValue > bias && m_audioValue <= bias)
        {
            if (m_timer > timeStep)
            {
                OnBeat();
            }
        }

        if (m_previousAudioValue <= bias && m_audioValue > bias)
        {
            if (m_timer > timeStep)
            {
                OnBeat();
            }
        }

        m_timer += Time.deltaTime;

    }



    private void Update()
    {
        OnUpdate();
    }


}
