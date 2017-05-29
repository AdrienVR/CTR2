
#define KART_DEBUG

using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class KartControl : MonoBehaviour {

    const float c_maxDt = 0.02f;
    //const float m_gravity = 10f;

    const float c_backwardSpeed = 0.35f;
    const float c_minVelocity = 0.1f;

    const int c_maxSupportedCollisions = 4;

    public Transform WheelFL;
    public Transform WheelFR;
    public Transform WheelBL;
    public Transform WheelBR;
    public Transform CenterPoint;
    public Camera m_cam;

    public Vector3 m_box;

    public float m_wheelOffsetWidth;
    public float m_wheelOffsetLength;
    public float m_wheelHeight;

    public float m_groundOffset = 0.25f;

    public float m_raycastMaxDist = 0.6f;

    public float m_gravity = -10;

    public float m_speed;
    public float m_repulsion;

    public float m_dampingForce;
    public float m_airFriction;
    public float m_airFrictionAcc;
    public float m_minAccelerationAirFriction = 1;
    public float m_airRotationControl;

    [Range(0, 1)]
    public float m_dotFriction;

    public AnimationCurve m_angularBySpeed;

    public bool m_touchGround;

    public LayerMask m_groundMask;
    public LayerMask m_wallMask;

    [HideInInspector][SerializeField]
    private TextMesh m_WheelFL;
    [HideInInspector][SerializeField]
    private TextMesh m_WheelFR;
    [HideInInspector][SerializeField]
    private TextMesh m_WheelBL;
    [HideInInspector][SerializeField]
    private TextMesh m_WheelBR;
    [HideInInspector][SerializeField]
    private TextMesh m_Center;

    public RaycastHit m_WheelFLInfo;
    public RaycastHit m_WheelFRInfo;
    public RaycastHit m_WheelBLInfo;
    public RaycastHit m_WheelBRInfo;

    public RaycastHit m_CenterInfo;

    public Vector3 m_forward;
    
    [Header("Acceleration")]
    public Vector3 m_acceleration;
    public Vector3 m_velocity;
    public float m_jump;

    public float m_maxVelocity;

    public float m_maxAcceleration;
    
    public float m_angularVelocity;

    public float m_maxAngularVelocity;

    public float m_angularAcceleration;

    public float m_maxAngularAcceleration;

    [Header("Sliding")]
    public bool m_sliding;
    public float m_slidingCoef;
    public float m_maxSliding;
    public float m_slidingAcc;
    public float m_slidingAngularCoef;
    public SlidingTrace m_trace;
    public float m_jumpAmplitude;

    [Range(0,1)]
    public float m_rotationFriction;

    [Range(0, 1)]
    public float m_friction;

    private float m_dt;

    private Vector3 m_position;
    private Vector3 m_slidingDir;
    private bool m_sliddingRight;

    private GamePadStateHolder gamePadState;

    private Transform m_transform;
    private int m_wheelOnGroundCount;
    //private int m_lastWheelOnGroundCount;

    private float m_currentMaxAngularVelocity;
    private float m_currentMaxAngularAcc;

    private Vector3[] m_lastWheelPos;

    private RaycastHit[] m_hits = new RaycastHit[c_maxSupportedCollisions];

    private List<RaycastHit> m_touchingWheelsInfos = new List<RaycastHit>(c_maxSupportedCollisions);

    private float m_curDotFriction;

#if KART_DEBUG
    [ContextMenu("OnValidate")]
    void OnValidate()
    {
        m_WheelFL = WheelFL.GetComponentInChildren<TextMesh>();
        m_WheelFR = WheelFR.GetComponentInChildren<TextMesh>();
        m_WheelBL = WheelBL.GetComponentInChildren<TextMesh>();
        m_WheelBR = WheelBR.GetComponentInChildren<TextMesh>();
        m_Center = CenterPoint.GetComponentInChildren<TextMesh>();
        m_trace = GetComponentInChildren<SlidingTrace>();
    }
#endif // KART_DEBUG

    void Start()
    {
        //m_rigidBody.con(new Vector3(0, m_gravity), ForceMode.Acceleration);
        m_transform = transform;
        gamePadState = ControllerManager.Instance.GamePadStates[0];

        m_lastWheelPos = new Vector3[4] { WheelFL.position, WheelFR.position, WheelBL.position, WheelBR.position };
    }

    // Update is called once per frame
    void Update ()
    {
        m_forward = m_transform.forward;

        Debug.DrawRay(m_transform.position + m_transform.forward * m_wheelOffsetLength, m_forward, Color.blue);

        m_dt = Time.deltaTime;
        if (m_dt > c_maxDt)
            m_dt = c_maxDt;
        m_position = m_transform.position;

        m_wheelOnGroundCount = 0;
        m_touchingWheelsInfos.Clear();

        m_WheelFLInfo = WheelRaycast(WheelFL, m_WheelFL, 0);
        m_WheelFRInfo = WheelRaycast(WheelFR, m_WheelFR, 1);
        m_WheelBLInfo = WheelRaycast(WheelBL, m_WheelBL, 2);
        m_WheelBRInfo = WheelRaycast(WheelBR, m_WheelBR, 3);
        m_CenterInfo = WheelRaycast(CenterPoint, m_Center, -1, false);

        UpdateGroundPhysic();
        UpdateVelocity();
        UpdateAngularVelocity();

        if (m_touchGround)
            UpdateGroundControl();
        else
            UpdateAirControl();

        UpdateGravity();

        UpdateCollision();
        // after collision velocity and angularVelocity can be applied safely

        m_position += m_velocity * m_dt;

        m_transform.rotation *= Quaternion.Euler(0, m_angularVelocity * m_dt, 0);
        m_transform.position = m_position;

        m_WheelFLInfo = WheelRaycast(WheelFL, m_WheelFL, 0);
        m_WheelFRInfo = WheelRaycast(WheelFR, m_WheelFR, 1);
        m_WheelBLInfo = WheelRaycast(WheelBL, m_WheelBL, 2);
        m_WheelBRInfo = WheelRaycast(WheelBR, m_WheelBR, 3);
        UpdateGravity();

        //m_lastWheelOnGroundCount = m_wheelOnGroundCount;
    }

    public TextMesh[] m_specialDebug;

    private void ApplyCollision(RaycastHit rHit, Vector3 nDir, int i)
    {
        Debug.DrawRay(rHit.point, rHit.normal, Color.blue);
        Debug.DrawRay(rHit.point, nDir, Color.red);
        float dot = Vector3.Dot(nDir, rHit.normal);
        if (m_specialDebug.Length > 0)
            m_specialDebug[i].text = dot.ToString();

        if (dot < -0.5f)
        {
            m_velocity = Vector3.zero;
            m_acceleration = Vector3.zero;
        }
        else
        {
            m_velocity = rHit.normal * m_speed * m_repulsion;
        }
    }

    private void UpdateCollision()
    {
        float speed = m_velocity.magnitude;
        Vector3 nDir = m_velocity.normalized;

        Quaternion newRot = m_transform.rotation * Quaternion.Euler(0, m_angularVelocity * m_dt, 0);


        if (m_angularVelocity != 0)
        {
            RaycastHit rayHit;
            if (Physics.BoxCast(m_position - (newRot * Vector3.forward), m_box, (newRot * Vector3.forward), out rayHit, newRot, 1, m_wallMask))
            {
                m_angularVelocity = 0;
                newRot = m_transform.rotation;
            }
            else if (Physics.BoxCast(m_position + (newRot * Vector3.forward), m_box, (newRot * -Vector3.forward), out rayHit, newRot, 1, m_wallMask))
            {
                m_angularVelocity = 0;
                newRot = m_transform.rotation;
            }
        }

        if (speed > 0)
        {
            int hits = Physics.BoxCastNonAlloc(m_position, m_box, nDir, m_hits, newRot, speed * m_dt, m_wallMask);
            for (int i = 0; i < hits; i++)
            {
                ApplyCollision(m_hits[i], nDir, i);
            }
        }
    }

    private void UpdateGravity()
    {
        // apply gravity
        if (m_CenterInfo.distance == 2)
        {
            m_acceleration.y = m_gravity;
            m_touchGround = false;
        }
        else
        {
            m_touchGround = true;
            if (m_velocity.y < 0)
                m_velocity.y = 0;
            else if (m_velocity.y > -m_gravity * m_dt)
                m_velocity.y += m_gravity * m_dt;

            if (m_CenterInfo.distance != 2 && (m_wheelOnGroundCount == 4 || m_wheelOnGroundCount < 3))
                m_position.y = m_CenterInfo.point.y;
            else
            {
                m_position.y = 0;
                float count = m_wheelOnGroundCount;
                for (int i = 0; i< count; i++)
                {
                    m_position.y += m_touchingWheelsInfos[i].point.y / count;
                }
            }
            m_position.y += m_groundOffset;
        }


        Vector3 lhs;
        Vector3 rhs;
        
        Vector3 normal = Vector3.zero;

        if (m_wheelOnGroundCount == 4)
        {
            lhs = m_WheelFLInfo.point - m_WheelBLInfo.point;
            rhs = m_WheelBRInfo.point - m_WheelBLInfo.point;
            normal = Vector3.Cross(lhs, rhs);
        }
        else if (m_wheelOnGroundCount == 3)
        {
            lhs = m_touchingWheelsInfos[1].point - m_touchingWheelsInfos[0].point;
            rhs = m_touchingWheelsInfos[2].point - m_touchingWheelsInfos[0].point;
            normal = Vector3.Cross(lhs, rhs);
        }

        if (normal.y <= 0 && m_CenterInfo.distance != 2)
        {
            normal = m_CenterInfo.normal;
        }
        else
        {
            normal = Vector3.up;
        }

        Debug.DrawRay(m_transform.position, m_transform.forward, Color.blue);
        Vector3 dir = Vector3.ProjectOnPlane(m_transform.forward, normal);
        Debug.DrawRay(m_transform.position, dir, Color.red);
        Debug.DrawRay(m_transform.position, normal, Color.yellow);
        m_transform.LookAt(m_transform.position + dir, normal);
    }

    private void UpdateVelocity()
    {
        m_velocity += m_acceleration * m_dt;
        m_velocity.y += m_jump * m_dt;
        m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxVelocity);

        if (m_curDotFriction > 0 && m_speed > 0)
        {
            Vector3 forwardVelocity = m_forward * Vector3.Dot(m_forward, m_velocity);
            m_velocity = Vector3.Lerp(m_velocity, forwardVelocity, m_curDotFriction);
        }
    }

    private void UpdateAngularVelocity()
    {
        m_angularVelocity += m_angularAcceleration * m_dt;
        if (m_angularVelocity > 0)
        {
            m_angularVelocity -= m_rotationFriction * m_currentMaxAngularAcc * m_dt;
            if (m_angularVelocity < 0)
                m_angularVelocity = 0;
        }
        else if (m_angularVelocity < 0)
        {
            m_angularVelocity += m_rotationFriction * m_currentMaxAngularAcc * m_dt;
            if (m_angularVelocity > 0)
                m_angularVelocity = 0;
        }

        if (m_angularVelocity > m_currentMaxAngularVelocity)
            m_angularVelocity = m_currentMaxAngularVelocity;
        if (m_angularVelocity < -m_currentMaxAngularVelocity)
            m_angularVelocity = -m_currentMaxAngularVelocity;
    }
    
    private void UpdateGroundPhysic()
    {
        // les roues arrières ne vont pas vers l'avant = recul
        //if (m_WheelBLInfo.normal)

        // 3 roues en l'air = force vers le vide

        // direction à 90° sans dérapage = stop
    }

    private void UpdateAirControl()
    {
        UpdateSliding();

        UpdateAirFriction();
    }

    Quaternion m_airRotation;

    private void UpdateAirFriction()
    {
        Vector3 velocityDir = m_velocity.normalized;
        if (Vector3.Dot(velocityDir, m_acceleration) > 0 && m_acceleration.magnitude > m_minAccelerationAirFriction)
        {
            if (m_acceleration.y > 0)
                m_acceleration.y = 0;
            m_acceleration -= velocityDir * m_airFrictionAcc * Time.deltaTime;
            if (Vector3.Dot(velocityDir, m_acceleration) < 0)
                m_acceleration = Vector3.zero;
        }

        UpdateFriction(m_airFriction);
    }

    private bool m_slidingInput;

    private void UpdateSliding()
    {
        bool lastInput = m_slidingInput;
        m_slidingInput = gamePadState.State.Buttons.RightShoulder == ButtonState.Pressed || gamePadState.State.Buttons.LeftShoulder == ButtonState.Pressed;
        
        m_slidingCoef = m_angularBySpeed.Evaluate(m_speed);
        if (m_sliding)
            m_slidingCoef *= m_slidingAngularCoef;

        float slideInput = m_sliddingRight ? gamePadState.State.ThumbSticks.Left.X : -gamePadState.State.ThumbSticks.Left.X;
        float clampedSlideInput = Mathf.Clamp01(slideInput);

        m_maxSliding = Mathf.Max(clampedSlideInput, m_maxSliding);

        m_slidingCoef *= m_maxSliding;

        if (m_slidingInput && m_touchGround && !lastInput)
        {
            m_jump = m_jumpAmplitude;
        }
        else
            m_jump = 0;

        if (m_slidingInput && m_touchGround && m_speed > 5)
        {
            m_curDotFriction = 0;
            if (!m_sliding)
            {
                m_sliding = true;
                if (gamePadState.State.ThumbSticks.Left.X > 0)
                    m_sliddingRight = true;
                else
                    m_sliddingRight = false;

                m_trace.enabled = true;
            }
            m_slidingDir = m_sliddingRight ? m_transform.right : m_transform.rotation * -Vector3.right;
            m_acceleration += m_slidingAcc * m_slidingDir * m_slidingCoef * m_speed * Mathf.Abs(m_angularVelocity) * m_dt;

            float sliding = (m_maxSliding + slideInput);

            m_currentMaxAngularVelocity = sliding * m_maxAngularVelocity;
            m_angularAcceleration = sliding * (m_sliddingRight ? m_currentMaxAngularAcc : -m_currentMaxAngularAcc);
        }
        else
        {
            m_curDotFriction = m_dotFriction;

            m_angularAcceleration = gamePadState.State.ThumbSticks.Left.X * m_currentMaxAngularAcc;

            if (m_sliding)
            {
                m_sliding = false;
                m_trace.RemoveAll();
                m_trace.enabled = false;
                m_maxSliding = 0;
            }
        }

        if (m_sliding)
        {
            //m_currentMaxAngularVelocity = m_maxAngularVelocity * m_slidingCoef;
            //m_currentMaxAngularAcc = m_maxAngularAcceleration * m_slidingCoef;
        }
        else
        {
            m_currentMaxAngularVelocity = m_maxAngularVelocity;
            m_currentMaxAngularAcc = m_maxAngularAcceleration;
        }
    }

    private void UpdateGroundControl()
    {
        if (gamePadState.State.Buttons.A == XInputDotNetPure.ButtonState.Pressed)
        {
            m_acceleration = m_forward * m_maxAcceleration;
        }
        else if (gamePadState.State.ThumbSticks.Left.Y < 0)
        {
            m_acceleration = -m_forward * m_maxAcceleration * c_backwardSpeed;
        }
        else
        {
            m_acceleration = Vector3.zero;
        }

        UpdateSliding();

        UpdateGroundFriction();
    }


    private void UpdateGroundFriction()
    {
        float coef = m_friction;
        if (Vector3.Dot(m_forward, m_velocity) < 0)
            coef *= c_backwardSpeed;
        UpdateFriction(coef);
    }

    void UpdateFriction(float _coef)
    {
        Vector3 xzVelocity = m_velocity;
        xzVelocity.y = 0;
        m_speed = xzVelocity.magnitude;

        if (m_speed < c_minVelocity)
        {
            m_velocity.x = 0;
            m_velocity.z = 0;
        }
        else
        {
            m_velocity -= xzVelocity * _coef * m_maxAcceleration * m_dt;
        }
    }

    RaycastHit WheelRaycast(Transform _tr, TextMesh _text, int _lastPosIdx, bool _wheel = true)
    {
        RaycastHit hit;
        Vector3 dir;
        Vector3 origin;
        if (_wheel)
        {
            dir = (_tr.position - m_lastWheelPos[_lastPosIdx] - _tr.up);
            origin = m_lastWheelPos[_lastPosIdx] + m_groundOffset * _tr.up;

            m_lastWheelPos[_lastPosIdx] = _tr.position;
        }
        else
        {
            origin = _tr.position;
            dir = -Vector3.up;
        }

        bool hitBool = Physics.Raycast(origin, dir, out hit, m_raycastMaxDist, (int)m_groundMask);
        //if (!_wheel)
        Debug.DrawRay(origin, dir * m_raycastMaxDist, Color.red);
        if (hitBool)
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        //Debug.DrawRay(_wheel.position, -Vector3.up * m_raycastMaxDist, hitBool ? Color.green : Color.red);
        float dist = hit.distance / m_raycastMaxDist;
        if (!hitBool)
            dist = 2;
        else if (_wheel)
            m_wheelOnGroundCount++;
        _text.text = (dist).ToString("n2");
        hit.distance = dist;

        if (_wheel && hitBool)
            m_touchingWheelsInfos.Add(hit);
        //m_rigidBody.AddForceAtPosition(Vector3.up * (1 - dist) * m_dampingForce * Time.deltaTime, _wheel.position, ForceMode.VelocityChange);
        return hit;
    }

#if UNITY_EDITOR
    [ContextMenu("SetOffsets")]
    void SetOffsets()
    {
        UnityEditor.Undo.RecordObjects(new Transform[]{WheelFL, WheelFR, WheelBL, WheelBR, CenterPoint}, "SetOffsets");
        WheelFL.localPosition = new Vector3(-m_wheelOffsetWidth, m_wheelHeight, m_wheelOffsetLength);
        WheelFR.localPosition = new Vector3(m_wheelOffsetWidth, m_wheelHeight, m_wheelOffsetLength);
        WheelBL.localPosition = new Vector3(-m_wheelOffsetWidth, m_wheelHeight, -m_wheelOffsetLength);
        WheelBR.localPosition = new Vector3(m_wheelOffsetWidth, m_wheelHeight, -m_wheelOffsetLength);
        CenterPoint.localPosition = new Vector3(0, m_wheelHeight, 0);
    }
#endif
}
