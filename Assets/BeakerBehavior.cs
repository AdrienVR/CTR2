//#define DEBUG_VARS

using UnityEngine;

#if DEBUG_VARS
using HideVar = UnityEngine.HideInInspector;
#else
public class HideVar : System.Attribute
{ }
#endif

public class BeakerBehavior : WeaponBehavior
{
    public Color ExplosionColor;
    public float LightDuration;
    public AnimationCurve HeightCurve;
    public float CurveProgressionSpeed;
    public float HeightAmplitude;
    public float LaunchingSpeed;
    public Light Light;
    [HideVar]
    public bool ApplyEffect;

    public override void Initialize(bool backWard)
    {
        m_renderer = GetComponent<Renderer>();
        m_rigidbody = GetComponent<Rigidbody>();

        transform.forward = Owner.transform.forward;

        if (!backWard)
        {
            m_rigidbody.position = Owner.transform.position + transform.forward * 3f + Vector3.up;
            m_initialHeight = m_rigidbody.position.y;
            m_launched = true;
        }
        else
        {
            m_rigidbody.position = Owner.transform.position - transform.forward * 3f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Hit(Owner, name);
            //if (ApplyEffect)
            //    player.
        }
        Explode();
    }

    void Update()
    {
        if (m_exploded == true)
        {
            m_explosionTimer -= Time.deltaTime;

            Light.color = Color.Lerp(Color.black, ExplosionColor, m_explosionTimer / LightDuration);

            if (m_explosionTimer < 0)
            {
                Destroy(gameObject);
            }
        }
        else if (m_launched)
        {
            RaycastHit hitGround;
            if (Physics.Raycast(transform.position, -Vector3.up, out hitGround, 0.5f, Consts.Layer.Ground))
            {
                m_launched = false;
                Destroy(m_rigidbody);
                transform.up = hitGround.normal;
            }
            m_launchedTimer += Time.deltaTime * CurveProgressionSpeed;
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = m_rigidbody.position + transform.forward* LaunchingSpeed *Time.deltaTime;
        newPosition.y = m_initialHeight + HeightCurve.Evaluate(m_launchedTimer) * HeightAmplitude;
        m_rigidbody.position = newPosition;
    }

    private void Explode()
    {
        AudioManager.Instance.Play("beaker");
        m_renderer.enabled = false;
        Light.color = ExplosionColor;
        m_exploded = true;
        m_explosionTimer = LightDuration;
    }

    private float m_initialHeight;
    private bool m_launched = false;
    private float m_launchedTimer = 0;
    private bool m_exploded = false;
    private Renderer m_renderer;
    private Rigidbody m_rigidbody;
    private float m_explosionTimer;
}
