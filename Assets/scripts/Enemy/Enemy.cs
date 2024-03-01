using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyManager enemyManager;
    public GameObject onHitEffect;

    private float enemyHealth = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checks if enemy has any health left
        if(enemyHealth <= 0)
        {
            //destroys object and removes enemy from list if enemy dies
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }

    //applies damage to enemies
    public void TakeDamage(float damage)
    {
        Instantiate(onHitEffect, transform.position, Quaternion.identity);
        enemyHealth -= damage;
    }
}
