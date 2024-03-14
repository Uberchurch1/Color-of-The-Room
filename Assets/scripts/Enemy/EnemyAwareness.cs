using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRad = 8f;
    public Material aggroMat;
    public bool isAggro;
    
   
    
    private Transform playerTransform;
    private float distToPLayer;

    private void Start() {
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    private void Update() 
    {
        distToPLayer = Vector3.Distance(transform.position, playerTransform.position);
        if(distToPLayer <= awarenessRad){
            isAggro = true;
        }
        //if (isAggro){
        //    GetComponent<MeshRenderer>().material = aggroMat;
        //}
        
    }
    
    

    
}
