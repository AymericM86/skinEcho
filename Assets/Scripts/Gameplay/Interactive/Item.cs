using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : VibrationSpot
{
    public override void InSpot(bool leftHand, bool rightHand)
    {
        if (m_isFound)
            return;

        if (!m_playingSound)
        {
            if (leftHand)
            {
                if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
                {
                    AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
                    m_playingSound = true;
                    
                    m_controllerLeft.gameObject.tag = gameObject.tag;
                }
                else
                {
                    // Todo : sound on enter area

                    /*AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
                    m_playingSound = true;*/
                }
            }
        }

        if (!m_playingSound)
        {
            if (rightHand)
            {
                if (SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger())
                {
                    AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
                    m_playingSound = true;

                    m_controllerRight.gameObject.tag = gameObject.tag;
                }
                else
                {
                    // Todo : sound on enter area

                    /*AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
                    m_playingSound = true;*/
                }
            }
        }
    }
    
}
