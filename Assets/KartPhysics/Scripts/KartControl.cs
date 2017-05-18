
#define KART_DEBUG

using UnityEngine;

public class KartControl : MonoBehaviour {

    public Transform WheelFL;
    public Transform WheelFR;
    public Transform WheelBL;
    public Transform WheelBR;
    public Camera m_cam;

    public float m_wheelOffsetWidth;
    public float m_wheelOffsetLength;
    public float m_wheelHeight;

    public float m_raycastMaxDist = 0.6f;

    public float m_gravity = -10;

    public float m_dampingForce;

    public LayerMask m_cast;

#if KART_DEBUG
    [HideInInspector][SerializeField]
    private TextMesh m_WheelFL;
    [HideInInspector][SerializeField]
    private TextMesh m_WheelFR;
    [HideInInspector][SerializeField]
    private TextMesh m_WheelBL;
    [HideInInspector][SerializeField]
    private TextMesh m_WheelBR;

    [ContextMenu("OnValidate")]
    void OnValidate()
    {
        m_WheelFL = WheelFL.GetComponentInChildren<TextMesh>();
        m_WheelFR = WheelFR.GetComponentInChildren<TextMesh>();
        m_WheelBL = WheelBL.GetComponentInChildren<TextMesh>();
        m_WheelBR = WheelBR.GetComponentInChildren<TextMesh>();
    }
#endif // KART_DEBUG

    void Start()
    {
        //m_rigidBody.con(new Vector3(0, m_gravity), ForceMode.Acceleration);
        m_transform = transform;
    }

    public RaycastHit m_WheelFLInfo;
    public RaycastHit m_WheelFRInfo;
    public RaycastHit m_WheelBLInfo;
    public RaycastHit m_WheelBRInfo;

    public Vector3 m_direction;

    public Vector3 m_acceleration;
    public Vector3 m_velocity;

    private Transform m_transform;
    public int m_wheelOnGroundCount;

    public float m_angularVelocity;
    public float m_angularAcceleration;

    public float m_maxAngularAcceleration;

    // Update is called once per frame
    void Update () {

        m_wheelOnGroundCount = 0;
        m_WheelFLInfo = UpdateWheel(WheelFL, m_WheelFL);
        m_WheelFRInfo = UpdateWheel(WheelFR, m_WheelFR);
        m_WheelBLInfo = UpdateWheel(WheelBL, m_WheelBL);
        m_WheelBRInfo = UpdateWheel(WheelBR, m_WheelBR);

        // les roues arrières ne vont pas vers 
        //if (m_WheelBLInfo.normal)

        if (m_wheelOnGroundCount == 0)
            m_acceleration.y = -10;
        else
        {
            m_acceleration.y = 0;
            m_velocity.y = 0;
        }

        float dt = Time.deltaTime;
        m_transform.position += m_velocity * dt;

        m_velocity += m_acceleration * dt;

        m_transform.rotation *= Quaternion.Euler(0, m_angularVelocity * dt, 0);

        m_angularVelocity += m_angularAcceleration * dt;

        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            m_angularAcceleration = 1;
        }
        else
        {
            m_angularAcceleration = 0;
        }

        m_angularAcceleration = Mathf.Clamp(m_angularAcceleration, -m_maxAngularAcceleration, m_maxAngularAcceleration);
    }

    RaycastHit UpdateWheel(Transform _wheel, TextMesh _mesh)
    {
        RaycastHit hit;
        bool hitBool = Physics.Raycast(_wheel.position, -Vector3.up, out hit, m_raycastMaxDist, (int)m_cast);
        //Debug.DrawRay(_wheel.position, -Vector3.up * m_raycastMaxDist, hitBool ? Color.green : Color.red);
        float dist = hit.distance / m_raycastMaxDist;
        if (!hitBool)
            dist = 2;
        else
            m_wheelOnGroundCount++;
        _mesh.text = (dist).ToString("n2");
        hit.distance = dist;
        //m_rigidBody.AddForceAtPosition(Vector3.up * (1 - dist) * m_dampingForce * Time.deltaTime, _wheel.position, ForceMode.VelocityChange);
        return hit;
    }


    [ContextMenu("SetOffsets")]
    void SetOffsets()
    {
        WheelFL.localPosition = new Vector3(-m_wheelOffsetWidth, m_wheelHeight, m_wheelOffsetLength);
        WheelFR.localPosition = new Vector3(m_wheelOffsetWidth, m_wheelHeight, m_wheelOffsetLength);
        WheelBL.localPosition = new Vector3(-m_wheelOffsetWidth, m_wheelHeight, -m_wheelOffsetLength);
        WheelBR.localPosition = new Vector3(m_wheelOffsetWidth, m_wheelHeight, -m_wheelOffsetLength);
    }
}
