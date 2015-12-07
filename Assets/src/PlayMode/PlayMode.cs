
using System;

[Serializable]
public abstract class PlayMode
{
    public PlayerManager PlayerManager;

    public int MaxScore = 8;

    public virtual void UpdateDeath(int deathPlayerIndex, int killerPlayerIndex)
    {

    }
}
