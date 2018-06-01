using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBreach : GameSequence
{
    [SerializeField]
    Breach m_breach;

    [SerializeField]
    Timer m_timeBeforeNextSequence;
    bool m_activated = false;

    public override bool IsTerminated()
    {
        m_timeBeforeNextSequence.UpdateTimer();
        if (!m_activated)
        {
            AkSoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.RTPC_BREACH_SIZE, 100.0f);
            m_breach.PlaySound();
            m_activated = true;
        }

        return m_timeBeforeNextSequence.IsTimedOut();
    }

    public override void Rearm()
    {
        m_timeBeforeNextSequence.Restart();
        m_activated = false;
        m_breach.StopSound();
    }
}
