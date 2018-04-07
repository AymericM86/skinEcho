using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : GameSequence {

    [SerializeField]
    private Item m_item;

    public override bool IsTerminated()
    {
        return m_item.IsTriggered();
    }
}
