using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string name;
    public int level;
    public int Attack;
    public int maxHP;
    public int currentHP;
    public int crit;
    public int potions;
    public bool TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        
        
    }
}
