using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public Transform PlayerPos;
    public Transform EnemyPos;

    

    public TMP_Text DialougeText;

    Unit PlayerU;
    Unit EnemyU;

    public AttackAbility[] AbilityList;
    public GameObject[] AbilityButList;
    public GameObject AbilityDescription;
    public TMP_Text AbilityDescriptionTxt;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;

        AbilityDescriptionTxt = AbilityDescription.GetComponentInChildren<TMP_Text>();
        AbilityDescriptionTxt.text = AbilityList[0].description;
        AbilityDescription = Instantiate(AbilityDescription, AbilityButList[0].transform);

        StartCoroutine(SetUpBattle());
        
    }

    private void Update()
    {
      
    }

    private IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(PlayerPrefab, PlayerPos);
        playerGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        PlayerU = playerGO.GetComponent<Unit>();
        
        GameObject enemyGO = Instantiate(EnemyPrefab, EnemyPos);
        enemyGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        EnemyU = enemyGO.GetComponent<Unit>();

        EnemyU.Alive = true;
        EnemyU.CurHP = EnemyU.MaxHP;

        DialougeText.text = "A " + EnemyU.name + " appears.";

        playerHUD.SetHud(PlayerU);
        enemyHUD.SetHud(EnemyU);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack(int damage)
    {
        EnemyU.TakeDamage(PlayerU, damage);

        enemyHUD.SetHP(EnemyU.CurHP);
        playerHUD.SetHP(PlayerU.CurHP);


        DialougeText.text = PlayerU.name + " hit " + EnemyU.name + " for " + damage + " damage";


        if (EnemyU.Alive)
        {
            state = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(3);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = BattleState.WON;
            yield return new WaitForSeconds(3);
            EndBattle();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            DialougeText.text = "You Won!";
        }

        if (state == BattleState.LOST)
        {
            DialougeText.text = "You Lost..";
        }
    }

    IEnumerator EnemyTurn()
    {
        if (state == BattleState.ENEMYTURN)
        {
            int DMG = EnemyU.ATK - PlayerU.DEF;
            if (DMG < 0)
            {
                DMG = 0;
            }

            PlayerU.TakeDamage(EnemyU, DMG);

            enemyHUD.SetHP(EnemyU.CurHP);
            playerHUD.SetHP(PlayerU.CurHP);

            DialougeText.text = EnemyU.name + " hit " + PlayerU.name + " for " + DMG + " damage";

            yield return new WaitForSeconds(3);

            if (PlayerU.Alive)
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
            else
            {
                state = BattleState.LOST;
                EndBattle();
            }
        }
    }

    void PlayerTurn()
    {
        DialougeText.text = "Choose an action";
    }

    public void OnAttackbutton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        int damage = PlayerU.ATK - EnemyU.DEF;
        if (damage < 0)
        {
            damage = 0;
        }

        StartCoroutine(PlayerAttack(damage));
    }

    public void OnAbilityButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        for (int i = 0; i < AbilityList.Length; i++)
        {
            if (AbilityList[i].Obtained)
            {
                AbilityButList[i].SetActive(true);
            }
        }
    }

    public void OnScratchButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        if (AbilityList[0].Obtained)
        {
            int Attack = AbilityList[0].Activate(PlayerU);

            int damage = Attack - EnemyU.DEF;
            if (damage < 0)
            {
                damage = 0;
            }

            StartCoroutine(PlayerAttack(damage));
        }
    }
}
