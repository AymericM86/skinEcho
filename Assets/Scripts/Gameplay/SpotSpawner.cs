using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotSpawner : MonoBehaviour {

    [SerializeField]
    float m_radiusSpawn = 1;
    [SerializeField]
    float m_averageHeight = 1.1f;
    [SerializeField]
    VibrationSpot m_spot;
    [SerializeField]
    float m_timeToLock = 1;
    float m_timeElapsed = 0;
	
	// Update is called once per frame
	void Update ()
    {
        if (m_spot.IsTriggered())
        {
            m_timeElapsed += Time.deltaTime;
            if (m_timeElapsed >= m_timeToLock)
                RandomizePosition();
        }
        else
            m_timeElapsed = 0;
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,m_radiusSpawn);
    }

    private void RandomizePosition()
    {
        float x = Random.Range(-m_radiusSpawn,m_radiusSpawn) + transform.position.x;
        float z = Random.Range(-m_radiusSpawn,m_radiusSpawn) + transform.position.z;
        float y = Random.Range(-m_radiusSpawn/4,m_radiusSpawn/4) + m_averageHeight;

        m_spot.transform.position = new Vector3(x, y, z);
    }
}
