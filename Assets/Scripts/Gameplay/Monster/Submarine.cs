using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour {

    [SerializeField]
    float m_swayAngle = 25.0f;
    [SerializeField]
    Timer m_swayLenght;
    [SerializeField]
    Breach[] m_breaches;
    
	// Use this for initialization
	void Start () {
        gameObject.tag = "Submarine";
        foreach(Breach breach in m_breaches)
        {
            breach.gameObject.SetActive(false);
        }

        while (!m_swayLenght.IsTimedOut())
            m_swayLenght.UpdateTimer();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(!m_swayLenght.IsTimedOut())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(-m_swayAngle, m_swayAngle, Mathf.PingPong(m_swayLenght.GetRatio(), 0.2f) * 5));
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

	}

    public void OpenBreach()
    {
        foreach(Breach breach in m_breaches)
        {
            if(!breach.gameObject.activeSelf)
            {
                breach.gameObject.SetActive(true);
                return;
            }
        }

        Debug.LogWarning("All breaches open");
    }

    public void StartSway()
    {
        m_swayLenght.Restart();
    }
}
