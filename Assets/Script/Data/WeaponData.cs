using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Axe = 0,
    Boomerang = 1,
    Knife = 2,
    Hammer = 3,
    Arrow = 4,
}

[Serializable]
public class WeaponData
{
    public WeaponType weaponType;
    public Weapon weapon;
    public Bullet bullet;
    public float autoAttackRange;

    // Clone the WeaponData
    public WeaponData Clone()
    {
        return new WeaponData
        {
            weaponType = this.weaponType,
            weapon = this.weapon,
            bullet = this.bullet,
            autoAttackRange = this.autoAttackRange
        };
    }
}
