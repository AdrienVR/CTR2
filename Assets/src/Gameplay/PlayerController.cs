using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int PlayerIndex;

    [HideInInspector]
    public Team Team;

    [HideInInspector]
    public CharacterSide CharacterSide;

    [HideInInspector]
    public List<GameObject> WeaponPrefab;

    public KartTransformer KartTransformer;

    [HideInInspector]
    public KartState KartState;

    [HideInInspector]
    public CameraController CameraController;

    public AnimationCurve WheelRotationCurve;
    public AnimationCurve SpeedCurve;

    public KartRigidBody KartRigidbody;

    public float AcceleratingFactor;
    public float TurnSpeed;
    public float DeceleratingFactor;
    public float MaxSpeed;

    public float MaxTorque = 100;

    [HideInInspector]
    public float SpeedCoefficient = 1;


    public AcceleratorBehavior Accelerator;

    [HideInInspector]
    public SkidTrace2 TraceL, TraceR;

    [HideInInspector]
    public UIPlayerManager UIPlayerManager;

    [HideInInspector]
    public int NbApplesTmp, NbApples, NbPts;

    private GamePadStateHolder gamePadState;
    private GamePadStateHolder lastGamePadState;

#if UNITY_EDITOR
    private void OnValidate()
    {
        m_wheelRotation = GetComponentInChildren<WheelRotation>();
    }

    [ContextMenu("SetPrefabParentNull")]
    void SetPrefabParentNull()
    {
        UnityEditor.PrefabUtility.DisconnectPrefabInstance(gameObject);
    }
