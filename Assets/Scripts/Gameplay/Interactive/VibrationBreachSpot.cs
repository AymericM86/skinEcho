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

        if (leftHand && m_controllerLeft.CompareTag("Blowtorch") && SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
        {
            if (m_timerLeft.GetRatio() == 0)
                m_controllerLeft.GetComponent<WelderSound>().AddSpot();

            m_timerLeft.UpdateTimer();
            m_isFound = m_timerLeft.IsTimedOut();
            if(m_isFound)
            {
                m_controllerLeft.GetComponent<WelderSound>().RemoveSpot();
                return;
            }
        }
        else
        {
            if(m_timerLeft.GetRatio() != 0)
                m_controllerLeft.GetComponent<WelderSound>().RemoveSpot();
            m_timerLeft.Restart();
        }

        if (rightHand && m_controllerRight.CompareTag("Blowtorch") && SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger())
        {
            if (m_timerRight.GetRatio() == 0)
                m_controllerRight.GetComponent<WelderSound>().AddSpot();

            m_timerRight.UpdateTimer();
            m_isFound = m_timerRight.IsTimedOut();

            if (m_isFound)
            {
                m_controllerRight.GetComponent<WelderSound>().RemoveSpot();
                return;
            }
        }
        else
        {
            if (m_timerRight.GetRatio() != 0)
                m_controllerRight.GetComponent<WelderSound>().RemoveSpot();
            m_timerRight.Restart();
        }
    }

    public float GetCompletionLevel()
    {
        if (IsTriggered())
            return 1;

        if (m_timerLeft.GetRatio() > m_timerRight.GetRatio())
            return m_timerLeft.GetRatio();
        else
            return m_timerRight.GetRatio();
    }
}
