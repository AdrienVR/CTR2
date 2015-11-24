using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int PlayerIndex;

    public KartTransformer KartTransformer;

    [HideInInspector]
    public CameraController CameraController;

    public AnimationCurve WheelRotationCurve;
    public AnimationCurve SpeedCurve;

    public float AcceleratingFactor;
    public float TurnSpeed;
    public float DeceleratingFactor;
    public float MaxSpeed;

    // Use this for initialization
    void Start()
    {
        m_controller = ControllerManager.Instance.GetController(PlayerIndex);
        m_rigidbody = GetComponent<Rigidbody>();
        KartTransformer.KartRigidbody = m_rigidbody;
        KartTransformer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        KartTransformer.Update();
        UpdateCamera();
        UpdateGameplay();
    }

    public void UpdateGameplay()
    {
        if (m_controller.GetKey("validate"))
        {
            //Debug.Log(m_acceleratingTimer);
            if (m_acceleratingTimer < 1)
                m_acceleratingTimer += Time.deltaTime * AcceleratingFactor;
        }
        else
        {
            if (m_acceleratingTimer > 0)
                m_acceleratingTimer -= Time.deltaTime * DeceleratingFactor;
        }
        if (m_acceleratingTimer > 0)
        {
            m_rigidbody.position += transform.forward * SpeedCurve.Evaluate(m_acceleratingTimer) * MaxSpeed;
        }
        else
        {
            m_rigidbody.position -= transform.forward * SpeedCurve.Evaluate(-m_acceleratingTimer) * MaxSpeed;
        }
        // wheels turning
        if ((m_controller.GetKey("stop") || m_controller.GetKey("validate")))
        {
            if (m_controller.GetKey("right"))
                KartTransformer.YAngle += 0.5f * m_controller.GetAxis("right") * TurnSpeed;
            if (m_controller.GetKey("left"))
                KartTransformer.YAngle -= 0.5f * m_controller.GetAxis("left") * TurnSpeed;
        }
        else if (m_controller.GetKey("stop") == false)
        {
            if (m_controller.GetKey("down") && m_controller.GetAxis("down") > 0.9f)
            {
                if (m_acceleratingTimer > -1)
                    m_acceleratingTimer -= Time.deltaTime * AcceleratingFactor * 2;
                if (m_controller.GetKey("right"))
                    KartTransformer.YAngle -= 0.5f * m_controller.GetAxis("right") * TurnSpeed;
                else if (m_controller.GetKey("left"))
                    KartTransformer.YAngle += 0.5f * m_controller.GetAxis("left") * TurnSpeed;
            }
            else if (m_acceleratingTimer < 0)
            {
                m_acceleratingTimer += Time.deltaTime * AcceleratingFactor * 2;
            }
        }

        if (m_controller.GetKeyDown("jump"))
        {
        }

    }

    public void UpdateCamera()
    {
        if (m_controller.GetKeyDown("inverseCamera"))
        {
            CameraController.Reversed = -1f;
        }

        if (m_controller.GetKeyUp("inverseCamera"))
        {
            CameraController.Reversed = 1f;
        }

        if (m_controller.GetKeyDown("switchCamera"))
        {
            if (CameraController.PositionForward == 1f)
                CameraController.PositionForward = 0.85f;
            else
                CameraController.PositionForward = 1f;
        }
    }

    private ControllerBase m_controller;
    private float m_acceleratingTimer;
    private Rigidbody m_rigidbody;
}
