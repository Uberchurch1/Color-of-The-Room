using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyManager : MonoBehaviour
{
    public List<Baby> babiesInTrigger = new List<Baby>();
    public GameObject hudManager;
    public Animator grabAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //add/remove babies from grab hitbox
    public void AddBaby(Baby baby)
    {
        babiesInTrigger.Add(baby);
        if(babiesInTrigger.Count != 0){
            hudManager.GetComponent<HudManager>().ShowGrab();
        }
    }
    
    public void RemoveBaby(Baby baby)
    {
        babiesInTrigger.Remove(baby);
        if(babiesInTrigger.Count == 0){
            hudManager.GetComponent<HudManager>().HideGrab();
        }
    }

    public int BabyCount()
    {
        return babiesInTrigger.Count;
    }
}
