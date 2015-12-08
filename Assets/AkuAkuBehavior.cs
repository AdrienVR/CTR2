using UnityEngine;
using System.Collections;

public class AkuAkuBehavior : WeaponBehavior
{
    public float RotationSpeed = 460f;

    public override void Initialize(PlayerController owner)
    {
        transform.SetParent(owner.transform);
        transform.localPosition = Vector3.zero;

        base.Initialize(owner);
    }

    void Update()
    {
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
}
