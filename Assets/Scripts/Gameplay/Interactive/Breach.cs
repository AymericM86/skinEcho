using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breach : MonoBehaviour
{

    [SerializeField]
    Transform m_startBreach;
    [SerializeField]
    Transform m_endBreach;
    [SerializeField]
    int m_nbSegmentBetweenSpot = 2;
    [SerializeField]
    Transform m_room;
    [SerializeField]
    GameObject m_vibrationSpot;
    [SerializeField]
    SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    SteamVR_TrackedObject m_controllerRight;


    // Use this for initialization
    void Start ()
    {
        List<Vector3> points = GetPoints();

        foreach(Vector3 point in points)
        {
            GameObject spot = Instantiate(m_vibrationSpot);
            spot.transform.position = point;
            spot.transform.parent = transform;
            VibrationBreachSpot component = spot.GetComponent<VibrationBreachSpot>();
            component.SetControllers(m_controllerLeft,m_controllerRight);
        }
	}

    List<Vector3> GetPoints()
    {
        Vector3 c = m_room.transform.position;
        float alpha = Vector3.Angle(m_startBreach.transform.position, m_endBreach.transform.position);
        List<Vector3> points = new List<Vector3>();
        Vector3 u = m_startBreach.transform.position - c;
        Vector3 v = m_endBreach.transform.position - c;

        if (u.sqrMagnitude != v.sqrMagnitude)
        {
            Debug.LogError("Error ! StartBreach and EndBreach must have the same magnitude ! ");
            return points;
        }

        for (int i = 0; i <= m_nbSegmentBetweenSpot; ++i)
        {
            Vector3 point = Vector3.Slerp(u, v, (float)i / m_nbSegmentBetweenSpot);
            //point.Normalize();
            Vector3 fromMiddle = c - point;
            //Debug.Log(fromMiddle.sqrMagnitude);
            points.Add(point + c);
        }

        return points;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_nbSegmentBetweenSpot <= 0 && m_startBreach != null && m_endBreach != null)
            return;

        List<Vector3> points = GetPoints();

        // Draw
        if(points.Count != 0)
        {
            Vector3 previous = points[0];
            points.RemoveAt(0);

            Gizmos.color = Color.blue;

            foreach (Vector3 point in points)
            {
                Gizmos.DrawLine(previous, point);
                previous = point;
            }

        }

    }
}
