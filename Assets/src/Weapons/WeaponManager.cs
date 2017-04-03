using UnityEngine;
using System.Collections;
using System;

public class WeaponManager : MonoBehaviour
{
    // Singleton
    public static WeaponManager Instance;

    public ActivableWeapon[] BattleWeapons;
    public ActivableWeapon[] RaceWeapons;

    void Awake()
    {
        if (!enabled)
            return;
        Instance = this;
    }

    public void SetBattleWeaponActivation(string name, bool active = true)
    {
        SetWeaponActivation(BattleWeapons, name, active);
    }

    public ActivableWeapon GetRandomBattleWeapon()
    {
        int index = UnityEngine.Random.Range(0, BattleWeapons.Length);
        while (index == lastRandomIndex || BattleWeapons[index].Active == false)
            index = UnityEngine.Random.Range(0, BattleWeapons.Length);

        lastRandomIndex = index;

        return BattleWeapons[index];
    }

    public void SetRaceWeaponActivation(string name, bool active = true)
    {
        SetWeaponActivation(RaceWeapons, name, active);
    }

    private void SetWeaponActivation(ActivableWeapon[] weapons, string name, bool active = true)
    {
        foreach (ActivableWeapon weapon in weapons)
        {
            if (weapon.Name == name)
            {
                weapon.Active = active;
            }
        }
    }

    private int lastRandomIndex;
}
