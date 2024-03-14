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
    public AudioSource walkSource;
    public float safeRange = 5f;
    public AudioSource oneShots;
    public AudioClip[] screams;
    public AudioClip seeRightThroughMe;

    private NavMeshAgent babyNavMesh;

    private Baby _baby;
    // Start is called before the first frame update
    void Start()
    {
        _baby = GetComponentInParent<Baby>();
        babyAware = GetComponent<BabyAwarness>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
        babyNavMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (babyAware.isAggro && !running)
        {
            //play scream
            if (_baby.IsInRoom())
            {
                if (Random.Range(0, 200) == 0)
                {
                    oneShots.PlayOneShot(seeRightThroughMe);
                }
                else
                {
                    oneShots.PlayOneShot(screams[Random.Range(0, screams.Length)]);
                }
            }

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
