using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyTemplate : ScriptableObject
{
    public string EnemyName;
    public int EnemyMaxHP;
    public int EnemyCurHP;
    public int EnemyAtk;
    public int EnemyDef;
    public int Exp;
    public bool ExpGiven;
    public bool Dead;
    public Material material;
    public GameObject Model;
}
