using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int PlayerIndex;

    [HideInInspector]
    public CharacterSide CharacterSide;

    [HideInInspector]
    public List<GameObject> WeaponPrefab;

    public KartTransformer KartTransformer;

    [HideInInspector]
    public KartState KartState;

    [HideInInspector]
    public ControllerBase Controller;

    [HideInInspector]
    public CameraController CameraController;

    public AnimationCurve WheelRotationCurve;
    public AnimationCurve SpeedCurve;

    public KartRigidBody KartRigidbody;

    public float AcceleratingFactor;
    public float TurnSpeed;
    public float DeceleratingFactor;
    public float MaxSpeed;

	public SkidTrace2 TraceL, TraceR;

	public UIPlayerManager UIPlayerManager;

	public int NbApplesTmp, NbApples;
    

    // Use this for initialization
    void Start()
    {
        KartState = new KartState();
        m_animator = GetComponent<Animator>();
        Controller = ControllerManager.Instance.GetController(PlayerIndex);
        KartRigidbody.transform = transform;
        KartRigidbody.position = transform.position;
        KartTransformer.KartRigidbody = KartRigidbody;
        KartTransformer.Start();

        KartRigidbody.KartTransformer = KartTransformer;
        KartRigidbody.Initialize();

        WeaponPrefab = new List<GameObject>();

		TraceL = KartTransformer.BottomLeftParent.GetComponent<SkidTrace2> ();
		TraceR = KartTransformer.BottomRightParent.GetComponent<SkidTrace2> ();
    }

    // Update is called once per frame
    void Update()
    {
        KartTransformer.Update();
        UpdateCamera();

        if (KartState.CanMove())
            UpdateGameplay();
        KartRigidbody.Update();
        KartState.Update();
        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        if (Controller.GetKeyDown("action"))
        {
            if (WeaponPrefab.Count > 0 && KartState.WeaponLocked == false)
            {
                GameObject weaponGo = WeaponPrefab[0];
                WeaponPrefab.RemoveAt(0);
                weaponGo = Instantiate(weaponGo);
                WeaponBehavior wb = weaponGo.GetComponent<WeaponBehavior>();
                wb.Initialize(this);

                if (WeaponPrefab.Count == 0)
                {
                    UIPlayerManager.HideWeapon();
                    KartState.IsArmed = false;
                }
                else
                {
                    UIPlayerManager.DecrementWeaponText();
                }
            }
        }
    }

    void UpdateGameplay()
    {
        if (Controller.GetKey("validate"))
        {
            //Debug.Log(m_acceleratingTimer);
            if (m_acceleratingTimer < 1)
            {
                m_acceleratingTimer += Time.deltaTime * AcceleratingFactor;
                if (m_acceleratingTimer > 1)
                {
                    m_acceleratingTimer = 1;
                }
            }
        }
        else
        {
            if (m_acceleratingTimer > 0)
            {
                m_acceleratingTimer -= Time.deltaTime * DeceleratingFactor;
                if (m_acceleratingTimer < 0)
                {
                    m_acceleratingTimer = 0;
                }
            }
        }
        if (m_acceleratingTimer > 0)
        {
            KartRigidbody.position += transform.forward * SpeedCurve.Evaluate(m_acceleratingTimer) * MaxSpeed;
        }
        else if (m_acceleratingTimer < 0)
        {
            KartRigidbody.position -= transform.forward * SpeedCurve.Evaluate(-m_acceleratingTimer) * MaxSpeed;
        }
        // wheels turning
        if ((Controller.GetKey("stop") || Controller.GetKey("validate")))
        {
            if (Controller.GetKey("right"))
                KartTransformer.YAngle += 0.5f * Controller.GetAxis("right") * TurnSpeed;
            if (Controller.GetKey("left"))
                KartTransformer.YAngle -= 0.5f * Controller.GetAxis("left") * TurnSpeed;
        }
        else if (Controller.GetKey("stop") == false)
        {
            if (Controller.GetKey("down") && Controller.GetAxis("down") > 0.9f)
            {
                if (m_acceleratingTimer > -1)
                    m_acceleratingTimer -= Time.deltaTime * AcceleratingFactor * 2;
                if (Controller.GetKey("right"))
                    KartTransformer.YAngle -= 0.5f * Controller.GetAxis("right") * TurnSpeed;
                else if (Controller.GetKey("left"))
                    KartTransformer.YAngle += 0.5f * Controller.GetAxis("left") * TurnSpeed;
            }
            else if (m_acceleratingTimer < 0)
            {
                m_acceleratingTimer += Time.deltaTime * AcceleratingFactor * 2;
            }
        }

        if (Controller.GetKeyDown("jump"))
        {
        }
		
		if (Controller.GetKey("jump") && KartTransformer.isInAir == false && KartTransformer.YAngle != 0)
		{
			ActiveTrace(true);
		}
		else
		{
			ActiveTrace(false);
		}

    }

	public void ActiveTrace(bool state)
	{
		if(TraceL != null && TraceR != null)
		{
			if(state == false)
			{
				TraceL.RemoveAll();
				TraceR.RemoveAll();
			}
			TraceL.enabled = state;
			TraceR.enabled = state;
		}
	}

    public void UpdateCamera()
    {
        if (Controller.GetKeyDown("inverseCamera"))
        {
            CameraController.Reversed = -1f;
        }

        if (Controller.GetKeyUp("inverseCamera"))
        {
            CameraController.Reversed = 1f;
        }

        if (Controller.GetKeyDown("switchCamera"))
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

    public void CollisionStop()
    {
        m_acceleratingTimer = 0;
        AudioManager.Instance.Play("Ouille");
        KartState.SetUnabilityToMove(0.666f);
        m_animator.Play("Collision");
    }

    public void Die(PlayerController killer, string weapon)
    {
        m_acceleratingTimer = 0;
        AudioManager.Instance.Play("Ouille");
        KartState.SetUnabilityToMove(1.75f);
        m_animator.Play("Death");
    }

    private Animator m_animator;
    private float m_acceleratingTimer;
}
