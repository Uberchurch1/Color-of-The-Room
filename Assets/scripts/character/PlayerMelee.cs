using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float range = 1.5f;
    public float damage = 10f;
    public float hitRate = 1f;

    public EnemyManager enemyManager;
    public AudioManager audioMan;
    private BoxCollider meleeTrigger;

    private float timeToHit;
        
    // Start is called before the first frame update
    void Start()
    {
        
        meleeTrigger = GetComponent<BoxCollider>();
        meleeTrigger.size = new Vector3(1, 2, range);
        meleeTrigger.center = new Vector3(0, 0, range*0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && Time.time > timeToHit)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (enemyManager.EnemyMeleeCount() != 0)
        { 
            audioMan.MeleeHit();
            enemyManager.MeleeDamge(damage); 
        }
        else
        {
            audioMan.MeleeAir();
        }
        timeToHit = Time.time + hitRate;
    }
    private void OnTriggerEnter(Collider other)
    {
        //add potential enemy to list(when looking at enemy)
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if(enemy)
        {
            enemyManager.AddMeleeEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //remove enemy from list(when not looking at enemy)
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if(enemy)
        {
            Debug.Log("enemy exit: "+enemy);//REMOVE:
            enemyManager.RemoveMeleeEnemy(enemy);
        }
    }
    
}