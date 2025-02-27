using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackAbility : Ability
{
    public double Multiplyer;
    public bool Paralysis;
    public int Odds;
    public override int Activate(Unit Player, Unit Target)
    {
        int newATK = Player.ATK;
        //return base.Activate(Player); 
        if (Multiplyer > 0)
        {
            newATK = (int)(Player.ATK * Multiplyer);
        }

        if (Paralysis)
        {
            int RandHit = Random.Range(0, 100);

            if (RandHit < Odds)
            {
                Target.Paralysis = true;
            }
        }

        return newATK;
    }
}
