using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public bool respawnable;
    public string babyTypeS;
    private int babyTypeI;
    private AudioSource walkSource;
    private RoomManager roomMan;
    private BabyManager babyManager;

    private Collider babyCollider;
    //private Animator spriteAnim; FIXME:

    // Start is called before the first frame update
    void Start()
    {
        babyManager = FindObjectOfType<BabyManager>();
        roomMan = FindObjectOfType<RoomManager>();
        walkSource = GetComponent<AudioSource>();
        babyTypeI = Array.IndexOf(roomMan.GetRoomList(), babyTypeS);
        babyCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRoom())
        {
            babyCollider.enabled = true;
            walkSource.enabled = true;
        }
        else
        {
            babyCollider.enabled = false;
            walkSource.enabled = false;
        }
    }

    public void Eat()
    {
        //REMOVE:Debug.Log("eaten type: "+babyTypeI);
        roomMan.SetRoomTypeI(babyTypeI);
        babyManager.RemoveBaby(this);
        if (!respawnable)
        {
            Destroy(gameObject);
        }
        //REMOVE:Debug.Log("destroyed type: "+babyTypeI);
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
