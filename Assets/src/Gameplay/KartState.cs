using System.Collections.Generic;
using UnityEngine;

public class KartState
{
    public Dictionary<string, float> TempBuffs = new Dictionary<string, float>(8)
    {
        { "SingleHitProtection", 0 }
    };
    public WeaponBehavior UsingWeapon;
    public bool IsArmed;
    public bool waiting;
    public AkuAkuBehavior AkuAkuEquiped;
	public InvisibilityBehavior InvisibilityEquiped;

    public void Update()
    {
        if (m_shootUnabilityTimer > 0)
            m_shootUnabilityTimer -= Time.deltaTime;
        if (m_moveUnabilityTimer > 0)
            m_moveUnabilityTimer -= Time.deltaTime;
        if (m_invincibilityTimer > 0)
            m_invincibilityTimer -= Time.deltaTime;
    }

    public bool AbleToShoot()
    {
        if (m_shootUnabilityTimer > 0)
            return false;
        return true;
    }

    public bool CanMove()
    {
        if (m_moveUnabilityTimer > 0)
            return false;
        return true;
    }

    public bool IsInvincible()
    {
        if (m_invincibilityTimer > 0)
            return true;
        return false;
    }

    public void SetUnabilityToShoot(float duration)
    {
        m_shootUnabilityTimer = duration;
    }

    public void SetUnabilityToMove(float duration)
    {
        m_moveUnabilityTimer = duration;
    }

    public void SetInvincibility(float duration, bool reset = false)
    {
        if (reset)
        {
            m_invincibilityTimer = duration;
        }
        else
        {
            m_invincibilityTimer += duration;
        }
    }

    private float m_shootUnabilityTimer;
    private float m_moveUnabilityTimer;
    private float m_invincibilityTimer;
}
