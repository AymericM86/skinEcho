using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : GameSequence {

    RandomSoundPlayer m_soundPlayer;

    RandomVibration m_vibrationRandom;

    public override bool IsTerminated()
    {
        m_soundPlayer.enabled = false;
        m_vibrationRandom.enabled = false;
        return true;
    }
}
