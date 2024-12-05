using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace LP.TurnBased
{
    public class TutorialBattle : MonoBehaviour
    {
        [SerializeField] private GameObject[] EnemySpot;

        [SerializeField] private Slider Phealth = null;
        //[SerializeField] private Slider Ehealth = null;

        [SerializeField] private Button AttackBut = null;
        [SerializeField] private Button HealBut = null;

        public GameObject[] TargetButtonObject;
        public Button[] TargetButton;

        [SerializeField] private GameObject LoseScreen = null;
        [SerializeField] private GameObject WinScreen = null;
        [SerializeField] private GameObject BattleTab = null;

        public GameObject[] healthBarObject;
        public Slider[] healthBar;

        public PlayerTemplate PlayerStat;
        public EnemyTemplate[] Enemies;

        public EnemyTemplate[] EnemyTurns;

        EnemyTemplate EnemyStat;

        bool Playerturn = true;

        void Start()
        {

            for (int k = 0; k < healthBarObject.Length; k++)
            {
                healthBarObject[k].SetActive(false);
                TargetButtonObject[k].SetActive(false);
            }

            for (int i = 0; i < Enemies.Length; i++)
            {
                spawn(Enemies[i].Model, EnemySpot[i]);
                Enemies[i].EnemyCurHP = Enemies[i].EnemyMaxHP;
                Enemies[i].Dead = false;
                Enemies[i].ExpGiven = false;
            }

            for (int j = 0; j < healthBar.Length; j++)
            {
                healthBar[j].maxValue = Enemies[j].EnemyMaxHP;
                healthBarObject[j].SetActive(true);
            }

            //Old Single Enemy Stuff
            int RandEnemy = 0;
            RandEnemy = UnityEngine.Random.Range(0, Enemies.Length);

            EnemyStat = Enemies[RandEnemy];

            //spawn(EnemyStat.Model, EnemySpot[0]);

            EnemyStat.ExpGiven = false;

            EnemyStat.EnemyCurHP = EnemyStat.EnemyMaxHP;

            Phealth.maxValue = PlayerStat.MaxHP;
            //Ehealth.maxValue = EnemyStat.EnemyMaxHP;

            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            BattleTab.SetActive(true);
        }

        private void Update()
        {
            for (int j = 0; j < healthBar.Length; j++)
            {
                healthBar[j].value = Enemies[j].EnemyCurHP;
            }
            Phealth.value = PlayerStat.CurHP;
            //Ehealth.value = EnemyStat.EnemyCurHP;

            for (int j = 0; j < Enemies.Length; j++)
            {
                if (Enemies[j].EnemyCurHP <= 0)
                {
                    Enemies[j].Dead = true;
                }
            }

            if (Enemies[0].Dead)
            {
                Win();
            }

            if (PlayerStat.MaxHP <= 0)
            {
                Lose();
            }
        }

        private void ChangeTurn()
        {
            Playerturn = !Playerturn;

            for (int i = 0; i < TargetButtonObject.Length; i++)
            {
                TargetButtonObject[i].SetActive(false);
            }

            if (!Playerturn)
            {
                AttackBut.interactable = false;
                HealBut.interactable = false;

                for (int j = 0; j < Enemies.Length; j++)
                {
                    if (!Enemies[j].Dead)
                    {
                        StartCoroutine(EnemyTurn(Enemies[j]));
                    }
                }

                ChangeTurn();
            }

            if (Playerturn)
            {
                AttackBut.interactable = true;
                HealBut.interactable = true;
            }
        }

        private IEnumerator EnemyTurn(EnemyTemplate EnemyStat)
        {
            if (EnemyStat.EnemyCurHP > 0)
            {
                yield return new WaitForSeconds(3);

                int RandomAct = 0;
                RandomAct = UnityEngine.Random.Range(0, 2);   //2 is exclueded so this is 0-1

                if (RandomAct == 10)
                {
                    Heal(false, 10);
                }
                else
                {
                    EnemyAttack(EnemyStat);
                }
            }

        }



        private void Heal(bool player, int Healed)
        {
            if (player & PlayerStat.CurHP < PlayerStat.MaxHP)
            {
                PlayerStat.CurHP += Healed;
                if (PlayerStat.CurHP > PlayerStat.MaxHP)
                {
                    PlayerStat.CurHP = PlayerStat.MaxHP;
                }
            }

            if (!player & EnemyStat.EnemyCurHP < EnemyStat.EnemyMaxHP)
            {
                EnemyStat.EnemyCurHP += Healed;
                if (EnemyStat.EnemyCurHP > EnemyStat.EnemyMaxHP)
                {
                    EnemyStat.EnemyCurHP = EnemyStat.EnemyMaxHP;
                }
            }

            //ChangeTurn();
        }

        public void AttackButton()
        {
            for (int i = 0; i < TargetButtonObject.Length; i++)
            {
                if (!Enemies[i].Dead)
                {
                    TargetButtonObject[i].SetActive(true);
                }
            }

            AttackBut.interactable = false;
            HealBut.interactable = false;

            //Attack(false, PlayerStat.Atk, EnemyStat.EnemyDef);
        }

        public void HealButton()
        {
            Heal(true, 15);
        }

        public void Lose()
        {
            BattleTab.SetActive(false);

            LoseScreen.SetActive(true);
        }

        public void Win()
        {
            BattleTab.SetActive(false);

            WinScreen.SetActive(true);

            for (int i = 0; i < Enemies.Length; i++)
            {
                if (!Enemies[i].ExpGiven)
                {
                    PlayerStat.Exp += Enemies[i].Exp;
                    Enemies[i].ExpGiven = true;
                }
            }

        }

        public void EndBattle()
        {
            SceneManager.LoadScene(0);
        }

        public void spawn(GameObject prefab, GameObject location)
        {
            prefab.transform.position = location.transform.position;
            GameObject newSpawn = Instantiate(prefab);
        }

        public void PlayerAttack(EnemyTemplate enemyStat)
        {
            int attack = PlayerStat.Atk - enemyStat.EnemyDef;
            if (attack < 0)
            {
                attack = 0;
            }

            enemyStat.EnemyCurHP -= attack;

            ChangeTurn();
        }

        public void EnemyAttack(EnemyTemplate enemyStat)
        {
            int attack = enemyStat.EnemyAtk - PlayerStat.Def;
            if (attack < 0)
            {
                attack = 0;
            }

            PlayerStat.CurHP -= attack;

        }
    }
}