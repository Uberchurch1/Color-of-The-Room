using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(dir);
    }
}
