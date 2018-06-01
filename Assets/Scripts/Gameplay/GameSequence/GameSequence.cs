using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSequence : MonoBehaviour {

    [SerializeField]
    private GameSequence m_nextSequence;

    private bool m_isRunning = false;
    [SerializeField]
    private bool m_isFirstSequence = false;
    [SerializeField]
    private bool m_isCheckpoint = false;

    private OxygeneGauge m_gauge;

    private void Start()
    {
        m_gauge = GameObject.FindGameObjectWithTag("Oxygene").GetComponent<OxygeneGauge>();

        m_isRunning = false;

        Rearm();

        gameObject.SetActive(false);
        if (m_isFirstSequence)
            Launch();
    }

    private void Update()
    {
        if(m_isRunning)
        {
            DoInUpdate();
        }

        LaunchNextSequence();
    }

    public void Launch()
    {
        m_isRunning = true;
        gameObject.SetActive(true);
        DoInStart();
        if (m_isCheckpoint)
            m_gauge.SetCheckPoint(gameObject);
    }

    private void LaunchNextSequence()
    {
        if (IsTerminated())
        {
            if (m_nextSequence)
            {
                m_nextSequence.Launch();
            }
            gameObject.SetActive(false);
        }
    }

    public abstract bool IsTerminated();

    public virtual void DoInUpdate()
    {

    }

    public virtual void DoInStart()
    {

    }

    public GameSequence GetNextSequence()
    {
        return m_nextSequence;
    }

    public virtual void Rearm()
    {
        
    }
}
