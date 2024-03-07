using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public bool respawnable;
    public string babyTypeS;
    private int babyTypeI;
    
    private RoomManager roomMan;
    private BabyManager babyManager;

    private Collider babyCollider;
    //private Animator spriteAnim; FIXME:

    // Start is called before the first frame update
    void Start()
    {
        babyManager = FindObjectOfType<BabyManager>();
        roomMan = FindObjectOfType<RoomManager>();
        
        babyTypeI = Array.IndexOf(roomMan.GetRoomList(), babyTypeS);
        babyCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRoom())
        {
            babyCollider.enabled = true;
        }
        else
        {
            babyCollider.enabled = false;
        }
    }

    public void Eat()
    {
        Debug.Log("eaten type: "+babyTypeI);//REMOVE:
        roomMan.SetRoomTypeI(babyTypeI);
        babyManager.RemoveBaby(this);
        if (!respawnable)
        {
            Destroy(gameObject);
        }
        Debug.Log("destroyed type: "+babyTypeI);//REMOVE:
    }
    
    public int GetTypeI()
    {
        return babyTypeI;
    }
    
    public bool IsInRoom()
    {
        if (roomMan.GetRoomTypeI() != babyTypeI)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
