using UnityEngine;

public class AkuAkuBehavior : WeaponBehavior
{
    public float RotationSpeed = 460f;

    public float BaseLifetime = 7f;

    public override void Initialize(PlayerController owner)
    {
        base.Initialize(owner);

        if (owner.KartState.AkuAkuEquiped != null)
        {
            owner.KartState.AkuAkuEquiped.SetLifetime();
            Destroy(gameObject);
            return;
        }

        transform.SetParent(owner.transform);
        transform.localPosition = Vector3.zero;

        owner.KartState.AkuAkuEquiped = this;
        SetLifetime();

        AudioManager.Instance.Play("akuaku", true);
    }

    public void SetLifetime()
    {
        if (Owner.IsSuper())
        {
            m_lifetime += BaseLifetime * 2;
        }
        else
        {
            m_lifetime += BaseLifetime;
        }
    }

    void Update()
    {
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0)
        {
            AudioManager.Instance.PlayDefaultMapMusic();
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
                player.Die(Owner, name);
            }
        }
    }

    private float m_angle;
    private float m_lifetime = 0f;
}
