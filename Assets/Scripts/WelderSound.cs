﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelderSound : MonoBehaviour
{ 
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    int m_spotsInContact = 0;

    public void AddSpot()
    {
        m_spotsInContact++;
    }

    public void RemoveSpot()
    {
        m_spotsInContact--;
        if (m_spotsInContact < 0)
            m_spotsInContact = 0;
    }

    private bool InContact()
    {
        return m_spotsInContact > 0;
    }

    bool m_previouslyInContact = false;

    // Update is called once per frame
    void Update ()
    {
        if (!CompareTag("Blowtorch"))
            return;

        if (Controller.GetHairTriggerDown())
        {
            if (!InContact())
                AkSoundEngine.PostEvent("Play_SFX_Welding_Air", gameObject);
            else
                AkSoundEngine.PostEvent("Play_SFX_Welding", gameObject);
        }
        else if(Controller.GetHairTrigger())
        {

            if (InContact() != m_previouslyInContact)
            {
                if (!InContact())
                {
                    AkSoundEngine.PostEvent("Stop_SFX_Welding", gameObject);
                    AkSoundEngine.PostEvent("Play_SFX_Welding_Air", gameObject);
                }
                else
                {
                    AkSoundEngine.PostEvent("Stop_SFX_Welding_Air", gameObject);
                    AkSoundEngine.PostEvent("Play_SFX_Welding", gameObject);
                }
            }
        }

        if(Controller.GetHairTriggerUp())
        {
            AkSoundEngine.PostEvent("Stop_SFX_Welding_Air", gameObject);
            AkSoundEngine.PostEvent("Stop_SFX_Welding", gameObject);
            Reset();
        }

        m_previouslyInContact = InContact();
	}

    public void Reset()
    {
        m_spotsInContact = 0;
    }
}
