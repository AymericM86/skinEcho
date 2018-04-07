using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSequence : MonoBehaviour {

    [SerializeField]
    private GameSequence m_nextSequence;

    private bool m_isRunning = false;
    [SerializeField]
    private bool m_isFirstSequence = false;

    private void Start()
    {
        m_isRunning = m_isFirstSequence;
        if(!m_isRunning)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(m_isRunning)
        {
            DoInUpdate();
        }

        LaunchNextSequence();
    }

    private void Launch()
    {
        m_isRunning = true;
        gameObject.SetActive(true);
    }

    private void LaunchNextSequence()
    {
        if (IsTerminated())
        {
            if (m_nextSequence)
            {
                m_nextSequence.Launch();
            }
            Destroy(gameObject);
        }
    }

    public abstract bool IsTerminated();

    public virtual void DoInUpdate()
    {

    }
}
