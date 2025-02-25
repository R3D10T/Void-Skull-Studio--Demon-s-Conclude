using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public Transform PlayerPos;
    public Transform EnemyPos;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;

        SetUpBattle();
        
    }

    private void SetUpBattle()
    {
        GameObject player = Instantiate(PlayerPrefab, PlayerPos);
        player.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        
        GameObject enemy = Instantiate(EnemyPrefab, EnemyPos);
        enemy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
    }
}
