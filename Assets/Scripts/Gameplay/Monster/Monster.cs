using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    CALM,
    IRRITATED,
    ANGRY
}

public class Monster : MonoBehaviour
{
    [Header("Monster states")]
    [SerializeField]
    MonsterState m_state = MonsterState.CALM;

    [SerializeField]
    bool m_randomChange = true;

    Timer m_calmTimer = new Timer();
    Timer m_irritatedTimer = new Timer();

    [SerializeField]
    float m_minTimeIrritated = 5.0f;
    [SerializeField]
    float m_maxTimeIrritated = 10.0f;

    [SerializeField]
    float m_minTimeCalm = 10.0f;
    [SerializeField]
    float m_maxTimeCalm = 15.0f;
    [SerializeField]
    Timer m_timeInvinsible;
    
    bool m_soundAngryComplete = false;

    [SerializeField]
    Submarine m_submarine;

    [Header("SteamVR")]
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerRight;

    [Header("Monster rotation")]
    [SerializeField]
    float m_calmDistance = 10;
    [SerializeField]
    float m_irritatedDistance = 5;
    [SerializeField]
    float m_degresPerSecondCalm = 30;
    [SerializeField]
    float m_degresPerSecondIrritated = 40;

    float m_anglePosition = 0;
    Timer m_timeToStayAway;

    [SerializeField]
    float m_oxygeneToRemoveWhenAttack = 20;
    OxygeneGauge m_oxygene;

    bool m_controllerLeftIsVibrating = false;
    bool m_controllerRightIsVibrating = false;

    [SerializeField]
    float m_vibrationDuration = 3;
    [SerializeField]
    ushort m_vibrationForce = 3000;

    private void Start()
    {
        AkSoundEngine.PostEvent("Play_SFX_Monster_Calm", gameObject);
        SetCalmTime();
        SetIrritatedTime();

        m_timeToStayAway = new Timer();
        m_oxygene = GameObject.FindGameObjectWithTag("Oxygene").GetComponent<OxygeneGauge>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Distance from the submarine
        float distance = 0;

		switch(m_state)
        {
            case MonsterState.CALM:
                DoWhenCalm();
                m_anglePosition += m_degresPerSecondCalm * Time.deltaTime;
                distance = Mathf.Lerp(m_irritatedDistance, m_calmDistance, m_timeToStayAway.GetRatio());

                break;
            case MonsterState.IRRITATED:
                DoWhenIrritated();
                m_anglePosition += m_degresPerSecondIrritated * Time.deltaTime;
                distance = Mathf.Lerp(m_calmDistance, m_irritatedDistance, m_timeInvinsible.GetRatio());
                break;
            case MonsterState.ANGRY:
                DoWhenAngry();
                break;
            default:
                break;
        }

        if (m_anglePosition > 360)
            m_anglePosition -= 360;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = m_submarine.transform.position + new Vector3(0, 0, distance);
        transform.RotateAround(m_submarine.transform.position, Vector3.up, m_anglePosition);
	}

    private void SetCalmTime()
    {
        SetCalmTime(Random.Range(m_minTimeCalm, m_maxTimeCalm));
    }

    private void SetIrritatedTime()
    {
        SetIrritatedTime(Random.Range(m_minTimeIrritated, m_maxTimeIrritated));
    }

    public void SetCalmTime(float time)
    {
        m_calmTimer.Start(time);
    }

    public void SetIrritatedTime(float time)
    {
        m_irritatedTimer.Start(time);
    }

    public void SetRandom(bool random)
    {
        m_randomChange = random;
    }

    private void DoWhenCalm()
    {
        m_calmTimer.UpdateTimer();
        m_timeToStayAway.UpdateTimer();

        if (m_calmTimer.IsTimedOut())
        {
            m_state = MonsterState.IRRITATED;

            m_timeInvinsible.Restart();

            if (m_randomChange)
                SetCalmTime();

            AkSoundEngine.PostEvent("Stop_SFX_Monster_Calm", gameObject);
            AkSoundEngine.PostEvent("Play_SFX_Monster_Aggressive", gameObject);
            AkSoundEngine.PostEvent(AK.EVENTS.PLAY_SFX_RADAR, gameObject);
        }
    }

    private void DoWhenIrritated()
    {
        m_irritatedTimer.UpdateTimer();

        if (m_irritatedTimer.IsTimedOut())
        {
            m_timeToStayAway.Start(m_timeInvinsible.GetTimeToReach());
            m_state = MonsterState.CALM;

            if (m_randomChange)
                SetIrritatedTime();

            AkSoundEngine.PostEvent(AK.EVENTS.STOP_SFX_RADAR, gameObject);
            AkSoundEngine.PostEvent("Stop_SFX_Monster_Aggressive", gameObject);
            AkSoundEngine.PostEvent("Play_SFX_Monster_Calm", gameObject);
        }
        else
        {
            m_timeInvinsible.UpdateTimer();
            if (!m_timeInvinsible.IsTimedOut())
                return;

            if((m_controllerLeft.CompareTag("Blowtorch") && SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
                || (m_controllerRight.CompareTag("Blowtorch") && SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger()))
            {
                m_state = MonsterState.ANGRY;

                m_soundAngryComplete = false;

                if (m_randomChange)
                    SetIrritatedTime();
                
                StartCoroutine(LongVibrationControllerLeft());
                StartCoroutine(LongVibrationControllerRight());
                AkSoundEngine.PostEvent(AK.EVENTS.STOP_SFX_RADAR, gameObject);
                AkSoundEngine.PostEvent("Stop_SFX_Monster_Aggressive", gameObject);
                AkSoundEngine.PostEvent("Play_SFX_Monster_Attack", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateAngry, null);
            }
        }
    }

    private void DoWhenAngry()
    {
        if(m_soundAngryComplete)
        {
            m_state = MonsterState.CALM;

            m_oxygene.RemoveOxygene(m_oxygeneToRemoveWhenAttack);
            m_submarine.OpenBreach();
            m_submarine.StartSway();
        }
    }

    void TerminateAngry(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            m_soundAngryComplete = true;
            AkSoundEngine.PostEvent("Play_SFX_Monster_Calm", gameObject);
        }
    }

    public void SetupRandom(float calmMin, float calmMax, float irritatedMin, float irritatedMax)
    {
        m_randomChange = true;
        m_minTimeCalm = calmMin;
        m_maxTimeCalm = calmMax;
        m_minTimeIrritated = irritatedMin;
        m_maxTimeIrritated = irritatedMax;

        SetCalmTime();
        SetIrritatedTime();
        m_state = MonsterState.CALM;
    }

    public void Setup(float calmTime, float irritatedTime)
    {
        m_randomChange = false;

        SetCalmTime(calmTime);
        SetIrritatedTime(irritatedTime);
        m_state = MonsterState.CALM;
    }

    IEnumerator LongVibrationControllerLeft()
    {
        m_controllerLeftIsVibrating = true;

        for (float i = 0; i < m_vibrationDuration; i += Time.deltaTime)
        {
            SteamVR_Controller.Input((int)m_controllerLeft.index).TriggerHapticPulse(m_vibrationForce);
            yield return null;
        }
        m_controllerLeftIsVibrating = false;
    }

    IEnumerator LongVibrationControllerRight()
    {
        m_controllerRightIsVibrating = true;

        for (float i = 0; i < m_vibrationDuration; i += Time.deltaTime)
        {
            SteamVR_Controller.Input((int)m_controllerRight.index).TriggerHapticPulse(m_vibrationForce);
            yield return null;
        }
        m_controllerRightIsVibrating = false;
    }

}
