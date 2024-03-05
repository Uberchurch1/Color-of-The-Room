using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public string babyType;

    private BabyManager babyManager;
    //private Animator spriteAnim; FIXME:

    // Start is called before the first frame update
    void Start()
    {
        babyManager = FindObjectOfType<BabyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Eat()
    {
        babyManager.RemoveBaby(this);
        Destroy(gameObject);
    }
}
