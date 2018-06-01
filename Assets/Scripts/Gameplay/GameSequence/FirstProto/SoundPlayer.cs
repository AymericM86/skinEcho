using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : GameSequence
{
    [SerializeField]
    private Wwise_ID_Enum_EVENTS m_eventName;
    private bool m_isTerminated = false;
    [SerializeField]
    Timer m_timeBeforeNext;

    [Header("Vibrations")]
    [SerializeField]
    bool m_addVibration = false;
    [SerializeField]
    ushort m_vibrationIntensity = 3000;
    [Header("Not necessary if add vibration is not checked")]
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerRight;


    public override bool IsTerminated()
    {
        return m_isTerminated && m_timeBeforeNext.IsTimedOut();
    }

    // Use this for initialization
    public override void DoInStart ()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        if(body)
        {
            body.isKinematic = true;
            body.useGravity = false;
        }
        AkSoundEngine.PostEvent((uint)m_eventName, gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);

        if(m_addVibration)
        {
            StartCoroutine(LongVibrationControllerLeft());
            StartCoroutine(LongVibrationControllerRight());

        }
    }

    public override void DoInUpdate()
    {
        if(m_isTerminated)
        {
            m_timeBeforeNext.UpdateTimer();
        }
    }

    void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            m_isTerminated = true;
            m_timeBeforeNext.Start();
        }
    }


    private void OnValidate()
    {
        
    }

    public override void Rearm()
    {
        m_isTerminated = false;
    }

    
    IEnumerator LongVibrationControllerLeft()
    {

        while (!m_isTerminated)
        {
            SteamVR_Controller.Input((int)m_controllerLeft.index).TriggerHapticPulse(m_vibrationIntensity);
            yield return null;
        }
    }

    IEnumerator LongVibrationControllerRight()
    {
        while(!m_isTerminated)
        {
            SteamVR_Controller.Input((int)m_controllerRight.index).TriggerHapticPulse(m_vibrationIntensity);
            yield return null;
        }
    }
}
