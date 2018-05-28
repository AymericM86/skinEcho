using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [Header("SteamVR")]
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerRight;


    [Range(0, 100)]
    [SerializeField]
    float m_volumeSFX;
    [Range(0, 100)]
    [SerializeField]
    float m_volumeVoices;
    [SerializeField]
    float m_step = 5;

    // Use this for initialization
    void Start ()
    {
        /*AkSoundEngine.SetRTPCValue("", m_volumeVoices);
        AkSoundEngine.SetRTPCValue("", m_volumeSFX);*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerLeft.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.x >= 0.7f)
                m_volumeVoices += m_step;
            else if (touchpad.x <= -0.7f)
                m_volumeVoices -= m_step;
        }
        if (SteamVR_Controller.Input((int)m_controllerRight.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerRight.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.x >= 0.7f)
                m_volumeSFX += m_step;
            else if (touchpad.x <= -0.7f)
                m_volumeSFX -= m_step;
        }


        /*AkSoundEngine.SetRTPCValue("", m_volumeVoices);
        AkSoundEngine.SetRTPCValue("", m_volumeSFX);*/
    }
}
