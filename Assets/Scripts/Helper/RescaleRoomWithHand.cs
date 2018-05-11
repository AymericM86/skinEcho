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

            float radius = Vector2.Distance(new Vector2(headPosition.x,headPosition.z), new Vector2(averageControllerPosition.x, averageControllerPosition.z));
            if (radius < 0.55f)
                radius = 0.55f;
            radius *= 2;
            
            m_room.transform.position = new Vector3(headPosition.x, averageControllerPosition.y, headPosition.z);
            m_room.transform.localScale = new Vector3(radius, radius, radius);
            m_terminated = true;

            float angle = Vector3.SignedAngle(m_camera.transform.forward, m_room.transform.forward, Vector3.up);

            m_room.transform.Rotate(Vector3.up, -angle, Space.Self);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Vector3 direction = transform.forward;
        Color color = Color.green;
        float arrowHeadLength = 0.25f;
        float arrowHeadAngle = 20;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    public override void Rearm()
    {
        m_terminated = false;
    }
}
