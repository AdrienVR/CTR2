
using UnityEngine;

public class KartCamera : MonoBehaviour
{
    public Transform m_kartTransform;

    public bool m_far;

    public Vector3 m_smallOffset;
    public Vector3 m_bigOffset;

    public float m_smoothSpeedPosition;
    public float m_smoothSpeedRotation;

    public float m_angleX;

    [Range(0.01f, 0.02f)]
    public float m_dt;

    private Transform m_transform;

#if UNITY_EDITOR
    [ContextMenu("SetSmallOffset")]
    void SetSmallOffset()
    {
        UnityEditor.Undo.RecordObject(this, "SetSmallOffset");
        m_smallOffset = transform.position - m_kartTransform.position;
    }
    [ContextMenu("SetBigOffset")]
    void SetBigOffset()
    {
        UnityEditor.Undo.RecordObject(this, "SetBigOffset");
        m_bigOffset = transform.position - m_kartTransform.position;
    }
#endif

    // Use this for initialization
    void Start()
    {
        m_transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //m_dt = Time.deltaTime;
        m_transform.position = Vector3.Lerp(m_transform.position, m_kartTransform.position + m_kartTransform.rotation * (m_far ? m_bigOffset : m_smallOffset), m_smoothSpeedPosition * m_dt);
        m_transform.rotation = Quaternion.Lerp(m_transform.rotation, m_kartTransform.rotation * Quaternion.Euler(m_angleX, 0, 0), m_smoothSpeedRotation * m_dt);
    }
}
