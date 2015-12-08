using UnityEngine;
using System;

[Serializable]
public class KartTransformer
{
    [HideInInspector]
    public float YAngle;
    [HideInInspector]
    [NonSerialized]
    public KartRigidBody KartRigidbody;
    [HideInInspector]
    public Animator KartAnimator;

    public Transform WholeKart;
    public Transform FrontLeftParent;
    public Transform FrontLeftWheel;
    public Transform FrontRightParent;
    public Transform FrontRightWheel;
    public Transform BottomLeftParent;
    public Transform BottomLeftWheel;
    public Transform BottomRightParent;
    public Transform BottomRightWheel;

    public AnimationCurve YtoAngleZ;
    public AnimationCurve YToAngleX;

    public const float DistanceFromGroundCast = 6;
    public float GravityFactor = 9.81f;

	public bool isInAir;

    public void Start()
    {
        m_groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        m_wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        m_back = (BottomRightWheel.position + BottomLeftWheel.position) * 0.5f;
        m_front = (FrontRightWheel.position + FrontLeftWheel.position) * 0.5f;
    }

    public void Update()
    {
        UpdateZAngle();
        UpdateXAngle();
        UpdatePosition();

        KartRigidbody.rotationEuler = new Vector3(m_xAngle, YAngle, m_zAngle);
    }

    private void UpdatePosition()
    {
        isInAir = true;
        RaycastHit hitBottomLeft;
        if (Physics.Raycast(BottomLeftWheel.position + Vector3.up * 3, -Vector3.up, out hitBottomLeft, DistanceFromGroundCast, m_groundLayerMask))
        {
            isInAir = false;
            RaycastHit hitBottomRight;
            if (Physics.Raycast(BottomRightWheel.position + Vector3.up * 3, -Vector3.up, out hitBottomRight, DistanceFromGroundCast, m_groundLayerMask))
            {
                RaycastHit hitFrontLeft;
                if (Physics.Raycast(FrontLeftWheel.position + Vector3.up * 3, -Vector3.up, out hitFrontLeft, DistanceFromGroundCast, m_groundLayerMask))
                {
                    RaycastHit hitFrontRight;
                    if (Physics.Raycast(FrontRightWheel.position+Vector3.up *3, -Vector3.up, out hitFrontRight, DistanceFromGroundCast, m_groundLayerMask))
                    {
                        Vector3 newPosition = KartRigidbody.position;
                        newPosition.y = 0.25f * (hitBottomLeft.point.y + hitBottomRight.point.y + hitFrontLeft.point.y + hitFrontRight.point.y);
                        KartRigidbody.position = newPosition;
                    }
                }
            }
        }
        else
        {
            RaycastHit hitFrontRight;
            if (Physics.Raycast(FrontRightWheel.position + Vector3.up * 3, -Vector3.up, out hitFrontRight, DistanceFromGroundCast, m_groundLayerMask))
            {
                isInAir = false;
            }
        }
        if(isInAir)
        {
            Vector3 newPosition = KartRigidbody.position;
            newPosition.y -= GravityFactor * Time.deltaTime;
            KartRigidbody.position = newPosition;
        }
    }

    private void UpdateZAngle()
    {
        Vector3 right = (BottomRightWheel.position + FrontRightWheel.position) * 0.5f;
        RaycastHit hitRight;
        //Debug.DrawRay(right, -Vector3.up * DistanceFromGroundCast, Color.blue);
        if (Physics.Raycast(right + Vector3.up * 3, -Vector3.up, out hitRight, DistanceFromGroundCast, m_groundLayerMask))
        {
            Vector3 left = (BottomLeftWheel.position + FrontLeftWheel.position) * 0.5f;
            //Debug.DrawRay(left + Vector3.up * 3, -Vector3.up * DistanceFromGroundCast, Color.red);
            RaycastHit hitLeft;
            if (Physics.Raycast(left + Vector3.up * 3, -Vector3.up, out hitLeft, DistanceFromGroundCast, m_groundLayerMask))
            {
                float y = hitRight.point.y - hitLeft.point.y;
                m_zAngle = YtoAngleZ.Evaluate(y);
            }
        }
    }

    private void UpdateXAngle()
    {
        RaycastHit hitBack;
        if (Physics.Raycast(m_back + Vector3.up * 3, -Vector3.up, out hitBack, DistanceFromGroundCast, m_groundLayerMask))
        {
            RaycastHit hitFront;
            //Debug.DrawRay(m_front + Vector3.up * 3, -Vector3.up * DistanceFromGroundCast, Color.red);
            if (Physics.Raycast(m_front + Vector3.up * 3, -Vector3.up, out hitFront, DistanceFromGroundCast, m_groundLayerMask))
            {
                float y = hitBack.point.y - hitFront.point.y;
                m_xAngle = YToAngleX.Evaluate(y);
            }
        }
    }

    private void CheckCollision()
    {
        RaycastHit hitBack;
        if (Physics.Raycast(m_back, WholeKart.forward, out hitBack, 1, m_wallLayerMask))
        {
        }
        RaycastHit hitFront;
        if (Physics.Raycast(m_front + Vector3.up * 3, -Vector3.up, out hitFront, DistanceFromGroundCast, m_groundLayerMask))
        {
            float y = hitBack.point.y - hitFront.point.y;
            m_xAngle = YToAngleX.Evaluate(y);
        }
    }

    private Vector3 m_back;
    private Vector3 m_front;

    private float m_xAngle;
    private float m_zAngle;

    private int m_groundLayerMask;
    private int m_wallLayerMask;

}