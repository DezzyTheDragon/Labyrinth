using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : ItemBase
{
    public string weaponName;
    void Start()
    {
        objectName = weaponName;
        objectTag = ObjectTags.Weapon;
    }

}
