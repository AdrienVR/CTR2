
using UnityEngine;

public class KartCamera : MonoBehaviour
{

    public KartControl m_kart;

    public bool m_far;

    public Vector3 m_smallOffset;
    public Vector3 m_bigOffset;

    public float m_smoothSpeedPosition;
    public float m_smoothSpeedRotation;

    public float m_angleX;

    private Transform m_transform;
    private Transform m_kartTransform;

#if UNITY_EDITOR
    [ContextMenu("SetSmallOffset")]
    void SetSmallOffset()
    {
        UnityEditor.Undo.RecordObject(this, "SetSmallOffset");
        m_smallOffset = transform.position - m_kart.transform.position;
    }
    [ContextMenu("SetBigOffset")]
    void SetBigOffset()
    {
        UnityEditor.Undo.RecordObject(this, "SetBigOffset");
        m_bigOffset = transform.position - m_kart.transform.position;
    }
#endif

    // Use this for initialization
    void Start()
    {
        m_transform = transform;
        m_kartTransform = m_kart.transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_transform.position = Vector3.Lerp(m_transform.position, m_kartTransform.position + m_kartTransform.rotation * (m_far ? m_bigOffset : m_smallOffset), m_smoothSpeedPosition * Time.deltaTime);
        m_transform.rotation = Quaternion.Lerp(m_transform.rotation, m_kartTransform.rotation * Quaternion.Euler(m_angleX, 0, 0), m_smoothSpeedRotation * Time.deltaTime);
    }
}
