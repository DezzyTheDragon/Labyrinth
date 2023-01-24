using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ItemBase
{
    protected int weaponID = 0;
    protected int weaponDamage = 0;
    protected float fireRate = 0f;

    public int GetWeaponID()
    {
        return weaponID;
    }

    public int GetWeaponDamage()
    {
        return weaponDamage;
    }

    public float GetWeaponFireRate()
    {
        return fireRate;
    }
}
