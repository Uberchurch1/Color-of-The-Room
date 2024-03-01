using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private EnemyAwareness enemyAware;
    private Transform playerTransform;
    private NavMeshAgent enemyNavMesh;

    private void Start()
    {
        enemyAware = GetComponent<EnemyAwareness>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
        enemyNavMesh = GetComponent<NavMeshAgent>();

    }
    
    private void Update() {
        if (enemyAware.isAggro)
        {
            enemyNavMesh.SetDestination(playerTransform.position);
        }
        else{
            enemyNavMesh.SetDestination(transform.position);
        }
    }
}
