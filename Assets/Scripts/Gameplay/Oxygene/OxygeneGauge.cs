﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OxygeneStage
{
    public float m_percentage;
    public string m_soundName;
    private bool m_triggered = false;

    public void Restart(float oxygeneLevel)
    {
        m_triggered = oxygeneLevel <= m_percentage;
    }

    public void Trigger()
    {
        m_triggered = true;
    }

    public bool IsTriggered()
    {
        return m_triggered;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class OxygeneGauge : MonoBehaviour
{
    [SerializeField]
    OxygeneStage[] m_stages;

    [SerializeField]
    float m_oxygeneToRemove = 1;

    [SerializeField]
    float m_oxygeneLevel = 100;

    [SerializeField]
    float m_oxygeneToAddAfterDeath = 10;

    private GameObject m_checkpoint;
    private float m_oxygeneAtCheckPoint = 100;

    [SerializeField]
    private Timer m_timeBeforeRespawn;

    void SetStages()
    {
        foreach (OxygeneStage stage in m_stages)
        {
            stage.Restart(m_oxygeneLevel);
        }
    }

    private void Awake()
    {
        gameObject.tag = "Oxygene";
    }

    // Use this for initialization
    void Start ()
    {
        SetStages();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(m_oxygeneLevel <= 0)
        {
            // Todo die
            Debug.Log("Dead ! ");
            AkSoundEngine.PostEvent((int)Wwise_ID_Enum_EVENTS.PLAY_DEATH, gameObject);
            m_timeBeforeRespawn.UpdateTimer();
            if(m_timeBeforeRespawn.IsTimedOut())
            {
                m_oxygeneLevel = Mathf.Clamp(m_oxygeneToAddAfterDeath + m_oxygeneAtCheckPoint, 0, 100);
                GameSequence sequence = m_checkpoint.GetComponent<GameSequence>();
                while (sequence != null)
                {
                    sequence.Rearm();
                    sequence.gameObject.SetActive(false);
                    sequence = sequence.GetNextSequence();
                }
                m_checkpoint.SetActive(true);
                m_timeBeforeRespawn.Restart();
            }
            
        }

		foreach(OxygeneStage stage in m_stages)
        {
            if (stage.IsTriggered())
                continue;

            if(stage.m_percentage >= m_oxygeneLevel)
            {
                stage.Trigger();
                AkSoundEngine.PostEvent(stage.m_soundName, gameObject);
            }
        }
	}

    public void DecreaseOxygene()
    {
        RemoveOxygene(m_oxygeneToRemove);
    }

    public void SetCheckPoint(GameObject go)
    {
        m_checkpoint = go;
        m_oxygeneAtCheckPoint = m_oxygeneLevel;
    }

    public void Refill(float value)
    {
        m_oxygeneLevel = value;
        if (m_oxygeneLevel > 100)
            m_oxygeneLevel = 100;
    }

    public void RemoveOxygene(float remove)
    {
        m_oxygeneLevel -= remove;
        if (m_oxygeneLevel < 0)
            m_oxygeneLevel = 0;
    }
}
