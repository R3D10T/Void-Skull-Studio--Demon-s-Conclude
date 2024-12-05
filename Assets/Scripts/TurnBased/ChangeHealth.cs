using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class enemyChangeHealth : MonoBehaviour
{
    [SerializeField] private Slider Bar;
    public EnemyTemplate Health;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Bar.maxValue = Health.EnemyMaxHP;
        Bar.value = Health.EnemyCurHP;
        
    }
}
