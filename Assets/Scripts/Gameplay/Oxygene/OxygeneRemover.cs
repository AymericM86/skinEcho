using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygeneRemover : MonoBehaviour {

    [SerializeField]
    Timer m_timeBetweenOxygeneRemove;

    OxygeneGauge m_gauge;

	// Use this for initialization
	void Start ()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Oxygene");
        if (go == null)
        {
            Debug.LogError("No object with tag 'Oxygene' found");
            Debug.Break();
        }
        m_gauge = go.GetComponent<OxygeneGauge>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_timeBetweenOxygeneRemove.UpdateTimer();
        if (m_timeBetweenOxygeneRemove.IsTimedOut())
        {
            m_gauge.DecreaseOxygene();
            m_timeBetweenOxygeneRemove.Restart();
        }
	}
}
