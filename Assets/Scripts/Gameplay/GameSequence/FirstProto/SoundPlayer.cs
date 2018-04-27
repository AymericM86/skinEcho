using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : GameSequence
{
    [SerializeField]
    private Wwise_ID_Enum m_eventName;
    private bool m_isTerminated = false;

    public override bool IsTerminated()
    {
        return m_isTerminated;
    }

    // Use this for initialization
    public override void DoInStart ()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        AkSoundEngine.PostEvent((uint)m_eventName, gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
	}
	
    void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if(in_type == AkCallbackType.AK_EndOfEvent)
            m_isTerminated = true;
    }


    private void OnValidate()
    {
        
    }
}
