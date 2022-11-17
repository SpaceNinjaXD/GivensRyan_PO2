using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, REST, ENEMYSPAWN, WIN, LOST }
public class StateMachine : MonoBehaviour
{

    public Text currentState;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private GameObject currentEnemy;
    public Transform playerSpawn;
    public Transform enemySpawn;
    Unit playerUnit;
    Unit enemyUnit;
    public int waveNumber;
    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public BattleState state;
    public int difficultyLevel = 1;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    public void OnLootButton()
    {
        if (state != BattleState.REST)
            return;
        int pots = Random.Range(1, 4);
        playerUnit.potions += pots;
        playerHUD.SetPotions(playerUnit.potions);
        dialogueText.text = "You found " + pots + "potions.";

        StartCoroutine(EnemySpawn());
    }

    public void OnLevelButton()
    {
        if (state != BattleState.REST)
            return;
        dialogueText.text = "You Leveled up!!!";

        playerUnit.level += 1;
        playerUnit.Attack += Random.Range(1, 4);
        playerUnit.maxHP += 10;
        playerUnit.currentHP += 10;
        playerHUD.SetHUD(playerUnit);
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(1f);
        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawn);
        currentEnemy = enemyGO;
        enemyUnit = enemyGO.GetComponent<Unit>();
        state = BattleState.PLAYERTURN;
        enemyUnit.level += difficultyLevel;
        enemyUnit.Attack += difficultyLevel * 2;
        enemyUnit.maxHP += difficultyLevel * 7;
        enemyUnit.currentHP = enemyUnit.maxHP;

        dialogueText.text = "Another " + enemyUnit.name + " replaces the last one...";
        difficultyLevel += 1;
        enemyHUD.SetHUD(enemyUnit);
        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator RestTime()
    {
        dialogueText.text = "Pick a option";
        

        yield return new WaitForSeconds(2f);

    }



    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<Unit>();
        playerHUD.SetPotions(playerUnit.potions);

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawn);
        currentEnemy = enemyGO;
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A " + enemyUnit.name + " appears out of nowhere...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {

        int var = Random.RandomRange(-1, 3);
        bool isDead = enemyUnit.TakeDamage(playerUnit.Attack+var);

        enemyHUD.SetHP(enemyUnit.currentHP);
        var += playerUnit.Attack;
        dialogueText.text = "You hit for " + var + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            if(waveNumber <= 0)
            {
                state = BattleState.WIN;
                EndBattle();
            }
            else
            {
                waveNumber -= 1;
                
                Destroy(currentEnemy);
                state = BattleState.REST;
                StartCoroutine(RestTime());

            }
            
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        if(enemyUnit.currentHP <= enemyUnit.maxHP / 3 && enemyUnit.potions > 0)
        {
            dialogueText.text = enemyUnit.name + " drinks a health potion!";

            yield return new WaitForSeconds(1f);

            enemyUnit.currentHP += 9 * difficultyLevel;
            enemyUnit.potions -= 1;
            enemyHUD.SetHP(enemyUnit.currentHP);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            dialogueText.text = enemyUnit.name + " attacks!";

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.Attack);

            playerHUD.SetHP(playerUnit.currentHP);
            dialogueText.text = enemyUnit.name + " hits you for " + enemyUnit.Attack + " damage!";
            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        

    }

    IEnumerator MainMenu()
    {

        yield return new WaitForSeconds(5f);
        Application.LoadLevel(0);
    }
    void EndBattle()
    {
        if (state == BattleState.WIN)
        {
            dialogueText.text = "You Win!";
            
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You Lose...";
        }
        StartCoroutine(MainMenu());
    }

    void PlayerTurn()
    {
        dialogueText.text = "Pick your action:";
    }

    IEnumerator PlayerHeal()
    {
        if(playerUnit.potions > 0)
        {
            playerUnit.Heal(25);

            playerHUD.SetHP(playerUnit.currentHP);
            dialogueText.text = "You have consumed a health potion!";
            playerUnit.potions -= 1;
            playerHUD.SetPotions(playerUnit.potions);

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            dialogueText.text = "You are out of health potions.";
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();


        }

        
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    private void Update()
    {
        currentState.text = "Current State: " + state.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(0);
        }
    }
}
