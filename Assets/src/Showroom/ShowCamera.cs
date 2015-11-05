using UnityEngine;

public class ShowCamera : MonoBehaviour
{
    public float MovingDuration;

    // Use this for initialization
    void Start()
    {
        m_currentDestination = transform.position;
        m_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer < MovingDuration)
        {
            m_timer += Time.deltaTime;

            if (m_destination.Length == 1)
            {
                transform.position = LerpMove(transform.position, m_destination[0], m_timer / MovingDuration);
            }
            else if (m_destination.Length == 3)
            {
                if (m_timer < MovingDuration * 0.5f)
                {
                    transform.position = LerpMove(transform.position, m_destination[0], m_timer / (MovingDuration * 0.5f));
                }
                else
                {
                    if (m_teleported == false)
                    {
                        transform.position = m_destination[1];
                        m_teleported = true;
                    }
                    else
                    {
                        transform.position = LerpMove(transform.position, m_destination[2], (m_timer - MovingDuration*0.5f) / (MovingDuration* 0.5f));
                    }
                }
            }
        }
    }

    public void GoTo(Vector3[] destination)
    {
        transform.position = destination[0];
        if (m_destination.Length == 1)
        {
            transform.position = m_destination[0];
        }
        else if (m_destination.Length == 3)
        {
            transform.position = m_destination[2];
        }
        m_destination = destination;
        m_timer = 0;
        m_teleported = false;
    }

    private Vector3 LerpMove(Vector3 from, Vector3 to, float value)
    {
        return Vector3.Lerp(from, to, value);
    }

    private float m_timer;
    private Vector3[] m_destination;
    private bool m_teleported = false;
}
