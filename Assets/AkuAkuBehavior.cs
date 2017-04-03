using UnityEngine;

public class AkuAkuBehavior : WeaponBehavior
{
    public float RotationSpeed = 460f;

    public float BaseLifetime = 7f;

    public float AddingSpeed = 0.75f;

    public override void Initialize(bool backWard)
    {
        

        float duration = 0;

        if (Owner.KartState.AkuAkuEquiped != null)
        {
            duration = Owner.KartState.AkuAkuEquiped.SetLifetime();
            Owner.Boost(duration);
            Owner.KartState.SetInvincibility(duration, false);
            Destroy(gameObject);
            return;
        }

        transform.SetParent(Owner.transform.GetChild(0));
        transform.localPosition = Vector3.zero;

        Owner.KartState.AkuAkuEquiped = this;
        duration = SetLifetime();
        Owner.KartState.SetInvincibility(duration, true);

        Owner.Boost(duration);

        AudioManager.Instance.PlayOverrideMusic("akuaku");
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
            AudioManager.Instance.StopOverrideMusic("akuaku");
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
