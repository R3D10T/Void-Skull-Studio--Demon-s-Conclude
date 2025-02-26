using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string name;

    public int ATK;
    public int MaxHP;
    public int CurHP;
    public int DEF;
    public bool Alive;

    public void TakeDamage(Unit Attacker, int damage)
    {
        CurHP -= damage;
        
        if (CurHP == 0) 
        {
            Alive = false;
        }
    }
}
