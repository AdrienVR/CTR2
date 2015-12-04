
using System;
using UnityEngine;

[Serializable]
public class KartRigidBody
{
    public float MinSpeedToHurt;
    public PlayerController PlayerController;

    [HideInInspector]
    [NonSerialized]
    public KartTransformer KartTransformer;

    [HideInInspector]
    public Transform transform;
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 rotationEuler;

    public const float RayCastLength = 0.5f;

    public static int WallLayer = 1 << LayerMask.NameToLayer("Wall");

    // Update is called once per frame
    public void Update ()
    {
        Vector3 direction = position - transform.position;

        if (direction.Equals(Vector3.zero))
        {
            return;
        }

        RaycastHit hitDirection;
        if (Vector3.Dot(direction, transform.forward) > 0)
        {
            Vector3 forwardDirection = transform.forward * RayCastLength;
            Debug.DrawRay(KartTransformer.FrontLeftWheel.position, forwardDirection);
            Debug.DrawRay(KartTransformer.FrontRightWheel.position, forwardDirection);
            if (Physics.Raycast(KartTransformer.FrontLeftWheel.position, forwardDirection, out hitDirection, RayCastLength, WallLayer))
            {
                Debug.DrawRay(KartTransformer.FrontLeftWheel.position + Vector3.up, forwardDirection, Color.red);
            }
            else if (Physics.Raycast(KartTransformer.FrontRightWheel.position, forwardDirection, out hitDirection, RayCastLength, WallLayer))
            {
                Debug.DrawRay(KartTransformer.FrontRightWheel.position + Vector3.up, forwardDirection, Color.red);
            }
            else
            {
                transform.position = position;
                transform.rotation = Quaternion.Euler(rotationEuler);
                m_lastRotation = rotationEuler;
            }
        }
        else
        {
            Vector3 forwardDirection = transform.forward * -RayCastLength;
            Debug.DrawRay(KartTransformer.BottomLeftWheel.position, forwardDirection);
            Debug.DrawRay(KartTransformer.BottomRightWheel.position, forwardDirection);
            if (Physics.Raycast(KartTransformer.BottomLeftWheel.position, forwardDirection, out hitDirection, RayCastLength, WallLayer))
            {
                Debug.DrawRay(KartTransformer.BottomLeftWheel.position + Vector3.up, forwardDirection, Color.red);
            }
            else if (Physics.Raycast(KartTransformer.BottomRightWheel.position, forwardDirection, out hitDirection, RayCastLength, WallLayer))
            {
                Debug.DrawRay(KartTransformer.BottomRightWheel.position + Vector3.up, forwardDirection, Color.red);
            }
            else
            {
                transform.position = position;
                transform.rotation = Quaternion.Euler(rotationEuler);
                m_lastRotation = rotationEuler;
            }
        }

        position = transform.position;
        rotationEuler = m_lastRotation;
    }
    
    private void CheckHurt()
    {

    }

    private Vector3 m_lastRotation;

}
