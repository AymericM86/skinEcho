using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundSetup : GameSequence
{
    [SerializeField]
    RandomSoundPlayer m_soundPlayer;

    public override bool IsTerminated()
    {
        m_soundPlayer.enabled = true;
        return true;
    }
}
