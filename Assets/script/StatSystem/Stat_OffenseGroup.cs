using UnityEngine;
using System;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;

    //物理
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    //元素
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

}
