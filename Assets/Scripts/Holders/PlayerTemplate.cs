using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerTemplate : ScriptableObject
{
    public int MaxHP;
    public int CurHP;
    public int Atk;
    public int Def;
    public int Level;
    public int Exp;
    public int ExpToNext;
    public Vector2Int gridCoords;
    
}
