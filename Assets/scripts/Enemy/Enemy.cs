using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private EnemyManager enemyManager;
    public GameObject onHitEffect;
    public GameObject onDeathDrop;
    public bool respawnable;
    public GameObject enemyPrefab;
    public string enemyTypeS;
    private int enemyTypeI;
    public Sprite[] sprites;

    private RoomManager roomMan;
    private int dropAmount;
    private float enemyHealth;

    private Animator spriteAnim;

    // Start is called before the first frame update
    void Awake()
    {
        enemyHealth = maxHealth;
        spriteAnim = GetComponentInChildren<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.AddLiveEnemy(this);//adds enemy to world list when spawned
        roomMan = FindObjectOfType<RoomManager>();
        enemyTypeI = Array.IndexOf(roomMan.GetRoomList(), enemyTypeS);
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
            enemyManager.RemoveLiveEnemy(this);
            //respawns enemy if allowed
            if(respawnable){
                Respawn();
            }
            Destroy(gameObject);
            enemyManager.CheckEndWave();
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

    //respawn itself MUST BE CALLED BEFORE DESTROY
    private void Respawn()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    public void BIGUPDAHOLEISLAND()//REMOVE:
    {
        Debug.Log("BIG UP DA HOLE ISLAND");
    }
}
