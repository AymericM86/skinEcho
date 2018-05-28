using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoButtons : GameSequence
{

    [SerializeField]
    VibrationButton m_button1;
    [SerializeField]
    VibrationButton m_button2;

    [SerializeField]
    float m_refillValue = 100;
    OxygeneGauge m_oxygene;

    bool m_triggered = false;

    public override bool IsTerminated()
    {
        return m_triggered;
    }

    public override void DoInUpdate()
    {
        if(m_button1.IsTriggered() && m_button2.IsTriggered())
        {
            m_triggered = true;
            m_oxygene.Refill(m_refillValue);
        }

    }

    public override void DoInStart()
    {
        m_oxygene = GameObject.FindGameObjectWithTag("Oxygene").GetComponent<OxygeneGauge>();
    }

    public override void Rearm()
    {
        m_button1.Rearm();
        m_button2.Rearm();
    }
}
