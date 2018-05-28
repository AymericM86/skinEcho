using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestVibrationSpot : VibrationSpot {
    public static int s_numberOfSpots = 0;
    public static float s_totalTimeInSpot = 0;
    public static float s_totalTimeElapsed = 0;

    private float m_timeInSpot = 0;
    private float m_timeElepased = 0;

    private bool m_leftIn = false;
    private bool m_rightIn = false;

    private bool m_writed = false;

    private void Start()
    {
        s_numberOfSpots++;
    }

    public override void InArea(bool leftHand, bool rightHand)
    {
        if (leftHand || rightHand)
        {
            m_timeInSpot += Time.deltaTime;
            s_totalTimeInSpot += Time.deltaTime;
        }
    }

    public override void InSpot(bool leftHand, bool rightHand)
    {
        if (m_isFound)
            return;

        m_timeElepased += Time.deltaTime;
        s_totalTimeElapsed += Time.deltaTime;

        if (!leftHand)
            m_leftIn = false;

        if (!rightHand)
            m_rightIn = false;

        if (!m_playingSound)
        {
            if (leftHand && SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
            {
                AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
                m_playingSound = true;
                WriteData();
            }
            else if (leftHand && !m_leftIn)
            {
                // Todo : sound on enter area

                AkSoundEngine.PostEvent("Play_SFX_Button_Touch", gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
                m_playingSound = true;
                m_leftIn = true;
            }
        }

        if (!m_playingSound)
        {
            if (rightHand && SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger())
            {
                AkSoundEngine.PostEvent("Play_SFX_Button_Press", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateFinalSound, null);
                m_playingSound = true;
                WriteData();
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

    private void WriteData()
    {
        if(m_writed == false)
            WriteInFile.Write("PlaytestDatas/", "Time to complete :" + m_timeInSpot.ToString() + " Time in area : " + m_timeElepased.ToString());
    }
}
