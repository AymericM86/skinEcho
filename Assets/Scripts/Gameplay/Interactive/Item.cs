using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : VibrationSpot
{
    public override void InSpot(bool leftHand, bool rightHand)
    {
        if(leftHand)
        {
            if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
            {
                m_isFound = true;
                m_controllerLeft.gameObject.tag = gameObject.tag;
            }
        }
        if (!m_isFound && rightHand)
        {
            if (SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger())
            {
                m_isFound = true;
                m_controllerRight.gameObject.tag = gameObject.tag;
            }
        }
    }
    
}
