using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class RandomSoundPlayer : MonoBehaviour
{
    private System.Random rng = new System.Random();

    public List<Wwise_ID_Enum_EVENTS> Shuffle(List<Wwise_ID_Enum_EVENTS> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Wwise_ID_Enum_EVENTS value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    [SerializeField]
    Wwise_ID_Enum_EVENTS[] m_sounds;

    [SerializeField]
    float m_timeBetweenSoundMin = 5;
    [SerializeField]
    float m_timeBetweenSoundMax = 10;

    bool m_soundIsPlaying = false;

    Timer m_timer = new Timer();

    List<Wwise_ID_Enum_EVENTS> m_soundsPlayed;

    private void Start()
    {
        m_timer.Start(UnityEngine.Random.Range(m_timeBetweenSoundMin, m_timeBetweenSoundMax));

        foreach(Wwise_ID_Enum_EVENTS sound in m_sounds)
        {
            m_soundsPlayed.Add(sound);
        }

        m_soundsPlayed = Shuffle(m_soundsPlayed);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!m_soundIsPlaying)
        {
            m_timer.UpdateTimer();

            if (m_timer.IsTimedOut())
            {
                m_soundIsPlaying = true;
                int index = UnityEngine.Random.Range(0, m_soundsPlayed.Count - 1);
                AkSoundEngine.PostEvent((uint)m_soundsPlayed[index], gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
                m_soundsPlayed.RemoveAt(index);
                m_timer.Start(UnityEngine.Random.Range(m_timeBetweenSoundMin, m_timeBetweenSoundMax));

                if(m_soundsPlayed.Count == 0)
                {
                    foreach (Wwise_ID_Enum_EVENTS sound in m_sounds)
                    {
                        m_soundsPlayed.Add(sound);
                    }

                    m_soundsPlayed = Shuffle(m_soundsPlayed);
                }
            }
        }
	}

    void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
            m_soundIsPlaying = false;
    }
}
