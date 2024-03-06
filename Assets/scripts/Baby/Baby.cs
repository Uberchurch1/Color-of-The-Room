using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public string babyTypeS;
    private int babyTypeI;
    public Sprite[] sprites;
    
    private RoomManager roomMan;
    private BabyManager babyManager;
    //private Animator spriteAnim; FIXME:

    // Start is called before the first frame update
    void Start()
    {
        babyManager = FindObjectOfType<BabyManager>();
        roomMan = FindObjectOfType<RoomManager>();
        babyTypeI = Array.IndexOf(roomMan.GetRoomList(), babyTypeS);
        //set sprite to correct type
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Eat()
    {
        babyManager.RemoveBaby(this);
        Destroy(gameObject);
        roomMan.SetRoomTypeI(babyTypeI);
    }
}
