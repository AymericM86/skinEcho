using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAButton : GameSequence {

    [SerializeField]
    private VibrationButton m_button;

    public override bool IsTerminated()
    {
        return m_button.IsTriggered();
    }
}
