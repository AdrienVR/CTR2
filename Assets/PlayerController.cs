using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

    public KartRigidBody KartRigidbody;

    public float AcceleratingFactor;
    public float TurnSpeed;
    public float DeceleratingFactor;
    public float MaxSpeed;

	public UIPlayerManager UIPlayerManager;

	public int NbApplesTmp, NbApples;

    // Use this for initialization
    void Start()
    {
        m_controller = ControllerManager.Instance.GetController(PlayerIndex);
        KartRigidbody.transform = transform;
        KartRigidbody.position = transform.position;
        KartTransformer.KartRigidbody = KartRigidbody;
        KartTransformer.Start();

        KartRigidbody.KartTransformer = KartTransformer;
    }

    // Update is called once per frame
    void Update()
    {
        KartTransformer.Update();
        UpdateCamera();
        UpdateGameplay();
        KartRigidbody.Update();
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
            KartRigidbody.position += transform.forward * SpeedCurve.Evaluate(m_acceleratingTimer) * MaxSpeed;
        }
        else
        {
            KartRigidbody.position -= transform.forward * SpeedCurve.Evaluate(-m_acceleratingTimer) * MaxSpeed;
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

	public void ChangeApples(int n)
	{
		NbApplesTmp = NbApples;
		if(n >0)
		{
			NbApples = System.Math.Min(10, NbApples + n);
			StartCoroutine (animApplesNb());
		}
		else
			NbApples = System.Math.Max(0, NbApples + n);

	}

	IEnumerator animApplesNb()
	{
		while(NbApplesTmp < NbApples)
		{
			NbApplesTmp ++;
			//kart.SetIllumination((kart.nbApples == 10));
			//GetComponent<AudioSource>().Play();
			UIPlayerManager.AnimApple();
			UIPlayerManager.SetAppleText(NbApplesTmp.ToString());
			yield return new WaitForSeconds (0.27f);
		}
	}

    private ControllerBase m_controller;
    private float m_acceleratingTimer;
}
