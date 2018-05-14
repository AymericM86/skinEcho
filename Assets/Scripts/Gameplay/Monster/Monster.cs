using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    CALM,
    CALM_TO_IRRITATED,
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

    bool m_soundCalmToIrritatedComplete = false;
    bool m_soundAngryComplete = false;

    [SerializeField]
    Submarine m_submarine;

    [Header("SteamVR")]
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    protected SteamVR_TrackedObject m_controllerRight;



    // Update is called once per frame
    void Update ()
    {
		switch(m_state)
        {
            case MonsterState.CALM:
                DoWhenCalm();
                break;
            case MonsterState.CALM_TO_IRRITATED:
                DoWhenCalmToIrritated();
                break;
            case MonsterState.IRRITATED:
                DoWhenIrritated();
                break;
            case MonsterState.ANGRY:
                DoWhenAngry();
                break;
            default:
                break;
        }
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

        if (m_calmTimer.IsTimedOut())
        {
            m_state = MonsterState.CALM_TO_IRRITATED;

            if (m_randomChange)
                SetCalmTime();

            //AkSoundEngine.PostEvent("stopMonsterCalm", gameObject);
            //AkSoundEngine.PostEvent("startMonsterCalmToIrritated", gameObject, (uint)AkCallbackType.AK_EndOfEvent, TerminateCalmToIrritated, null);
            m_soundCalmToIrritatedComplete = false;
        }
    }

    private void DoWhenIrritated()
    {
        m_irritatedTimer.UpdateTimer();

        if (m_irritatedTimer.IsTimedOut())
        {
            m_state = MonsterState.CALM;

            if (m_randomChange)
                SetIrritatedTime();

            //AkSoundEngine.PostEvent("stopMonsterIrritated", gameObject);
            //AkSoundEngine.PostEvent("startMonsterCalm", gameObject);
        }
        else
        {
            if((m_controllerLeft.CompareTag("Blowtorch") && SteamVR_Controller.Input((int)m_controllerLeft.index).GetHairTrigger())
                || (m_controllerRight.CompareTag("Blowtorch") && SteamVR_Controller.Input((int)m_controllerRight.index).GetHairTrigger()))
            {
                m_state = MonsterState.ANGRY;

                m_soundAngryComplete = false;

                if (m_randomChange)
                    SetIrritatedTime();

                //AkSoundEngine.PostEvent("stopMonsterIrritated", gameObject);
                //AkSoundEngine.PostEvent("startMonsterAngry", gameObject);
            }
        }
    }

    private void DoWhenCalmToIrritated()
    {
        if (m_soundCalmToIrritatedComplete)
        {
            m_state = MonsterState.IRRITATED;
            //AkSoundEngine.PostEvent("startMonsterIrritated", gameObject);
        }
    }

    private void DoWhenAngry()
    {
        if(m_soundAngryComplete)
        {
            m_state = MonsterState.CALM;

            m_submarine.OpenBreach();
            m_submarine.StartSway();
        }
    }

    void TerminateCalmToIrritated(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
            m_soundCalmToIrritatedComplete = true;
    }

    void TerminateAngry(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
            m_soundAngryComplete = true;
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

}
