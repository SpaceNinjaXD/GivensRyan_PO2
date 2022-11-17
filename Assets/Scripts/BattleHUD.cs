using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public Text pots;
    public void SetHUD(Unit unit)
    {
        nameText.text = unit.name;
        levelText.text = "Lvl " + unit.level;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }
    public void SetPotions(int amount)
    {
        pots.text = "Potions: " + amount;
    }
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
