using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : GameSequence {
    [SerializeField]
    RandomSoundPlayer m_soundPlayer;
    [SerializeField]
    RandomVibration m_vibrationRandom;
    [SerializeField]
    Monster m_monster;

    public override bool IsTerminated()
    {
        m_soundPlayer.enabled = false;
        m_vibrationRandom.enabled = false;
        m_monster.gameObject.SetActive(false);
        AkSoundEngine.PostEvent("Stop_SFX_Monster_Calm", m_monster.gameObject);
        AkSoundEngine.PostEvent("Stop_SFX_Monster_Agressive", m_monster.gameObject);
        return true;
    }
}
