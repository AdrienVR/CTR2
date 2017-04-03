using UnityEngine;
using System.Collections;

public class AcceleratorBehavior : WeaponBehavior
{
    public float AddingSpeed = 0.75f;

    public float Duration = 2.5f;

    public override void Initialize(bool backWard)
    {
        if (Owner.IsSuper())
        {
            SetBoost(Duration);
        }
        else
        {
            SetBoost(Duration);
        }
    }

    public void SetBoost(float duration)
    {
        m_timer = duration;

        Owner.SpeedCoefficient += AddingSpeed;
        Owner.PlayBoostAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer < 0)
        {
            EndAcceleration();
        }
    }

    public void EndAcceleration()
    {
        Owner.SpeedCoefficient -= AddingSpeed;
        Destroy(gameObject);
    }

    private float m_timer;
}
