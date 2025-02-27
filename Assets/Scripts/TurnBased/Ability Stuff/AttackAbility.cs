using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackAbility : Ability
{
    public double Boost;
    public override int Activate(Unit Player)
    {
        //return base.Activate(Player); 

        int newATK = (int)(Player.ATK * Boost);

        return newATK;
    }
}
