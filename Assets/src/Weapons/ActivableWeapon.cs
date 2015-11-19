using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class ActivableWeapon
{
    public string Name;
    public bool Active = true;
    public Sprite Sprite;
    public GameObject Weapon;
    public Sprite DarkSprite;
    public GameObject DarkWeapon;
    public Sprite SuperSprite;
    public GameObject SuperWeapon;
    public int Multiplicity = 1;
}