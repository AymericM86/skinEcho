using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescaleRoomWithHand : GameSequence
{
    [SerializeField]
    private SteamVR_TrackedObject m_leftController;
    [SerializeField]
    private SteamVR_TrackedObject m_rightController;
    [SerializeField]
    private SteamVR_TrackedObject m_camera;

    [SerializeField]
    private GameObject m_room;

    private bool m_terminated = false;

    public override bool IsTerminated()
    {
        return m_terminated;
    }
    
	public override void DoInUpdate ()
    {
		if(SteamVR_Controller.Input((int)m_leftController.index).GetHairTrigger() && SteamVR_Controller.Input((int)m_rightController.index).GetHairTrigger())
        {
            Vector3 averageControllerPosition = (m_leftController.transform.position + m_rightController.transform.position) / 2;
            Vector3 headPosition = m_camera.transform.position;
            Debug.Log(averageControllerPosition);
            Debug.Log(headPosition);

            float radius = Vector2.Distance(new Vector2(headPosition.x,headPosition.z), new Vector2(averageControllerPosition.x, averageControllerPosition.z)) * 2;
            m_room.transform.position = new Vector3(headPosition.x, averageControllerPosition.y, headPosition.z);
            m_room.transform.localScale = new Vector3(radius, radius, radius);
            m_terminated = true;
        }
    }
}
