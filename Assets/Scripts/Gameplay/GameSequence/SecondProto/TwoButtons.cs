using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoButtons : GameSequence
{

    [SerializeField]
    VibrationButton m_button1;
    [SerializeField]
    VibrationButton m_button2;

    public override bool IsTerminated()
    {
        return m_button1.IsTriggered() && m_button2.IsTriggered();
    }

    public override void DoInUpdate()
    {

    }

    public override void DoInStart()
    {

    }

    public override void Rearm()
    {
        m_button1.Rearm();
        m_button2.Rearm();
    }
}
