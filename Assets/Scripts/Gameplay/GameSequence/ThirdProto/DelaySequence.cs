using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaySequence : GameSequence {

    [SerializeField]
    Timer m_timer;

    public override bool IsTerminated()
    {
        m_timer.UpdateTimer();
        return m_timer.IsTimedOut();
    }

    public override void Rearm()
    {
        m_timer.Restart();
    }
}
