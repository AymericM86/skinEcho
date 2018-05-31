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

    public override bool IsTerminated()
    {
        return m_isTerminated && m_timeBeforeNext.IsTimedOut();
    }

    // Use this for initialization
    public override void DoInStart ()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        AkSoundEngine.PostEvent((uint)m_eventName, gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
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
}
