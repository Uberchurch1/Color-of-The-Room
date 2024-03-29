using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class EnemyAI : MonoBehaviour
{
    private EnemyAwareness enemyAware;
    private Collider playerTransform;
    private NavMeshAgent enemyNavMesh;
    private AudioSource walkSource;

    private void Start()
    {
        enemyAware = GetComponent<EnemyAwareness>();
        playerTransform = FindObjectOfType<PlayerMove>().GetComponentInParent<CapsuleCollider>();
        enemyNavMesh = GetComponent<NavMeshAgent>();
        walkSource = GetComponent<AudioSource>();
    }
    
    private void Update() {
        if (enemyAware.isAggro)
        {
            enemyNavMesh.SetDestination(playerTransform.ClosestPointOnBounds(transform.position));
            if (!walkSource.isPlaying && walkSource.enabled)
            {
                walkSource.Play();
            }
        }
        else{
            enemyNavMesh.SetDestination(transform.position);
            if(walkSource.isPlaying)
            {
                walkSource.Stop();
            }
        }
    }

    public void ChangeSpeed(float speed)
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        enemyNavMesh.speed = speed;
    }
}
