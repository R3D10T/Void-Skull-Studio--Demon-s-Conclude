using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : ScriptableObject
{
    public string AbilityName;
    public string description;
    public bool Obtained;

    public virtual int Activate(Unit Player, Unit Target)
    {
        return 0;
    }
}
