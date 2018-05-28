using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVibrationSetup : GameSequence
{

    [SerializeField]
    RandomVibration m_randomVibration;

    [Header("Define config")]
    [SerializeField]
    float m_vibrationDuration = 10.0f;
    [SerializeField]
    float m_minTimeBeforeVibration = 5.0f;
    [SerializeField]
    float m_maxTimeBeforeVibration = 15.0f;

    public override bool IsTerminated()
    {
        m_randomVibration.Setup(m_vibrationDuration, m_minTimeBeforeVibration, m_maxTimeBeforeVibration);
        return true; 
    }
}
