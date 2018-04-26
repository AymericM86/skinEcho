using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VibrationBreachSpot))]
public class OxygeneRemover : MonoBehaviour {

    [SerializeField]
    Timer m_timeBetweenOxygeneRemove;

    OxygeneGauge m_gauge;

    VibrationBreachSpot m_spot;

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
        m_spot = GetComponent<VibrationBreachSpot>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_spot.IsTriggered())
            return;

        m_timeBetweenOxygeneRemove.UpdateTimer();
        if (m_timeBetweenOxygeneRemove.IsTimedOut())
        {
            m_gauge.DecreaseOxygene();
            m_timeBetweenOxygeneRemove.Restart();
        }
	}
}