#endif

    // Use this for initialization
    void Start()
    {
        gamePadState = ControllerManager.Instance.GamePadStates[PlayerIndex];
        lastGamePadState = ControllerManager.Instance.LastGamePadStates[PlayerIndex];
        m_input = new KartInput(gamePadState, lastGamePadState);

        KartState = new KartState();
        m_animator = GetComponent<Animator>();
        KartRigidbody.transform = transform;
        KartRigidbody.position = transform.position;
        KartTransformer.KartRigidbody = KartRigidbody;

        KartRigidbody.KartTransformer = KartTransformer;
        KartRigidbody.Initialize();

        WeaponPrefab = new List<GameObject>();

        TraceL = GetComponentsInChildren<SkidTrace2>()[0];
        TraceR = GetComponentsInChildren<SkidTrace2>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePadState.State.IsConnected == false)
            return;
        m_input.UpdateInput();
        //KartTransformer.Update();
        UpdateCamera();

        if (KartState.CanMove())
            UpdateGameplay();
        m_wheelRotation.m_horizontalFactor = m_input.horizontal;
        KartRigidbody.Update();
        KartState.Update();
        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        if (m_input.fire)
        {
            if (KartState.UsingWeapon)
            {
                KartState.UsingWeapon.Activate();
            }
            else if (WeaponPrefab.Count > 0)
            {
                GameObject weaponGo = WeaponPrefab[0];
                WeaponPrefab.RemoveAt(0);
                weaponGo = Instantiate(weaponGo);
                WeaponBehavior wb = weaponGo.GetComponent<WeaponBehavior>();
                wb.Owner = this;
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
        if (gamePadState.State.Buttons.A == ButtonState.Pressed)
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

        KartTransformer.BackWheels.leftWheel.motorTorque = m_acceleratingTimer * MaxTorque;
        KartTransformer.BackWheels.rightWheel.motorTorque = m_acceleratingTimer * MaxTorque;

        if (m_acceleratingTimer > 0)
        {
            KartRigidbody.position += transform.forward * SpeedCurve.Evaluate(m_acceleratingTimer) * MaxSpeed;
        }
        else if (m_acceleratingTimer < 0)
        {
            KartRigidbody.position -= transform.forward * SpeedCurve.Evaluate(-m_acceleratingTimer) * MaxSpeed;
        }

        float yAngle = 0;
        // wheels turning
        if ((gamePadState.State.Buttons.X == ButtonState.Pressed || gamePadState.State.Buttons.A == ButtonState.Pressed))
        {
            if (m_input.stop || m_input.moveForward)
            {
                yAngle = m_input.horizontal * TurnSpeed;
            }
            else if (m_input.horizontal < 0)
            {
                yAngle = -m_input.horizontal * TurnSpeed;
            }
        }
        else if (!m_input.stop)
        {
            if (m_input.vertical > 0.9f)
            {
                if (m_acceleratingTimer > -1)
                    m_acceleratingTimer -= Time.deltaTime * AcceleratingFactor * 2;
                if (m_input.horizontal > 0)
                {
                    yAngle = -m_input.horizontal * TurnSpeed;
                }
                else if (m_input.horizontal < 0)
                {
                    yAngle = m_input.horizontal * TurnSpeed;
                }
            }
            else if (m_acceleratingTimer < 0)
            {
                m_acceleratingTimer += Time.deltaTime * AcceleratingFactor * 2;
            }
        }

        KartTransformer.YAngle += (yAngle * Time.deltaTime);

        KartTransformer.FrontWheels.leftWheel.transform.localRotation = Quaternion.Euler(0, m_acceleratingTimer * MaxTorque, 0);
        KartTransformer.FrontWheels.rightWheel.transform.localRotation = Quaternion.Euler(0, m_acceleratingTimer * MaxTorque, 0);

        if (m_input.jump)
        {
        }

        if (m_input.jump && KartTransformer.isInAir == false && yAngle != 0)
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
        if (TraceL != null && TraceR != null)
        {
            if (state == false)
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
        if (ControllerManager.Instance.GetYDown(PlayerIndex))
        {
            CameraController.Reversed = -1f;
        }
        else if (ControllerManager.Instance.GetYUp(PlayerIndex))
        {
            CameraController.Reversed = 1f;
        }

        if (m_input.lTriggerUp)
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
        if (n > 0)
        {
            NbApples = System.Math.Min(10, NbApples + n);
            StartCoroutine(animApplesNb());
        }
        else
            NbApples = System.Math.Max(0, NbApples + n);


    }

    IEnumerator animApplesNb()
    {
        while (NbApplesTmp < NbApples)
        {
            NbApplesTmp++;
            //kart.SetIllumination((kart.nbApples == 10));
            //GetComponent<AudioSource>().Play();
            UIPlayerManager.AnimApple();
            UIPlayerManager.SetAppleText(NbApplesTmp.ToString());
            yield return new WaitForSeconds(0.27f);
        }
        if (NbApples >= 10)
            UIPlayerManager.SetSuperWeapons();
        else
            UIPlayerManager.UnsetSuperWeapons();
    }

    int lastFrame;

    public void CollisionStop()
    {
        if (Time.frameCount - lastFrame < 20)
            return;
        m_acceleratingTimer = 0;
        AudioManager.Instance.Play("Ouille");
        //KartState.SetUnabilityToMove(0.666f);
        m_animator.Play("Collision");
        lastFrame = Time.frameCount;
    }

    public bool IsSuper()
    {
        return NbApples == 10;
    }

    public void Boost(float duration)
    {
        AcceleratorBehavior accelerator = Instantiate(Accelerator) as AcceleratorBehavior;
        accelerator.Owner = this;
        accelerator.SetBoost(duration);
    }

    public void PlayBoostAnimation()
    {
        m_animator.Play("Boost");
    }

    public void Hit(PlayerController killer, string weapon)
    {
        if (KartState.UsingWeapon != null)
            KartState.UsingWeapon.OnHit();
        if (KartState.TempBuffs["SingleHitProtection"] == 1)
        {
            KartState.SetInvincibility(1.75f);
            KartState.TempBuffs["SingleHitProtection"] = 0;
        }
        else if (KartState.IsInvincible() == false)
        {
            Die(killer, weapon);
        }
    }

    public void Die(PlayerController killer, string weapon)
    {
        m_acceleratingTimer = 0;
        AudioManager.Instance.Play("Ouille");
        KartState.SetUnabilityToMove(1.75f);
        KartState.SetInvincibility(1.75f);
        m_animator.Play("Death");
        if (killer != this)
            killer.AddPoint();
        else
            LosePoint();

        PlayModeManager.Instance.UpdateDeath(this, killer);

    }

    public void AddPoint()
    {
        NbPts++;
        UIPlayerManager.AnimPts(NbPts.ToString(), false);
    }
    public void LosePoint()
    {
        NbPts--;
        UIPlayerManager.AnimPts(NbPts.ToString(), true);
    }

    [SerializeField]
    private KartInput m_input;
    private Animator m_animator;
    private float m_acceleratingTimer;
    [SerializeField][HideInInspector]
    private WheelRotation m_wheelRotation;
}
