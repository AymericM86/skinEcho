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

    public override bool IsTerminated()
    {
        m_monster.gameObject.SetActive(true);
        m_monster.SetupRandom(m_minTimeCalm, m_maxTimeCalm, m_minTimeIrritated, m_maxTimeIrritated);

        return true;
    }
}
