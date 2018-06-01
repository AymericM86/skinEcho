using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSetupRandom : GameSequence {

    [SerializeField]
    Monster m_monster;

    [Header("Random config")]
    [SerializeField]
    float m_minTimeIrritated = 5.0f;
    [SerializeField]
    float m_maxTimeIrritated = 10.0f;

    [SerializeField]
    float m_minTimeCalm = 10.0f;
    [SerializeField]
    float m_maxTimeCalm = 15.0f;

    [SerializeField]
    bool m_disableMonsterOnCheckpoint = true;

    public override bool IsTerminated()
    {
        m_monster.gameObject.SetActive(true);
        m_monster.SetupRandom(m_minTimeCalm, m_maxTimeCalm, m_minTimeIrritated, m_maxTimeIrritated);

        return true;
    }

    public override void Rearm()
    {
        if (m_disableMonsterOnCheckpoint)
        {
            m_monster.gameObject.SetActive(false);
            AkSoundEngine.PostEvent("Stop_SFX_Monster_Calm", m_monster.gameObject);
            AkSoundEngine.PostEvent("Stop_SFX_Monster_Aggressive", m_monster.gameObject);
        }
    }
}
