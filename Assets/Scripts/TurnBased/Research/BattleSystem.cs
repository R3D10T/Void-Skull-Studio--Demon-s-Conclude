using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

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

    public GameObject ContinueButton;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public AttackAbility[] AbilityList;
    public GameObject[] AbilityButList;

    public GameObject AbilityDescription;
    public TMP_Text AbilityDescriptionTxt;

    public int TimeBreak;

    public BattleState state;

    [Header("Audio Clips")]
    public AudioClip scratchSound;
    public AudioClip sweepSound;
    public AudioClip basicAttackSound;
    public AudioClip enemyAttackSound;
    public AudioClip enemyChargeUp;
    public AudioClip guardSound;
    public AudioClip victorySound;
    public AudioClip defeatSound;

    private AudioSource audioSource;

    void Start()
    {
        state = BattleState.START;

        audioSource = GetComponent<AudioSource>();

        AbilityDescriptionTxt = AbilityDescription.GetComponentInChildren<TMP_Text>();
        AbilityDescriptionTxt.text = AbilityList[0].description;
        AbilityDescription = Instantiate(AbilityDescription, AbilityButList[0].transform);

        StartCoroutine(SetUpBattle());
    }

    private IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(PlayerPrefab, canvas.transform);
        playerGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        GameObject enemyGO = Instantiate(EnemyPrefab, EnemyPos);
        enemyGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        EnemyU.Alive = true;
        EnemyU.CurHP = EnemyU.MaxHP;

        PlayerU.Alive = true;
        PlayerU.CurHP = PlayerU.MaxHP;

        ContinueButton.SetActive(false);

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
            yield return new WaitForSeconds(TimeBreak);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            yield return new WaitForSeconds(TimeBreak);
            PlayerTurn();
        }
    }

    IEnumerator PlayerAttack(int damage)
    {
        PlaySound(basicAttackSound);

        int DMG = EnemyU.TakeDamage(PlayerU, damage);

        enemyHUD.SetHP(EnemyU.CurHP);
        playerHUD.SetHP(PlayerU.CurHP);

        DialougeText.text = PlayerU.name + " hit " + EnemyU.name + " for " + DMG + " damage";

        if (EnemyU.Alive)
        {
            state = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(TimeBreak);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = BattleState.WON;
            yield return new WaitForSeconds(TimeBreak);
            EndBattle();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            PlaySound(victorySound);
            DialougeText.text = "You Won!";
            WinScreen.SetActive(true);
        }

        if (state == BattleState.LOST)
        {
            PlaySound(defeatSound);
            DialougeText.text = "You Lost..";
            LoseScreen.SetActive(true);
        }
    }

    public void ContinuePressed()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
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
                int RandAct = Random.Range(0, 4);
                if (EnemyU.Charged == true)
                {
                    RandAct = 0;
                }

                if (RandAct == 0 || RandAct == 1)
                {
                    int DMG = 0;
                    if (EnemyU.Charged)
                    {
                        DMG = (EnemyU.ATK * 3) - PlayerU.DEF;
                        EnemyU.Charged = false;
                    }
                    else
                    {
                        DMG = EnemyU.ATK - PlayerU.DEF;
                    }

                    if (DMG < 0)
                        DMG = 0;

                    PlaySound(enemyAttackSound);

                    int damage = PlayerU.TakeDamage(EnemyU, DMG);

                    enemyHUD.SetHP(EnemyU.CurHP);
                    playerHUD.SetHP(PlayerU.CurHP);

                    DialougeText.text = EnemyU.name + " hit " + PlayerU.name + " for " + damage + " damage";
                }

                if (RandAct == 2)
                {
                    EnemyU.Guarding = true;
                    PlaySound(guardSound);
                    DialougeText.text = EnemyU.name + " guarded";
                }

                if (RandAct == 3)
                {
                    EnemyU.Charged = true;
                    PlaySound(enemyChargeUp);
                    DialougeText.text = EnemyU.name + " is charging up";
                }
            }
            else
            {
                DialougeText.text = EnemyU.name + " is paralysed";
            }

            yield return new WaitForSeconds(TimeBreak);

            EnemyU.Paralysis = false;

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
        for (int i = 0; i < AbilityList.Length; i++)
        {
            AbilityList[i].LowerCooldown();
        }

        DialougeText.text = "Choose an action";
        PlayerU.Guarding = false;
    }

    public void OnAttackbutton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        int damage = PlayerU.ATK - EnemyU.DEF;
        if (damage < 0)
            damage = 0;

        StartCoroutine(PlayerAttack(damage));
    }

    public void OnAbilityButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

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
            return;

        if (AbilityList[0].ActiveCooldown > 0)
        {
            DialougeText.text = "Ability on cooldown";
            return;
        }

        if (AbilityList[0].Obtained)
        {
            PlaySound(scratchSound);

            int Attack = AbilityList[0].Activate(PlayerU, EnemyU);
            int damage = Attack - EnemyU.DEF;
            if (damage < 0)
                damage = 0;

            AbilityList[0].startCooldown();

            StartCoroutine(PlayerAttack(damage));
        }
    }

    public void OnSweepButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (AbilityList[1].ActiveCooldown > 0)
        {
            DialougeText.text = "Ability on cooldown";
            return;
        }

        if (AbilityList[1].Obtained)
        {
            PlaySound(sweepSound);

            AbilityList[1].Activate(PlayerU, EnemyU);
            DialougeText.text = "You swept the " + EnemyU.name + " off its feet";

            AbilityList[1].startCooldown();

            StartCoroutine(EndTurn(PlayerU));
        }
    }

    public void OnGuardButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        PlaySound(guardSound);

        PlayerU.Guarding = true;
        DialougeText.text = "You Guarded";

        StartCoroutine(EndTurn(PlayerU));
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
