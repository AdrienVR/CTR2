using UnityEngine;

public class AkuAkuBehavior : WeaponBehavior
{
    public float RotationSpeed = 460f;

    public float BaseLifetime = 7f;

    public float AddingSpeed = 0.75f;

    public override void Initialize(PlayerController owner)
    {
        base.Initialize(owner);

        float duration = 0;

        if (owner.KartState.AkuAkuEquiped != null)
        {
            duration = owner.KartState.AkuAkuEquiped.SetLifetime();
            owner.Boost(duration);
            owner.KartState.SetInvincibility(duration, false);
            Destroy(gameObject);
            return;
        }

        transform.SetParent(owner.transform.GetChild(0));
        transform.localPosition = Vector3.zero;

        owner.KartState.AkuAkuEquiped = this;
        duration = SetLifetime();
        owner.KartState.SetInvincibility(duration, true);

        owner.Boost(duration);

        AudioManager.Instance.Play("akuaku", true);
    }

    public float SetLifetime()
    {
        if (Owner.IsSuper())
        {
            m_lifetime += BaseLifetime * 2;
            return BaseLifetime * 2;
        }
        else
        {
            m_lifetime += BaseLifetime;
            return BaseLifetime;
        }
    }

    void Update()
    {
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0)
        {
            AudioManager.Instance.PlayDefaultMapMusic();
            Owner.SpeedCoefficient -= AddingSpeed;
            Owner.KartState.AkuAkuEquiped = null;
            Destroy(gameObject);
            return;
        }

        m_angle += RotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(Vector3.up * m_angle);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player != Owner)
            {
                player.Hit(Owner, name);
            }
        }
    }

    private float m_angle;
    private float m_lifetime = 0f;
}
