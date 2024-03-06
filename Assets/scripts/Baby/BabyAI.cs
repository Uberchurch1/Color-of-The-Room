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
    private bool running;
    private Transform playerTransform;
    public float safeRange = 5f;

    private NavMeshAgent babyNavMesh;
    // Start is called before the first frame update
    void Start()
    {
        babyAware = GetComponent<BabyAwarness>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
        babyNavMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (babyAware.isAggro && !running)
        {
            babyNavMesh.SetDestination(safeSpots[FindSafeSpot(transform.position)]);
            running = true;
        }

        if (running)
        {
            if (CheckSafeSpot(transform.position) != -1)
            {
                running = false;
            }
        }
    }

    private int CheckSafeSpot(Vector3 currentPos)
    {
        foreach (var spot in safeSpots)
        {
            var dist = Vector3.Distance(spot, currentPos);
            if (dist <= safeRange)
            {
                return Array.IndexOf(safeSpots, spot);
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
