using UnityEngine;
using System.Collections;

public class ShieldBehavior : WeaponBehavior
{

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Owner.Controller.GetKeyDown("action"))
        {
            Owner.KartState.WeaponLocked = false;
            Owner.KartState.ShieldBehavior = null;
            transform.parent = null;
            m_detached = true;
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

    private bool m_detached = false;
}
