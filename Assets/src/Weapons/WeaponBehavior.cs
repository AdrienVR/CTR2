using System;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    [HideInInspector]
    public PlayerController Owner;

    public abstract void Initialize(bool backWard);

    public virtual void Activate() { }

    public virtual void OnHit() { }
}