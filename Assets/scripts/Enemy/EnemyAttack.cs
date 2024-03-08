using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float atkRange;
    private float enemyDmg;
    private float atkSpeed;
    private BoxCollider atkCollider;
    private bool inAtkRange;
    private Enemy parentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
        atkRange = parentEnemy.atkRange;
        enemyDmg = parentEnemy.enemyDmg;
        atkSpeed = parentEnemy.atkSpeed;
        
        
        atkCollider = GetComponent<BoxCollider>();
        atkCollider.size = new Vector3(1, 2, atkRange);
        atkCollider.center = new Vector3(0, 0, atkRange * 0.5f);
    }

    private IEnumerator atkCoroutine(PlayerHealth player)
    {
        Debug.Log("starting atkCoroutine");//REMOVE:
        while (inAtkRange)
        {  
            yield return new WaitForSeconds(atkSpeed*0.2f);
            Debug.Log("returned to coroutine"); //REMOVE:
            if (inAtkRange)
            {
                Debug.Log("damaging player"); //REMOVE:
                player.DamagePlayer(enemyDmg);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter: "+other);//REMOVE:
        PlayerHealth player = other.transform.GetComponent<PlayerHealth>();
        if (player)
        {
            Debug.Log("triggered player: "+ player);//REMOVE:
            inAtkRange = true;
            StartCoroutine(atkCoroutine(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit: "+other);//REMOVE:
        PlayerHealth player = other.transform.GetComponent<PlayerHealth>();
        if (player)
        {
            Debug.Log("un-triggered player: "+ player);//REMOVE:
            inAtkRange = false;
            StopAllCoroutines();
        }
    }

    public void KillEnemy()
    {
        Debug.Log("killing enemy");//REMOVE:
        StopAllCoroutines();
    }
}