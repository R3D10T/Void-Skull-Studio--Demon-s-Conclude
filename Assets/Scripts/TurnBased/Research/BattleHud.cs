using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TMP_Text nameText;
    public Slider HpSlider;

    public void SetHud(Unit Unit)
    {
        nameText.text = Unit.name;
        HpSlider.maxValue = Unit.MaxHP;
        HpSlider.value = Unit.CurHP;
    }

    public void SetHP(int Hp)
    {
        HpSlider.value = Hp;
    }
}
