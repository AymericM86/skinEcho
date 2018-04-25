using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationButton : VibrationSpot {

    bool m_playingSound = false;

    public void Start()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.useGravity = false;
    }

    public override void InSpot(bool leftHand, bool rightHand)
    {
        if (m_isFound)
            return;

        if (leftHand && SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
        {
            AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
            m_playingSound = true;
        }
        else if(leftHand && !m_playingSound)
        {
            // Todo : sound on enter area

            /*AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
            m_playingSound = true;*/
        }


        if (m_isFound)
            return;

        if (rightHand && SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger())
        {
            AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
            m_playingSound = true;
        }
        else if(rightHand && !m_playingSound)
        {
            // Todo : sound on enter area

            /*AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
            m_playingSound = true;*/
        }
            
    }

    void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
            m_playingSound = false;
    }

    void TerminateFinalSound(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            m_isFound = true;
            m_playingSound = false;
        }
    }
}
