using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class KartTransformer
{
    [HideInInspector]
    public float YAngle;
    [HideInInspector]
    public Rigidbody KartRigidbody;
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

    public void Start()
    {
        m_layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    public void Update()
    {
        UpdateZAngle();
        UpdateXAngle();
        UpdatePosition();

        KartRigidbody.rotation = Quaternion.Euler(new Vector3(m_xAngle,
            YAngle,
            m_zAngle));

    }

    private void UpdatePosition()
    {
        bool isInAir = true;
        RaycastHit hitBottomLeft;
        if (Physics.Raycast(BottomLeftWheel.position + Vector3.up * 3, -Vector3.up, out hitBottomLeft, DistanceFromGroundCast, m_layerMask))
        {
            isInAir = false;
            RaycastHit hitBottomRight;
            if (Physics.Raycast(BottomRightWheel.position + Vector3.up * 3, -Vector3.up, out hitBottomRight, DistanceFromGroundCast, m_layerMask))
            {
                RaycastHit hitFrontLeft;
                if (Physics.Raycast(FrontLeftWheel.position + Vector3.up * 3, -Vector3.up, out hitFrontLeft, DistanceFromGroundCast, m_layerMask))
                {
                    RaycastHit hitFrontRight;
                    if (Physics.Raycast(FrontRightWheel.position+Vector3.up *3, -Vector3.up, out hitFrontRight, DistanceFromGroundCast, m_layerMask))
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
            if (Physics.Raycast(FrontRightWheel.position + Vector3.up * 3, -Vector3.up, out hitFrontRight, DistanceFromGroundCast, m_layerMask))
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
        Debug.DrawRay(right, -Vector3.up * DistanceFromGroundCast, Color.blue);
        if (Physics.Raycast(right + Vector3.up * 3, -Vector3.up, out hitRight, DistanceFromGroundCast, m_layerMask))
        {
            Vector3 left = (BottomLeftWheel.position + FrontLeftWheel.position) * 0.5f;
            Debug.DrawRay(left + Vector3.up * 3, -Vector3.up * DistanceFromGroundCast, Color.red);
            RaycastHit hitLeft;
            if (Physics.Raycast(left + Vector3.up * 3, -Vector3.up, out hitLeft, DistanceFromGroundCast, m_layerMask))
            {
                float y = hitRight.point.y - hitLeft.point.y;
                m_zAngle = YtoAngleZ.Evaluate(y);
            }
        }
    }

    private void UpdateXAngle()
    {
        Vector3 bottom = (BottomRightWheel.position + BottomLeftWheel.position) * 0.5f;
        RaycastHit hitBottom;
        if (Physics.Raycast(bottom + Vector3.up * 3, -Vector3.up, out hitBottom, DistanceFromGroundCast, m_layerMask))
        {
            Vector3 front = (FrontRightWheel.position + FrontLeftWheel.position) * 0.5f;
            RaycastHit hitFront;
            Debug.DrawRay(front + Vector3.up * 3, -Vector3.up * DistanceFromGroundCast, Color.red);
            if (Physics.Raycast(front + Vector3.up * 3, -Vector3.up, out hitFront, DistanceFromGroundCast, m_layerMask))
            {
                float y = hitBottom.point.y - hitFront.point.y;
                m_xAngle = YToAngleX.Evaluate(y);
            }
        }
    }

    private float m_xAngle;
    private float m_zAngle;

    private int m_layerMask;

}