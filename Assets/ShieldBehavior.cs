using UnityEngine;
using System.Collections;

public class ShieldBehavior : WeaponBehavior
{
    public const float Lifetime = 12f;

    public float Speed = 20f;
    public bool Unlimited = false;

    public override void Initialize(bool backWard)
    {
        

        if (Owner.KartState.AkuAkuEquiped != null)
        {
            Owner.KartState.AkuAkuEquiped.SetLifetime();
            Destroy(gameObject);
            return;
        }

        transform.SetParent(Owner.transform.GetChild(0));
        transform.localPosition = Vector3.zero;

        Owner.KartState.UsingWeapon = this;
        Owner.KartState.TempBuffs["SingleHitProtection"] = 1;

        m_timer = Lifetime;
    }

    public override void Activate()
    {
        Owner.KartState.UsingWeapon = null;
        transform.parent = null;
        transform.forward = Owner.transform.forward;
        transform.position += transform.forward * 2 + Vector3.up * 1.55f;
        m_detached = true;
    }

    public override void OnHit()
    {
        Disappear();
    }

    // Update is called once per frame
    void Update()
    {
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
            transform.position += transform.forward * Time.deltaTime * Speed;

            RaycastHit hitGround;
            if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hitGround, 2.55f, Consts.LayerMaskInt.Ground) == false)
            {
                if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hitGround, 200, Consts.LayerMaskInt.Ground))
                {
                    transform.position = Vector3.Lerp(transform.position, hitGround.point + Vector3.up * 1.55f, Time.deltaTime * 5);
                }
            }
            else
            {
                transform.position = hitGround.point + Vector3.up * 1.55f;
            }
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
        Owner.KartState.UsingWeapon = null;
        Destroy(gameObject);
    }
    
    private float m_timer;
    private bool m_detached = false;
}
