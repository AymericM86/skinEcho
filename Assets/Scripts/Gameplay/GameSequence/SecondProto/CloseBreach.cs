using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBreach : GameSequence {
    [SerializeField]
    Breach m_breach;

    public override bool IsTerminated()
    {
        if (m_breach.IsTriggerred())
            m_breach.StopSound();
        return m_breach.IsTriggerred();
    }

    public override void Rearm()
    {
        m_breach.Rearm();
    }

    public override void DoInStart()
    {
        m_breach.PlaySound();
    }
}
