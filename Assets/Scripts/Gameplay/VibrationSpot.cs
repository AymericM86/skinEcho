using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationSpot : MonoBehaviour {

    [SerializeField]
    private float m_distanceToStartDetect = 2;
    [SerializeField]
    private float m_distanceMaxRumble = 0.2f;
    [SerializeField]
    private float m_distanceToNearestController;
    [SerializeField]
    private float m_timeBetweenVibrationAtStart = 0.5f;


    [Header("SteamVR")]
    [SerializeField]
    private SteamVR_TrackedObject m_trackedObj1;
    [SerializeField]
    private SteamVR_TrackedObject m_trackedObj2;

    private float m_timeElapsed1 = 0;
    private float m_timeElapsed2 = 0;
    private bool m_vibrate1 = false;
    private bool m_vibrate2 = false;
    private bool m_isFound = false;
    private float m_distanceStartVibratingController1 = 0;
    private float m_distanceStartVibratingController2 = 0;
    private float m_vibrationDurationController1 = 0;
    private float m_vibrationDurationController2 = 0;

    private bool m_controller1IsVibrating = false;
    private bool m_controller2IsVibrating = false;

    private Material m_debugMaterial;

    private void Start()
    {
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        m_debugMaterial = renderer.material;
    }

    // Update is called once per frame
    void Update ()
    {
        m_distanceToNearestController = float.MaxValue;
        float distance = 0;
        bool foundController1 = false;
        bool foundController2 = false;

        if ((distance = GetDistance(m_trackedObj1)) <= m_distanceToStartDetect)
        {
            m_distanceToNearestController = distance;

            if (IsInArea(distance))
            {
                foundController1 = true;
            }

            if (!m_controller1IsVibrating)
            {
                m_distanceStartVibratingController1 = distance;
                m_vibrationDurationController1 -= Time.deltaTime;
            }

            if (m_controller1IsVibrating || (!m_controller1IsVibrating && m_vibrationDurationController1 <= 0))
                StartCoroutine(LongVibrationController1());
        }

        if ((distance = GetDistance(m_trackedObj2)) <= m_distanceToStartDetect)
        {
            if(m_distanceToNearestController > distance)
                m_distanceToNearestController = distance;

            if (IsInArea(distance))
            {
                foundController2 = true;
            }

            if (!m_controller2IsVibrating)
            {
                m_distanceStartVibratingController2 = distance;
                m_vibrationDurationController2 -= Time.deltaTime;
            }

            if(m_controller2IsVibrating || (!m_controller2IsVibrating && m_vibrationDurationController2 <= 0))
                StartCoroutine(LongVibrationController2());

        }

        // Debug
        if (foundController1 || foundController2)
        {
            m_debugMaterial.color = Color.green;
            m_isFound = true;
        }
        else
        {
            m_debugMaterial.color = Color.white;
            m_isFound = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_distanceToStartDetect);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_distanceMaxRumble);
    }

    private bool IsInArea(float distance)
    {
        return distance <= m_distanceMaxRumble;
    }

    private float GetDistance(SteamVR_TrackedObject hand)
    {
        return Vector3.Distance(transform.position, hand.transform.position);
    }

    IEnumerator LongVibrationController1()
    {
        m_controller1IsVibrating = true;
        if (!IsInArea(m_distanceStartVibratingController1))
        {
            m_vibrationDurationController1 = Mathf.Lerp(0, m_timeBetweenVibrationAtStart, (m_distanceStartVibratingController1 / m_distanceToStartDetect));
            for (float i = 0; i < m_vibrationDurationController1; i += Time.deltaTime)
            {
                SteamVR_Controller.Input((int)m_trackedObj1.index).TriggerHapticPulse((ushort)Mathf.Lerp(500, 2000, 1 - ((m_distanceStartVibratingController1 - m_distanceMaxRumble) / (m_distanceToStartDetect - m_distanceMaxRumble))));
                yield return null;
            }
        }
        else
            SteamVR_Controller.Input((int)m_trackedObj1.index).TriggerHapticPulse(3500);

        m_controller1IsVibrating = false;
    }

    IEnumerator LongVibrationController2()
    {
        m_controller2IsVibrating = true;
        if (!IsInArea(m_distanceStartVibratingController2))
        {
            m_vibrationDurationController2 = Mathf.Lerp(0, m_timeBetweenVibrationAtStart, (m_distanceStartVibratingController2 / m_distanceToStartDetect));
            for (float i = 0; i < m_vibrationDurationController2; i += Time.deltaTime)
            {
                SteamVR_Controller.Input((int)m_trackedObj2.index).TriggerHapticPulse((ushort)Mathf.Lerp(500, 2000, 1 - ((m_distanceStartVibratingController2 - m_distanceMaxRumble )/ (m_distanceToStartDetect - m_distanceMaxRumble))));
                yield return null;
            }
        }
        else
            SteamVR_Controller.Input((int)m_trackedObj2.index).TriggerHapticPulse(3500);

        m_controller2IsVibrating = false;
    }

    public bool IsTriggered()
    {
        return m_isFound;
    }
}
