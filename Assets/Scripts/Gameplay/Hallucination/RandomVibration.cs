using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVibration : MonoBehaviour
{
    [SerializeField]
    private SteamVR_TrackedObject m_leftController;
    [SerializeField]
    private SteamVR_TrackedObject m_rightController;
    [SerializeField]
    private float m_vibrationDuration;

    [Header("Vibration cycle")]
    [SerializeField]
    private float m_minTime;
    [SerializeField]
    private float m_maxTime;

    Timer m_timeBeforeVibration = new Timer();

    bool m_controllerRightIsVibrating = false;
    bool m_controllerLeftIsVibrating = false;

    private void Start()
    {
        SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        m_timeBeforeVibration.UpdateTimer();
        if (m_timeBeforeVibration.IsTimedOut())
        {
            if (!m_leftController.CompareTag("Blowtorch"))
                StartCoroutine(LongVibrationControllerLeft());

            if (!m_rightController.CompareTag("Blowtorch"))
                StartCoroutine(LongVibrationControllerRight());
        }
    }

    void SetTimer()
    {
        m_timeBeforeVibration.Start(Random.Range(m_minTime, m_maxTime));
    }

    IEnumerator LongVibrationControllerLeft()
    {
        m_controllerLeftIsVibrating = true;
        
        for (float i = 0; i < m_vibrationDuration; i += Time.deltaTime)
        {
            SteamVR_Controller.Input((int)m_leftController.index).TriggerHapticPulse(2000);
            yield return null;
        }

        SetTimer();
        m_controllerLeftIsVibrating = false;
    }

    IEnumerator LongVibrationControllerRight()
    {
        m_controllerRightIsVibrating = true;
        
        for (float i = 0; i < m_vibrationDuration; i += Time.deltaTime)
        {
            SteamVR_Controller.Input((int)m_rightController.index).TriggerHapticPulse(2000);
            yield return null;
        }

        SetTimer();
        m_controllerRightIsVibrating = false;
    }

    public void Setup(float duration, float minTime, float maxTime)
    {
        m_controllerLeftIsVibrating = false;
        m_controllerRightIsVibrating = false;
        m_maxTime = maxTime;
        m_minTime = minTime;
        m_vibrationDuration = duration;

        SetTimer();
    }
}
