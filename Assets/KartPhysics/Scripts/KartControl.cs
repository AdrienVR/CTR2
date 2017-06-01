
#define KART_DEBUG

using System.Collections;
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

    public float m_wheelRaycastDist = 0.6f;
    public float m_centerRaycastDist = 2.5f;

    public float m_gravity = -10;
    private Vector3 m_gravityVector;

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

    private Vector3 m_groundPos;
    private Vector3 m_groundDir;
    private float m_groundDist;
    private Vector3 m_groundNormal;
    private float m_lastGroundDist;
    private float m_maxGroundDist;

    public Vector3 m_forward;
    
    [Header("Acceleration")]
    public Vector3 m_acceleration;
    public Vector3 m_gravityResult;
    public Vector3 m_velocity;
    public Vector3 m_addVelocity;
    private Vector3 m_XZvelocity;
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
    //private Vector3 m_displacement;
    private Vector3 m_slidingDir;
    private bool m_sliddingRight;

    private GamePadStateHolder gamePadState;

    private Transform m_transform;
    private int m_wheelOnGroundCount;

    private float m_currentMaxAngularVelocity;
    private float m_currentMaxAngularAcc;

    private Vector3[] m_lastWheelPos;

    private RaycastHit[] m_hits = new RaycastHit[c_maxSupportedCollisions];

    private List<RaycastHit> m_touchingWheelsInfos = new List<RaycastHit>(c_maxSupportedCollisions);

    public float m_curDotFriction;

    private float m_maxGravityMove;

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

        m_maxGravityMove = Mathf.Abs(m_gravity * c_maxDt * c_maxDt);
        m_maxGroundDist = m_centerRaycastDist;

        m_gravityVector = new Vector3(0, m_gravity, 0);
    }

    // Update is called once per frame
    void Update ()
    {
        m_forward = m_transform.forward;

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

        UpdateGroundPhysic();
        UpdateVelocity();
        UpdateAngularVelocity();

        m_XZvelocity = m_velocity;
        m_XZvelocity.y = 0;

        if (m_touchGround)
            UpdateGroundControl();
        else
            UpdateAirControl();
        
        m_position += m_XZvelocity * m_dt;
        CenterRaycast(m_Center);
        UpdateGravity();
        m_position -= m_XZvelocity * m_dt;

        UpdateCollision();
        // after collision velocity and angularVelocity can be applied safely

        m_position += (m_velocity + m_addVelocity) * m_dt;

        m_transform.rotation *= Quaternion.Euler(0, m_angularVelocity * m_dt, 0);

        //m_displacement = m_transform.position - m_position;
        m_transform.position = m_position;
    }

    public TextMesh[] m_specialDebug;

    private Quaternion m_newRot;

    private void ApplyCollision(RaycastHit rHit, Vector3 nDir, int i)
    {

        if (rHit.point == Vector3.zero)
        {
            Debug.DrawRay(m_position - m_raycastHelp * m_forward, nDir * m_raycastHelp * 2, Color.green);

            Physics.BoxCastNonAlloc(m_position - m_raycastHelp * m_forward, m_box, nDir, m_hits, m_newRot, m_velocity.magnitude * m_dt + m_raycastHelp, m_wallMask);
            rHit = m_hits[i];
        }
        Debug.DrawRay(rHit.point, rHit.normal, Color.blue);
        Debug.DrawRay(rHit.point, nDir, Color.red);

        float dot = Vector3.Dot(nDir, rHit.normal);
        if (m_specialDebug.Length > 0)
            m_specialDebug[i].text = dot.ToString();
        
        if (dot < -0.5f)
        {
            m_velocity.x = 0;
            m_velocity.z = 0;
            m_acceleration.x = 0;
            m_acceleration.z = 0;
        }
        else
        {
            float y = m_velocity.y;
            m_velocity += rHit.normal * m_speed * m_repulsion;
            m_velocity.y = y;
        }

        if (!m_touchGround)
        {
            m_velocity.y += m_gravity * m_dt;
        }

        m_position += rHit.normal * rHit.distance;
    }

    private void UpdateCollision()
    {
        float speed = m_velocity.magnitude;
        Vector3 nDir = m_velocity.normalized;

        m_newRot = m_transform.rotation * Quaternion.Euler(0, (m_angularVelocity + m_tempAngVelocity) * m_dt, 0);


        if (m_angularVelocity != 0)
        {
            RaycastHit rayHit;
            if (Physics.BoxCast(m_position - (m_newRot * Vector3.forward), m_box, (m_newRot * Vector3.forward), out rayHit, m_newRot, 1, m_wallMask))
            {
                m_angularVelocity = 0;
                m_newRot = m_transform.rotation;
            }
            else if (Physics.BoxCast(m_position + (m_newRot * Vector3.forward), m_box, (m_newRot * -Vector3.forward), out rayHit, m_newRot, 1, m_wallMask))
            {
                m_angularVelocity = 0;
                m_newRot = m_transform.rotation;
            }
        }

        if (speed > 0)
        {
            int hits = Physics.BoxCastNonAlloc(m_position, m_box, nDir, m_hits, m_newRot, speed * m_dt + m_raycastHelp, m_wallMask);
            for (int i = 0; i < hits; i++)
            {
                ApplyCollision(m_hits[i], nDir, i);
            }
        }
    }

    [Range(0, 10f)]
    public float m_raycastHelp;

    [Range(-0.5f, 0.5f)]
    public float m_gravityThreshold = 0.05f;

    [Range(0f, 15f)]
    public float m_threshold = 0.05f;

    private void UpdateGravity()
    {
        float delta = m_groundDist - m_lastGroundDist;


        m_WheelFL.text = delta.ToString("N2");

        // apply gravity
        if (!m_centerHit || (delta > m_gravityThreshold && m_groundDist > m_maxGravityMove))
        {
            m_gravityResult = m_gravityVector;

            if (m_touchGround)
            {
                m_touchGround = false;
                
                m_velocity.y = 0;
            }
        }
        else
        {
            m_gravityResult = m_gravityVector - m_groundNormal * m_gravity / m_groundNormal.y;

            if (!m_touchGround)
            {
                m_touchGround = true;
                //m_velocity.y = 0;
            }

            //m_velocity.y = 0;
            float newY = ((m_groundPos.y + m_groundOffset - m_position.y) / m_dt/* / m_dt*/);
            /*if ( m_velocity.y > newY + m_threshold)
            {
                //if (m_velocity.y > m_addVelocity.y)
                    //m_addVelocity.y = m_velocity.y;
            }*/

            //m_acceleration.y = newY;
            m_velocity.y = newY;

            /* if (newY < m_velocity.y - m_maxGravityMove)
             {
                 m_velocity.y -= m_gravity * m_dt;
             }
             else
             {
                 m_velocity.y = newY;
             }*/

            //m_position.y = m_groundPos.y + m_groundOffset;

            /*if (m_wheelOnGroundCount == 4 || m_wheelOnGroundCount < 3)
                m_position.y = m_CenterInfo.point.y;
            else
            {
                m_position.y = 0;
                float count = m_wheelOnGroundCount;
                for (int i = 0; i < count; i++)
                {
                    m_position.y += m_touchingWheelsInfos[i].point.y / count;
                }
            }
            m_position.y += m_groundOffset;*/
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
        else if (m_groundDist != m_maxGroundDist)
        {
            normal = m_groundNormal;
        }
        else
        {
            normal = Vector3.up;
        }

        if (normal.y < 0)
            normal = m_normal;


        Debug.DrawRay(m_transform.position, m_velocity, Color.blue);
        Vector3 dir = Vector3.ProjectOnPlane(m_transform.forward, normal);
        Debug.DrawRay(m_transform.position, dir, Color.red);
        Debug.DrawRay(m_transform.position, normal, Color.yellow);
        m_transform.LookAt(m_transform.position + dir, normal);

        m_normal = normal;
    }
    
    private Vector3 m_normal;

    private Coroutine m_endSliding;
    private float m_tempAngVelocity;

    IEnumerator EndSliding()
    {
        float m_initRot = m_angularVelocity;
        float timer = 0;
        while (timer < 2)
        {
            m_tempAngVelocity = Mathf.Lerp(0, 0, timer / 2f);
            yield return null;
            timer += Time.deltaTime;
        }
        m_tempAngVelocity = 0;
    }

    public float m_addGravity = -1;

    private void UpdateVelocity()
    {
        m_velocity += (m_acceleration + m_gravityResult) * m_dt;

        if (m_addVelocity.y > 0)
        {
            m_addVelocity.y += m_gravity * m_dt;

            if (m_addVelocity.y < 0)
                m_addVelocity.y = 0;
        }

        float y = m_velocity.y;
        m_velocity.y = 0;
        m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxVelocity);
        m_velocity.y = y;

        if (m_curDotFriction > 0 && m_speed > 0)
        {
            //float y = m_velocity.y;
            Vector3 forwardVelocity = m_forward * Vector3.Dot(m_forward, m_velocity);
            forwardVelocity.y = m_velocity.y;
            forwardVelocity = forwardVelocity.normalized * m_velocity.magnitude;
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

    private void UpdateAirFriction()
    {
        Vector3 velocityDir = m_velocity.normalized;
        if (Vector3.Dot(velocityDir, m_acceleration) > 0 && m_acceleration.magnitude > m_minAccelerationAirFriction)
        {
            if (m_acceleration.y > 0)
                m_acceleration.y = 0;
            m_acceleration -= velocityDir * m_airFrictionAcc * m_dt;
            if (Vector3.Dot(velocityDir, m_acceleration) < 0)
                m_acceleration = Vector3.zero;
        }

        UpdateFriction(m_airFriction);
    }

    private bool m_slidingInput;
    
    public float m_frictionDownSpeed = 5;

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
            if (!m_sliding)
            {
                m_sliding = true;
                if (gamePadState.State.ThumbSticks.Left.X > 0)
                    m_sliddingRight = true;
                else
                    m_sliddingRight = false;

                m_trace.enabled = true;
                m_slidingStateTimer = 0;
            }
            if (m_curDotFriction > 0)
            {
                m_curDotFriction -= m_dt * m_frictionDownSpeed;
                if (m_curDotFriction < 0)
                    m_curDotFriction = 0;
            }
            m_slidingDir = m_sliddingRight ? m_transform.right : m_transform.rotation * -Vector3.right;
            m_acceleration += m_slidingAcc * m_slidingDir * m_slidingCoef * m_speed * Mathf.Abs(m_angularVelocity) * m_dt;

            float sliding = (m_maxSliding + slideInput);

            m_currentMaxAngularVelocity = sliding * m_maxAngularVelocity;
            m_angularAcceleration = sliding * (m_sliddingRight ? m_currentMaxAngularAcc : -m_currentMaxAngularAcc);
            m_slidingStateTimer += m_dt;
        }
        else
        {
            if (m_sliding)
            {
                m_sliding = false;
                m_trace.RemoveAll();
                m_trace.enabled = false;
                m_maxSliding = 0;
                m_slidingStateTimer = 0;
            }

            if (m_curDotFriction < m_dotFriction)
            {
                m_curDotFriction = m_slidingUp.Evaluate(m_slidingStateTimer);
            }

            m_angularAcceleration = gamePadState.State.ThumbSticks.Left.X * m_currentMaxAngularAcc;
            m_slidingStateTimer += m_dt;
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

    public AnimationCurve m_slidingUp;
    private float m_slidingStateTimer;

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
        m_speed = m_XZvelocity.magnitude;

        if (m_speed < c_minVelocity)
        {
            m_velocity.x = 0;
            m_velocity.z = 0;
        }
        else
        {
            m_velocity -= m_XZvelocity * _coef * m_maxAcceleration * m_dt;
        }
    }

    RaycastHit WheelRaycast(Transform _tr, TextMesh _text, int _lastPosIdx)
    {
        RaycastHit hit;
        Vector3 dir = (_tr.position - m_lastWheelPos[_lastPosIdx] - _tr.up);
        Vector3 origin = m_lastWheelPos[_lastPosIdx] + m_groundOffset * _tr.up;
        m_lastWheelPos[_lastPosIdx] = _tr.position;

        bool hitBool = Physics.Raycast(origin, dir, out hit, m_wheelRaycastDist, (int)m_groundMask);
        Debug.DrawRay(origin, dir * m_wheelRaycastDist, Color.red);
        if (hitBool)
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        if (hitBool)
            m_wheelOnGroundCount++;

        //_text.text = (hit.distance).ToString("n2");

        if (hitBool)
            m_touchingWheelsInfos.Add(hit);
        return hit;
    }

    private bool m_centerHit;
    
    RaycastHit CenterRaycast(TextMesh _text)
    {
        RaycastHit hit;
        Vector3 dir = -Vector3.up;
        float dist = m_maxGroundDist;
        
        m_lastGroundDist = m_groundDist;

        m_centerHit = Physics.Raycast(m_position, dir, out hit, dist, (int)m_groundMask);
        Debug.DrawRay(m_position, dir * dist, Color.red);
        if (m_centerHit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            m_groundDir = hit.point - m_position;
            m_groundDir.y += m_groundOffset;
            m_groundDist = m_groundDir.magnitude;
            m_groundNormal = hit.normal;
            if (m_groundDir.y > 0)
                m_groundDist *= -1;
            m_groundPos = hit.point;
        }

        _text.text = (m_groundDist).ToString("n2");
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
