using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSetup : GameSequence
{

    [SerializeField]
    Monster m_monster;

    [Header("Define config")]
    [SerializeField]
    float m_calmCycle = 10.0f;
    [SerializeField]
    float m_irritatedCycle = 15.0f;

    [SerializeField]
    bool m_disableMonsterOnCheckpoint = true;

    public override bool IsTerminated()
    {
        m_monster.gameObject.SetActive(true);
        m_monster.Setup(m_calmCycle, m_irritatedCycle);

        return true;
    }

    public override void Rearm()
    {
        if (m_disableMonsterOnCheckpoint)
        {
            m_monster.gameObject.SetActive(false);
            AkSoundEngine.PostEvent("Stop_SFX_Monster_Calm", m_monster.gameObject);
            AkSoundEngine.PostEvent("Stop_SFX_Monster_Aggressive", m_monster.gameObject);
            AkSoundEngine.PostEvent("Stop_SFX_Radar", m_monster.gameObject);
        }
    }
}
