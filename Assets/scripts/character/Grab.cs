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
            TryGrab();
        }
    }

    private void TryGrab()
    {
        //checks only the first baby
        int hitTotal = 0;
        foreach (var baby in babyManager.babiesInTrigger)
        {
            if(hitTotal > 0){
                break;
            }
            var dir = baby.transform.position - transform.position;
            //checks for line of sight with enemies
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, range*2f, raycastLayerMask))//can multiply range to reach corners but will overextend
            {
                if(hit.transform == baby.transform)
                {
                    hudMan.GrabTrigger();
                    baby.Eat();
                    //play shoot audio
                    GetComponent<AudioSource>().Stop();
                    GetComponent<AudioSource>().Play();
                    break;
                }
            }
            hitTotal++;
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
