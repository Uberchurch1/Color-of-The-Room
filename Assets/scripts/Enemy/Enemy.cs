using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private float enemyHealth;
    public float atkRange = 1.5f;
    public float enemyDmg = 5f;
    public float atkSpeed = 1f;
    public bool respawnable;
    public GameObject enemyPrefab;
    public GameObject onHitEffect;
    public GameObject onDeathDrop;
    public string enemyTypeS;
    private int enemyTypeI;

    private EnemyAttack _enemyAttack;
    private int dropAmount;
    private RoomManager roomMan;
    private EnemyManager enemyManager;
    private Collider enemyCollider;
    //private Animator spriteAnim; not used

    // Start is called before the first frame update
    void Awake()
    {
        _enemyAttack = GetComponentInChildren<EnemyAttack>();
        enemyHealth = maxHealth;
        //spriteAnim = GetComponentInChildren<Animator>(); not used
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.AddLiveEnemy(this);//adds enemy to world list when spawned
        roomMan = FindObjectOfType<RoomManager>();
        enemyTypeI = Array.IndexOf(roomMan.GetRoomList(), enemyTypeS);
        enemyCollider = GetComponent<CapsuleCollider>();
        Debug.Log("spawned type: "+enemyTypeI);//REMOVE:
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRoom())
        {
            enemyCollider.enabled = true;
        }
        else
        {
            enemyCollider.enabled = false;
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
        //checks if enemy has any health left
        if(enemyHealth <= 0)
        {
            Debug.Log(("health zero"));//REMOVE:
            //calculates spore drop amount
            dropAmount = Random.Range(0,5);
            //destroys object and removes enemy from list if enemy dies
            enemyManager.RemoveEnemy(this);
            enemyManager.RemoveLiveEnemy(this);
            //respawns enemy if allowed
            if(respawnable){
                Respawn();
            }
            Debug.Log("destroying");//REMOVE:
            enemyManager.CheckEndWave();
            //drops a random amount of spores 0-5 set from line 18
            if(dropAmount != 0){
                onDeathDrop.GetComponent<ItemPickup>().amount = dropAmount;
                Instantiate(onDeathDrop, transform.position, transform.rotation);
            }
            _enemyAttack.KillEnemy();
            Destroy(gameObject);
            Debug.Log("destroyed");//REMOVE:
        }
    }

    //respawn itself MUST BE CALLED BEFORE DESTROY
    private void Respawn()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
    
    public int GetTypeI()
    {
        return enemyTypeI;
    }

    public bool IsInRoom()
    {
        if (roomMan.GetRoomTypeI() == enemyTypeI)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void BIGUPDAHOLEISLAND()//REMOVE:
    {
        Debug.Log("BIG UP DA wHOLE ISLAND");
    }
}
