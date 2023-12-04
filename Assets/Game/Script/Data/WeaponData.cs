using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Hammer = 0,
    Boomerang = 1,
    Axe = 2,
    Knife = 3,
    Arrow = 4,
}

[Serializable]
public class WeaponData
{
    public WeaponType weaponType;
    public Weapon weapon;
    public Bullet bullet;
    public float autoAttackRange;
    public float attackSpeed;
    public int price;
}
