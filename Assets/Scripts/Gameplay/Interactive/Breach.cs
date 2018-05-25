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

    VibrationBreachSpot m_middleBreach = null;

    // Use this for initialization
    void Start ()
    {
        List<Vector3> points = GetPoints();

        int cpt = 0;
        foreach(Vector3 point in points)
        {
            GameObject spot = Instantiate(m_vibrationSpot);
            spot.name = "Breach spot " + cpt;
            spot.transform.position = point;
            spot.transform.parent = transform;
            VibrationBreachSpot component = spot.GetComponent<VibrationBreachSpot>();
            component.SetControllers(m_controllerLeft,m_controllerRight);

            if (cpt == points.Count / 2)
            {
                component.SetSoundSource();
                m_middleBreach = component;
            
            }
            cpt++;

        }
	}

    private void Update()
    {
        AkSoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.RTPC_BREACH_SIZE, 100.0f - GetCompletionPercentage());

        float minDist = 0.2f;
        float maxDist = 0.7f;

        float distance = 0;
        if (m_controllerLeft.CompareTag("Blowtorch"))
            distance = Vector3.Distance(m_controllerLeft.transform.position, m_middleBreach.transform.position);
        else
            distance = Vector3.Distance(m_controllerRight.transform.position, m_middleBreach.transform.position);

        print(distance);

        AkSoundEngine.SetRTPCValue("RTPC_Welder_Air", Mathf.Lerp(100, 0, distance - minDist / (maxDist - minDist)));
    }

    List<Vector3> GetPoints()
    {
        Vector3 c = m_room.transform.position;
        float alpha = Vector3.Angle(m_startBreach.transform.position, m_endBreach.transform.position);
        List<Vector3> points = new List<Vector3>();
        Vector3 u = m_startBreach.transform.position - c;
        Vector3 v = m_endBreach.transform.position - c;
        
        if (Mathf.Abs(u.sqrMagnitude - v.sqrMagnitude) >= 0.01f)
        {
            Debug.LogError("Error ! StartBreach and EndBreach must have the same magnitude ! (u : "+u.sqrMagnitude+"; v : "+v.sqrMagnitude+")");
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

    public bool IsTriggerred()
    {
        VibrationBreachSpot[] spots = GetComponentsInChildren<VibrationBreachSpot>();

        foreach(VibrationBreachSpot spot in spots)
        {
            if (!spot.IsTriggered())
                return false;
        }
        return true;
    }

    public float GetCompletionPercentage()
    {
        float result = 0;
        VibrationBreachSpot[] spots = GetComponentsInChildren<VibrationBreachSpot>();
        float valuePerPoint = 100.0f / (float)spots.Length;

        foreach(VibrationBreachSpot spot in spots)
        {
            result += spot.GetCompletionLevel() * valuePerPoint;
        }

        return result;
    }

    public void NormalizePositions()
    {
        m_endBreach.position -= m_room.position;
        m_startBreach.position -= m_room.position;

        m_endBreach.position = m_endBreach.position.normalized;
        m_endBreach.position /= 2; 
        m_startBreach.position = m_startBreach.position.normalized;
        m_startBreach.position /= 2;

        m_endBreach.position += m_room.position;
        m_startBreach.position += m_room.position;
    }

    public void Rearm()
    {
        VibrationBreachSpot[] spots = GetComponentsInChildren<VibrationBreachSpot>();
        foreach(VibrationBreachSpot spot in spots)
            spot.Rearm();
    }
}
