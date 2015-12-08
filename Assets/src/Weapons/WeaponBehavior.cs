using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [HideInInspector]
    public PlayerController Owner;

    public virtual void Initialize(PlayerController owner)
    {
        Owner = owner;
    }
}