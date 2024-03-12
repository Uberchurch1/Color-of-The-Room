using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BabyAI : MonoBehaviour
{
    public Vector3[] safeSpots;
    private BabyAwarness babyAware;
    public bool running;
    private Transform playerTransform;
    private AudioSource walkSource;
    public float safeRange = 5f;

    private NavMeshAgent babyNavMesh;
    // Start is called before the first frame update
    void Start()
    {
        walkSource = GetComponent<AudioSource>();
        babyAware = GetComponent<BabyAwarness>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
        babyNavMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (babyAware.isAggro && !running)
        {
            running = true;
            babyNavMesh.SetDestination(safeSpots[FindSafeSpot(transform.position)]);
        }

        if (running)
        {
            if (!walkSource.isPlaying && walkSource.enabled)
            {
                walkSource.Play();
            }
            if (CheckSafeSpot(transform.position) != -1)
            {
                if (!babyAware.isAggro)
                {
                    running = false;
                }
            }
        }
        else if (walkSource.isPlaying)
        {
            walkSource.Stop();
        }
    }

    private int CheckSafeSpot(Vector3 currentPos)
    {
        foreach (var spot in safeSpots)
        {
            if (spot != babyNavMesh.destination)
            {
                var dist = Vector3.Distance(spot, currentPos);
                if (dist <= safeRange)
                {
                    return Array.IndexOf(safeSpots, spot);
                }
            }
        }
        return -1;
    }

    private int FindSafeSpot(Vector3 currentPos)
    {
        if (CheckSafeSpot(currentPos) != -1)
        {
            int index = CheckSafeSpot(currentPos);
            int currentSpot = CheckSafeSpot(currentPos);
            while (currentSpot == index)
            {
                index = Random.Range(0, safeSpots.Length);
            }

            return index;
        }
        else
        {
            return Random.Range(0, safeSpots.Length);
        }
    }
}
