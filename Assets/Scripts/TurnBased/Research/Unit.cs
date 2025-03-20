using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Unit : ScriptableObject
{
    public string name;

    public int ATK;
    public int MaxHP;
    public int CurHP;
    public int DEF;
    public bool Alive;
    public bool Guarding;
    public bool Paralysis;
    public bool Charged;

    public int TakeDamage(Unit Attacker, int damage)
    {
        if (Guarding)
        {
            damage = (int)(damage / 4); 
        }

        CurHP -= damage;

        if (CurHP < 0)
        {
            CurHP = 0;
        }
        
        if (CurHP == 0) 
        {
            Alive = false;
        }
        return damage;
    }
}
