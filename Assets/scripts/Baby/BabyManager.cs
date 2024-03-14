using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyManager : MonoBehaviour
{
    public List<Baby> babiesInTrigger = new List<Baby>();
    public GameObject hudManager;
    public Animator grabAnim;
    public List<Door> doorsInTrigger = new List<Door>();
    public List<Shop> shopsInTrigger = new List<Shop>();

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
            hudManager.GetComponent<HudManager>().ShowGrab(false);
        }
    }
    
    public void RemoveBaby(Baby baby)
    {
        babiesInTrigger.Remove(baby);
        if(babiesInTrigger.Count == 0 && doorsInTrigger.Count == 0 && shopsInTrigger.Count == 0){
            hudManager.GetComponent<HudManager>().HideGrab();
        }
    }

    public int BabyCount()
    {
        return babiesInTrigger.Count;
    }
    
    public void AddDoor(Door door)
    {
        doorsInTrigger.Add(door);
        if(doorsInTrigger.Count != 0){
            hudManager.GetComponent<HudManager>().ShowGrab(false);
        }
    }
    
    public void RemoveDoor(Door door)
    {
        doorsInTrigger.Remove(door);
        if(babiesInTrigger.Count == 0 && doorsInTrigger.Count == 0 && shopsInTrigger.Count == 0){
            hudManager.GetComponent<HudManager>().HideGrab();
        }
    }
    
    public void AddShop(Shop shop)
    {
        shopsInTrigger.Add(shop);
        if(shopsInTrigger.Count != 0){
            hudManager.GetComponent<HudManager>().ShowGrab(false);
        }
    }
    
    public void RemoveShop(Shop shop)
    {
        shopsInTrigger.Remove(shop);
        if(babiesInTrigger.Count == 0 && doorsInTrigger.Count == 0 && shopsInTrigger.Count == 0){
            hudManager.GetComponent<HudManager>().HideGrab();
        }
    }
}
