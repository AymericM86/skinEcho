using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBreach : GameSequence {
    [SerializeField]
    Breach m_breach;

    public override bool IsTerminated()
    {
        return m_breach.IsTriggerred();
    }

    public override void Rearm()
    {
        m_breach.Rearm();
    }
}
