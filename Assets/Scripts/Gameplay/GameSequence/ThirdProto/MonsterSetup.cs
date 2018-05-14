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

    public override bool IsTerminated()
    {
        m_monster.Setup(m_calmCycle, m_irritatedCycle);

        return true;
    }
}
