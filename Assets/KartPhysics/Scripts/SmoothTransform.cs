
using UnityEngine;

public class SmoothTransform : MonoBehaviour
{
    public float m_smoothSpeedPosition = 15;
    public float m_smoothSpeedRotation = 10;

    [Range(0.01f, 0.02f)]
    public float m_dt = 0.015f;

    private Transform m_transform;
    [SerializeField]
    private Transform m_target;
    
    void Start()
    {
        m_transform = transform;
    }
    
    void Update()
    {
        m_transform.position = Vector3.Lerp(m_transform.position, m_target.position, m_smoothSpeedPosition * m_dt);
        m_transform.rotation = Quaternion.Lerp(m_transform.rotation, m_target.rotation, m_smoothSpeedRotation * m_dt);
    }
}
