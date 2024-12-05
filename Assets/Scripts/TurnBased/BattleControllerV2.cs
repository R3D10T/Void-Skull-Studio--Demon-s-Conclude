using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace LP.TurnBased
{
    public class BattleControllerV2 : MonoBehaviour
    {
        [SerializeField] private GameObject[] EnemySpot;

        [SerializeField] private Slider[] EnemyHBar;
        [SerializeField] private Slider Phealth = null;

        [SerializeField] private Button AttackBut = null;
        [SerializeField] private Button HealBut = null;

        [SerializeField] private GameObject LoseScreen = null;
        [SerializeField] private GameObject WinScreen = null;
        [SerializeField] private GameObject BattleTab = null;


        public PlayerTemplate PlayerStat;
        public EnemyTemplate[] Enemies;


        List<EnemyTemplate> EnemiesList = new List<EnemyTemplate>();

        bool Playerturn = true;

        void Start()
        {
            /*
            for (int k = 0; k < EnemyHBar.Length; k++)
            {
               GameObject HPBAR = EnemyHBar[k].GetComponent<GameObject>();
                HPBAR.SetActive(false);
            }
            */

            //New Multiple Enemies Stuff
            int RandTurns = 1;
            //RandTurns = Random.Range(0, 2);

            for (int i = 0; i < RandTurns + 1; i++ )
            {
                int RandEnem = 0;
                RandEnem = Random.Range(0, Enemies.Length);

                EnemiesList.Add(Enemies[RandEnem]);
            }

            for (int j = 0; j < EnemiesList.Capacity; j++)
            {
                spawn(EnemiesList[j].Model, EnemySpot[j]);

                EnemiesList[j].ExpGiven = false;
                EnemiesList[j].EnemyCurHP = EnemiesList[j].EnemyMaxHP;

                //GameObject HPBAR = EnemyHBar[j].GetComponent<GameObject>();
                //HPBAR.SetActive(true);

                EnemyHBar[j].maxValue = EnemiesList[j].EnemyMaxHP;

            }

            Phealth.maxValue = PlayerStat.MaxHP;
            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            BattleTab.SetActive(true);
        }

        /*
        private void Update()
        {
            for (int i = 0;i < EnemiesList.Capacity;i++)
            {
                EnemyHBar[i].value = EnemiesList[i].EnemyCurHP;
            }

            Phealth.value = PlayerStat.CurHP;

            int enemyCount = EnemiesList.Capacity - 1;
            
            if (EnemiesList[0].EnemyCurHP >= 0)
            {
                Win();
            }

            if (PlayerStat.MaxHP <= 0)
            {
                Lose();
            }
        }
        */

        private void ChangeTurn()
        {
            HealthCheck();

            Playerturn = !Playerturn;

            if (!Playerturn)
            {
                AttackBut.interactable = false;
                HealBut.interactable = false;

                for (int i = 0; i < EnemiesList.Capacity; i++)
                {
                    StartCoroutine(EnemyTurn(EnemiesList[i]));
                }
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
                RandomAct = Random.Range(0, 2);   //2 is exclueded so this is 0-3

                if (RandomAct == 0)
                {
                    Heal(false, EnemyStat, 10);
                }
                else
                {
                    Attack(true, EnemyStat, EnemyStat.EnemyAtk, PlayerStat.Def);
                }
            }

        }


        private void Attack(bool player, EnemyTemplate EnemyStat, int attack, int defence)
        {
            int damage = attack - defence;
            if (damage < 0)
            {
                damage = 0;
            }

            if (!player & EnemyStat.EnemyCurHP > 0)
            {
                EnemyStat.EnemyCurHP -= damage;
                if (EnemyStat.EnemyCurHP < 0)
                {
                    EnemyStat.EnemyCurHP = 0;
                }

            }

            if (player & PlayerStat.CurHP > 0)
            {
                PlayerStat.CurHP -= damage;
                if (PlayerStat.CurHP < 0)
                {
                    PlayerStat.CurHP = 0;
                }
            }

            ChangeTurn();
        }

        private void Heal(bool player, EnemyTemplate EnemyStat, int Healed)
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

            ChangeTurn();
        }

        public void AttackButton()
        {
            EnemyTemplate target = null;

            if (EnemiesList[0].EnemyCurHP >= 0)
            {
                target = EnemiesList[1];
            }
            else
            {
                target = EnemiesList[0];
            }

            Attack(false, target, PlayerStat.Atk, target.EnemyDef);
        }

        public void HealButton()
        {
            Heal(true, EnemiesList[0], 15);
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

            for (int i = 0; i < EnemiesList.Capacity; i++)
            {
                if (!EnemiesList[i].ExpGiven)
                {
                    PlayerStat.Exp += EnemiesList[i].Exp;
                    EnemiesList[i].ExpGiven = true;
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

        public void HealthCheck()
        {
            for (int i = 0; i < EnemiesList.Capacity; i++)
            {
                EnemyHBar[i].value = EnemiesList[i].EnemyCurHP;
            }

            Phealth.value = PlayerStat.CurHP;

            int enemyCount = EnemiesList.Capacity - 1;

            if (EnemiesList[0].EnemyCurHP >= 0)
            {
                Win();
            }

            if (PlayerStat.MaxHP <= 0)
            {
                Lose();
            }
        }
    }
}