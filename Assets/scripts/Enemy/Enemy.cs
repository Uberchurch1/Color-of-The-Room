using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private EnemyManager enemyManager;
    public GameObject onHitEffect;
    public GameObject onDeathDrop;

    private int dropAmount;
    private float enemyHealth;

    private Animator spriteAnim;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = maxHealth;
        spriteAnim = GetComponentInChildren<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks if enemy has any health left
        if(enemyHealth <= 0)
        {
            //calculates spore drop amount
            dropAmount = Random.Range(0,5);
            //destroys object and removes enemy from list if enemy dies
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
            //drops a random amount of spores 0-5 set from line 18
            if(dropAmount != 0){
                onDeathDrop.GetComponent<ItemPickup>().amount = dropAmount;
                Instantiate(onDeathDrop, transform.position, transform.rotation);
            }
        }
    }

    //applies damage to enemies
    public void TakeDamage(float damage)
    {
        //spawns blood particles based on amount of damage taken
        for(int i = 0; i <= damage; i += 15){
        Instantiate(onHitEffect, transform.position, transform.rotation);
        }
        enemyHealth -= damage;
    }
}
