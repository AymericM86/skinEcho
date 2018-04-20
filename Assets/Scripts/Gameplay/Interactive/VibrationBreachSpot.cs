using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationBreachSpot : VibrationSpot
{
    [SerializeField]
    float m_timeToLock = 0.5f;
    Timer m_timerRight = new Timer();
    Timer m_timerLeft = new Timer();

    public void Start()
    {
        m_timerLeft.Start(m_timeToLock);
        m_timerRight.Start(m_timeToLock);
    }

    public override void InSpot(bool leftHand, bool rightHand)
    {
        if (m_isFound)
            return;

        if (leftHand && m_controllerLeft.CompareTag("Blowtorch"))
        {
            m_timerLeft.UpdateTimer();
            m_isFound = m_timerLeft.IsTimedOut();
        }
        else
            m_timerLeft.Restart();

        if (m_isFound)
            return;

        if (rightHand && m_controllerRight.CompareTag("Blowtorch"))
        {
            m_timerRight.UpdateTimer();
            m_isFound = m_timerRight.IsTimedOut();
        }
        else
            m_timerRight.Restart();
    }
}
