using UnityEngine;
using System.Collections;

public class WheelRotation : MonoBehaviour
{
    public Transform LeftWheel;
    public Transform RightWheel;

    public float m_maxAngle = 40;

    [System.NonSerialized]
    public float m_horizontalFactor;
    private Quaternion m_initRotation;

#if UNITY_EDITOR
    private void OnValidate()
    {
        LeftWheel = ComponentUtils.GetByNameInChildren<Transform>(gameObject, "FrontLeft");
        RightWheel = ComponentUtils.GetByNameInChildren<Transform>(gameObject, "FrontRight");
    }
#endif

    public void Start()
	{
        m_initRotation = LeftWheel.localRotation;
	}

	void Update()
    {
        Quaternion wheelRotation = Quaternion.Euler(0, m_horizontalFactor * m_maxAngle, 0);
        LeftWheel.localRotation = m_initRotation * wheelRotation;
        RightWheel.localRotation = m_initRotation * wheelRotation;
    }
}
