using UnityEngine;
using System.Collections;

public class ShieldBehavior : WeaponBehavior
{
    public const float Lifetime = 12f;

    public float Speed = 20f;
    public bool Unlimited = false;

    public override void Initialize(PlayerController owner)
    {
        base.Initialize(owner);

        if (owner.KartState.AkuAkuEquiped != null)
        {
            owner.KartState.AkuAkuEquiped.SetLifetime();
            Destroy(gameObject);
            return;
        }

        transform.SetParent(owner.transform.GetChild(0));
        transform.localPosition = Vector3.zero;

        Owner.KartState.WeaponLocked = true;
        owner.KartState.ShieldBehavior = this;

        m_rigidbody = GetComponent<Rigidbody>();

        m_timer = Lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Owner.Controller.GetKeyDown("action"))
        {
            Owner.KartState.WeaponLocked = false;
            Owner.KartState.ShieldBehavior = null;
            transform.parent = null;
            transform.forward = Owner.transform.forward;
            m_rigidbody.position += transform.forward * 2;
            m_detached = true;
        }

        if (Unlimited == false)
        {
            m_timer -= Time.deltaTime;

            if (m_timer < 0)
            {
                Disappear();
            }
        }

        if (m_detached)
        {
            m_rigidbody.position += transform.forward * Time.deltaTime * Speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player != Owner)
            {
                player.Hit(Owner, name);
                Disappear();
            }
        }
        else if (m_detached == true)
        {
            Disappear();
        }
    }

    public void Disappear()
    {
        Owner.KartState.WeaponLocked = false;
        Owner.KartState.ShieldBehavior = null;
        Destroy(gameObject);
    }

    private Rigidbody m_rigidbody;
    private float m_timer;
    private bool m_detached = false;
}
