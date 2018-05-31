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

    [Header("SteamVR")]
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerRight;

    public override bool IsTerminated()
    {

        if(m_triggered)
        {
            m_controllerLeft.tag = "Blowtorch";
            m_controllerRight.tag = "Blowtorch";
        }
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
        m_controllerLeft.tag = "Untagged";
        m_controllerRight.tag = "Untagged";

    }

    public override void Rearm()
    {
        m_button1.Rearm();
        m_button2.Rearm();
    }
}
