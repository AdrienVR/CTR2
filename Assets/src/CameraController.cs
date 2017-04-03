using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float PositionForward = 0.85f;
    public float Reversed = 1f;
    public Vector3 backward;

    void Awake()
    {
        enabled = true;
    }

    void Start()
    {
        m_parent = transform.parent;
        m_startTime = Time.time;
        Vector3 objectiv = m_parent.position + backward - Reversed * PositionForward * 6 * m_parent.forward + m_parent.right * 4;
        //Vector3 tr = (transform.position - objectiv)/20f;
        //while (Vector3.Distance(transform.position, objectiv)>100f){
        transform.position = 0.75f * objectiv;
        //}
        m_journeyLength = Vector3.Distance(transform.position, m_parent.position);
        //Debug.Log ("duree de voyage " + journeyLength);
    }

    void FixedUpdate()
    {
        if (KartController.stop == true)
        {
            float distCovered = (Time.time - m_startTime);
            float fracJourney = distCovered / m_journeyLength;
            Vector3 objectiv = m_parent.position + backward - Reversed * PositionForward * 6 * m_parent.forward;
            transform.position = Vector3.Lerp(transform.position, objectiv, fracJourney * 0.75f);
            transform.LookAt(m_parent.position + new Vector3(0f, 2f, 0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (KartController.stop == false)
        {
            transform.position = m_parent.position + backward - Reversed * PositionForward * 6 * m_parent.forward;
            transform.LookAt(m_parent.position + new Vector3(0f, 2f, 0f));
        }
    }

    private Transform m_parent;

    private float m_startTime;
    private float m_journeyLength;
}
