using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBreach : GameSequence
{
    [SerializeField]
    Breach m_breach;

    public override bool IsTerminated()
    {
        AkSoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.RTPC_BREACH_SIZE, 100.0f);
        m_breach.PlaySound();
        return true;
    }
}
