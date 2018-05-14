using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour {

    [SerializeField]
    Vector3 m_direction;
    [SerializeField]
    float m_distance;
    [SerializeField]
    float m_cycleTime;

    Vector3 m_initialPosition;
    float m_timeElapsed;


    private void Start()
    {
        m_initialPosition = transform.position;
        m_timeElapsed = 0;
    }
    
    void Update ()
    {
        m_timeElapsed += Time.deltaTime;
        transform.position = m_initialPosition + Mathf.Lerp(-m_distance, m_distance, Mathf.PingPong(m_timeElapsed, m_cycleTime) / m_cycleTime)  * m_direction;
	}

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Vector3 direction = m_direction.normalized * m_distance;
        Color color = Color.blue;
        float arrowHeadLength = 0.1f;
        float arrowHeadAngle = 20;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);

        Gizmos.DrawRay(pos, -direction);
        right = Quaternion.LookRotation(-direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        left = Quaternion.LookRotation(-direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos - direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos - direction, left * arrowHeadLength);
    }
}
