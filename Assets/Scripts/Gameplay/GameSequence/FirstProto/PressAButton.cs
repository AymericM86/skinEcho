using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAButton : GameSequence {

    [SerializeField]
    private VibrationSpot m_button;

    public override bool IsTerminated()
    {
        return m_button.IsTriggered();
    }

    public override void Rearm()
    {
        m_button.Rearm();
    }
}
