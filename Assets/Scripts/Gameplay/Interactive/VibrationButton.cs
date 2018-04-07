using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationButton : VibrationSpot {

    public override void InSpot(bool leftHand, bool rightHand)
    {
        if(leftHand || rightHand)
        {
            m_isFound = true;
        }
    }

    public override void NotInSpot(bool leftHand, bool rightHand)
    {
        
    }
}
