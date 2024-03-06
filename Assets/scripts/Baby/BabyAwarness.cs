using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAwarness : MonoBehaviour
{
    public float awarenessRad = 10f;
    public bool isAggro;
    private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist < awarenessRad)
        {
            isAggro = true;
        }
        else
        {
            isAggro = false;
        }
    }
}
