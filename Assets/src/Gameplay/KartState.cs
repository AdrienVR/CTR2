using UnityEngine;

public class KartState
{
    public bool armed;
    public bool waiting;
    public bool armedEvolute;

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

    public void SetInvincibility(float duration)
    {
        m_invincibilityTimer = duration;
    }

    private float m_shootUnabilityTimer;
    private float m_moveUnabilityTimer;
    private float m_invincibilityTimer;
}
