using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatButtons : MonoBehaviour
{
    public StateMachine sm;
    public GameObject Attack;
    public GameObject Heal;
    public GameObject Level;
    public GameObject Loot;

    // Update is called once per frame
    void Update()
    {
        if(sm.state == BattleState.REST)
        {
            Level.SetActive(true);
            Loot.SetActive(true);
            Attack.SetActive(false);
            Heal.SetActive(false);
        }
        else
        {
            Level.SetActive(false);
            Loot.SetActive(false);
            Attack.SetActive(true);
            Heal.SetActive(true);
        }
    }
}
