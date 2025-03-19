using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public Transform PlayerPos;
    public Transform EnemyPos;

    public Canvas canvas;

    public Unit PlayerU;
    public Unit EnemyU;

    public TMP_Text DialougeText;

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
        GameObject playerGO = Instantiate(PlayerPrefab, canvas.transform);
        playerGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        
        GameObject enemyGO = Instantiate(EnemyPrefab, EnemyPos);
        enemyGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        EnemyU.Alive = true;
        EnemyU.CurHP = EnemyU.MaxHP;

        DialougeText.text = "A " + EnemyU.name + " appears.";

        playerHUD.SetHud(PlayerU);
        enemyHUD.SetHud(EnemyU);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EndTurn(Unit Turn)
    {

        if (Turn == PlayerU)
        {
            state = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(3);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            yield return new WaitForSeconds(3);
            PlayerTurn();
        }
    }

    IEnumerator PlayerAttack(int damage)
    {
        int DMG = EnemyU.TakeDamage(PlayerU, damage);

        enemyHUD.SetHP(EnemyU.CurHP);
        playerHUD.SetHP(PlayerU.CurHP);


        DialougeText.text = PlayerU.name + " hit " + EnemyU.name + " for " + DMG + " damage";


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
            EnemyU.Guarding = false;

            for (int i = 0; i < AbilityList.Length; i++)
            {
                if (AbilityList[i].Obtained)
                {
                    AbilityButList[i].SetActive(false);
                }
            }

            if (!EnemyU.Paralysis)
            {
                int RandAct = Random.Range(0, 3);
                if (EnemyU.Charged == true) 
                {
                    RandAct = 0;
                }

                if (RandAct == 0 || RandAct == 1)
                {
                    int DMG = 0;
                    if (EnemyU.Charged == true)
                    {
                        DMG = (EnemyU.ATK * 3) - PlayerU.DEF;
                        EnemyU.Charged = false;
                    }
                    else
                    {
                        DMG = EnemyU.ATK - PlayerU.DEF;
                    }

                    if (DMG < 0)
                    {
                        DMG = 0;
                    }

                    int damage = PlayerU.TakeDamage(EnemyU, DMG);

                    enemyHUD.SetHP(EnemyU.CurHP);
                    playerHUD.SetHP(PlayerU.CurHP);

                    DialougeText.text = EnemyU.name + " hit " + PlayerU.name + " for " + damage + " damage";
                }

                if (RandAct == 2)
                {
                    EnemyU.Guarding = true;

                    DialougeText.text = EnemyU.name + " guarded";
                }

                if (RandAct == 3)
                {
                    EnemyU.Charged = true;

                    DialougeText.text = EnemyU.name + " is charging up";
                }
            }
            else
            {
                DialougeText.text = EnemyU.name + " is paralysed";
            }

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

        PlayerU.Guarding = false;
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
            int Attack = AbilityList[0].Activate(PlayerU, EnemyU);

            int damage = Attack - EnemyU.DEF;
            if (damage < 0)
            {
                damage = 0;
            }

            StartCoroutine(PlayerAttack(damage));
        }
    }

    public void OnSweepButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        if (AbilityList[1].Obtained)
        {
            AbilityList[1].Activate(PlayerU, EnemyU);

            DialougeText.text = "You swept the " + EnemyU.name + " off it's feet"; //NEEDS FAIL LINE

            StartCoroutine(EndTurn(PlayerU));  
        }
    }

    public void OnGuardButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        PlayerU.Guarding = true;

        DialougeText.text = "You Guarded";

        StartCoroutine(EndTurn(PlayerU));
    }
}
