using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LP.TurnBased
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject[] EnemySpot;

        [SerializeField] private Slider Phealth = null;
        [SerializeField] private Slider Ehealth = null;

        [SerializeField] private Button AttackBut = null;
        [SerializeField] private Button HealBut = null;

        [SerializeField] private GameObject LoseScreen = null;
        [SerializeField] private GameObject WinScreen = null;
        [SerializeField] private GameObject BattleTab = null;


        public PlayerTemplate PlayerStat;
        public EnemyTemplate[] Enemies;

        public EnemyTemplate[] EnemyTurns;

        EnemyTemplate EnemyStat;

        bool Playerturn = true;

        void Start()
        {
            /*
            //New Multiple Enemies Stuff
            int RandTurns = 0;
            RandTurns = Random.Range(0, 2);

            for (int i = 0; i <= RandTurns; i++ )
            {
                int RandEnem = 0;
                RandEnem = Random.Range(0, Enemies.Length);

                EnemyTurns[i] = Enemies[RandEnem];
            }

            for (int j = 0; j <= EnemyTurns.Length; j++)
            {
                spawn(EnemyTurns[j].Model, EnemySpot[j]);

                EnemyTurns[j].ExpGiven = false;
                EnemyTurns[j].EnemyCurHP = EnemyTurns[j].EnemyMaxHP;
            }
            */

            for (int i = 0; i < Enemies.Length; i++)
            {
                spawn(Enemies[i].Model, EnemySpot[i]);
            }

            //Old Single Enemy Stuff
            int RandEnemy = 0;
            RandEnemy = Random.Range(0 , Enemies.Length);

            EnemyStat = Enemies[RandEnemy];

            //spawn(EnemyStat.Model, EnemySpot[0]);

            EnemyStat.ExpGiven = false;

            EnemyStat.EnemyCurHP = EnemyStat.EnemyMaxHP;

            Phealth.maxValue = PlayerStat.MaxHP;
            Ehealth.maxValue = EnemyStat.EnemyMaxHP;

            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            BattleTab.SetActive(true);
        }

        private void Update()
        {

            Phealth.value = PlayerStat.CurHP;
            Ehealth.value = EnemyStat.EnemyCurHP;

            if (EnemyStat.EnemyCurHP <= 0)
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

            if (!Playerturn) 
            {
                AttackBut.interactable = false;
                HealBut.interactable = false;

                StartCoroutine(EnemyTurn());
            }

            if (Playerturn) 
            {
                AttackBut.interactable = true;
                HealBut.interactable= true;
            }
        }

        private IEnumerator EnemyTurn()
        {
            if (EnemyStat.EnemyCurHP > 0)
            {
                yield return new WaitForSeconds(3);

                int RandomAct = 0;
                RandomAct = Random.Range(0, 2);   //2 is exclueded so this is 0-3

                if (RandomAct == 0)
                {
                    Heal(false, 10);
                }
                else
                {
                    Attack(true, EnemyStat.EnemyAtk, PlayerStat.Def);
                }
            }
            
        }


        private void Attack(bool player, int Attack, int Defence)
        {
            int damage = Attack - Defence;
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

        private void Heal(bool player, int Healed)
        {
            if (player  & PlayerStat.CurHP < PlayerStat.MaxHP)
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
            Attack(false, PlayerStat.Atk, EnemyStat.EnemyDef);
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

            if (!EnemyStat.ExpGiven)
            {
                PlayerStat.Exp += EnemyStat.Exp;
                EnemyStat.ExpGiven = true;
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
    }
}