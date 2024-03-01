using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float smallDmg = 1f;
    public float bigDmg = 2f;
    public float range = 20f;
    public float vertRange = 20f;
    public float fireRate = 1f;
    public float gunShotRadius = 20f;

    public EnemyManager enemyManager;
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    private float timeToFire;
    private BoxCollider gunTrigger;


    // Start is called before the first frame update
    void Start()
    {
        //creates a trigger hit box(big box that will damage all enemies inside)
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, vertRange, range);
        gunTrigger.center = new Vector3(0, 0, range*0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //check if can shoot(fire delay)
        if(Input.GetMouseButtonDown(0) && Time.time > timeToFire)
        {
            fire();
        }
    }

    void fire()
    {
        //alert enemies in gun shot radius
        Collider[] enemyColliders;
        enemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
        foreach (var enemyCollider in enemyColliders)
        {
            enemyCollider.GetComponent<EnemyAwareness>().isAggro = true;
        }

        //play shoot audio
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();

        //checks for each enemy in the trigger hit box
        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            var dir = enemy.transform.position - transform.position;
            //checks for line of sight with enemies
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, range, raycastLayerMask))//can multiply range to reach corners but will overextend
            {
                if(hit.transform == enemy.transform)
                {
                    //applies different damage amounts base off of half range
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);
                    if(dist > range * 0.5f)
                    {
                        enemy.TakeDamage(smallDmg);
                    }
                    else
                    {
                        enemy.TakeDamage(bigDmg);
                    }

                    
                }
            }

        }
        
        timeToFire = Time.time + fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        //add potential enemy to list(when looking at enemy)
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if(enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //remove enemy from list(when not looking at enemy)
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if(enemy)
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
