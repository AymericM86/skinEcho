using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationButton : VibrationSpot {

    bool m_leftIn = false;
    bool m_rightIn = false;

    public void Start()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.useGravity = false;
    }

    public override void InSpot(bool leftHand, bool rightHand)
    {
        if (m_isFound)
            return;

        if (!leftHand)
            m_leftIn = false;

        if (!rightHand)
            m_rightIn = false;

        if(!m_playingSound)
        {
            if (leftHand && SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
            {
                AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
                m_playingSound = true;
            }
            else if (leftHand && !m_leftIn)
            {
                // Todo : sound on enter area

                AkSoundEngine.PostEvent("Play_SFX_Button_Touch", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
                m_playingSound = true;
                m_leftIn = true;
            }
        }

        if(!m_playingSound)
        {
            if (rightHand && SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger())
            {
                AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
                m_playingSound = true;
            }
            else if (rightHand && !m_rightIn)
            {
                // Todo : sound on enter area

                AkSoundEngine.PostEvent("Play_SFX_Button_Touch", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
                m_playingSound = true;
                m_rightIn = true;
            }
        }
            
    }
}
