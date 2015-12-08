
using System;
using UnityEngine;

[Serializable]
public class KartRigidBody
{
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
    public const float SlippageAngle = -0.9f;

    public float CollisionMinSpeed = 0.1f;

    public static int WallLayer = 1 << LayerMask.NameToLayer("Wall");

    public void Initialize()
    {
        m_lastPosition = transform.position;
    }

    // Update is called once per frame
    public void Update ()
    {
        Vector3 direction = position - transform.position;

        if (direction.Equals(Vector3.zero))
        {
            return;
        }

        Vector3 distance = transform.position - m_lastPosition;
        m_speed = distance.magnitude / Time.deltaTime;
        m_lastPosition = transform.position;

        Vector3 horizontalDirection = direction;
        horizontalDirection.y = 0;


        RaycastHit hitDirection;
        if (Vector3.Dot(horizontalDirection, transform.forward) > 0)
        {
            Vector3 rayDirection = horizontalDirection * RayCastLength;
            if (Physics.Raycast(KartTransformer.FrontLeftWheel.position, rayDirection, out hitDirection, RayCastLength, WallLayer))
            {
                CheckForwardSlippage(horizontalDirection, hitDirection);
            }
            else if (Physics.Raycast(KartTransformer.FrontRightWheel.position, rayDirection, out hitDirection, RayCastLength, WallLayer))
            {
                CheckForwardSlippage(horizontalDirection, hitDirection);
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
            if (Physics.Raycast(KartTransformer.BottomLeftWheel.position, forwardDirection, out hitDirection, RayCastLength, WallLayer))
            {
                CheckBackwardSlippage(horizontalDirection, hitDirection);
            }
            else if (Physics.Raycast(KartTransformer.BottomRightWheel.position, forwardDirection, out hitDirection, RayCastLength, WallLayer))
            {
                CheckBackwardSlippage(horizontalDirection, hitDirection);
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
    
    private void CheckForwardSlippage(Vector3 horizontalDirection, RaycastHit hitDirection)
    {
        float dot = Vector3.Dot(horizontalDirection.normalized, hitDirection.normal.normalized);

        if (dot > SlippageAngle)
        {
            dot = Vector3.Dot(horizontalDirection, hitDirection.normal.normalized);
            Vector3 inWallDirection = horizontalDirection - hitDirection.normal.normalized * (dot);
            position = transform.position + inWallDirection;

            transform.position = position;
            transform.rotation = Quaternion.Euler(rotationEuler);
            m_lastRotation = rotationEuler;
        }
        else if (m_speed > CollisionMinSpeed)
        {
            ApplyCollision();
        }
    }

    private void CheckBackwardSlippage(Vector3 horizontalDirection, RaycastHit hitDirection)
    {
        float dot = Vector3.Dot(horizontalDirection.normalized, hitDirection.normal.normalized);

        if (dot < -SlippageAngle)
        {
            dot = Vector3.Dot(horizontalDirection, hitDirection.normal.normalized);
            Vector3 inWallDirection = horizontalDirection - hitDirection.normal.normalized * (dot);
            position = transform.position + inWallDirection;

            transform.position = position;
            transform.rotation = Quaternion.Euler(rotationEuler);
            m_lastRotation = rotationEuler;
        }
    }

    private void ApplyCollision()
    {
        PlayerController.CollisionStop();
    }

    private float m_speed;

    private Vector3 m_lastPosition;
    private Vector3 m_lastRotation;

}
