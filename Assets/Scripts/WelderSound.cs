using System.Collections;
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

    // Update is called once per frame
    void Update ()
    {
		if(Controller.GetHairTriggerDown())
        {
            AkSoundEngine.PostEvent("Play_SFX_Welder_Air", gameObject);
        }
        if(Controller.GetHairTriggerUp())
        {
            AkSoundEngine.PostEvent("Stop_SFX_Welder_Air", gameObject);
        }
	}
}
