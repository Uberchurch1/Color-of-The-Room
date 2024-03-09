using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public float range = 1.5f;
    public BabyManager babyManager;
    public LayerMask raycastLayerMask;
    private BoxCollider grabTrigger;
    public HudManager hudMan;
    


    // Start is called before the first frame update
    void Start()
    {
        grabTrigger = GetComponent<BoxCollider>();
        grabTrigger.size = new Vector3(1.5f, 1.5f, range);
        grabTrigger.center = new Vector3(0,0, range*0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            TryEat();
        }
    }

    private void TryEat()
    {
        //checks only the first baby
        foreach (var baby in babyManager.babiesInTrigger)
        {
            hudMan.GrabTrigger();
            baby.Eat();
            //play eat audio
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().Play();
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Baby baby = other.transform.GetComponent<Baby>();
        if(baby){
            babyManager.AddBaby(baby);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Baby baby = other.transform.GetComponent<Baby>();
        if(baby){
            babyManager.RemoveBaby(baby);
        }
    }
}
