using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public string[] roomTypes;
    private string currentRoom;
    private int currentRoomI;
    private EnvironmentManager environMan;
    
    // Start is called before the first frame update
    void Start()
    {
        environMan = FindObjectOfType<EnvironmentManager>();
        currentRoom = roomTypes[0];
        currentRoomI = 0;
    }


    public string[] GetRoomList()
    {
       return roomTypes;
    }

    public string GetRoomTypeS()
    {
        return currentRoom;
    }

    public void SetRoomTypeS(string roomType)
    {
        try
        {
            if (roomTypes.Contains(roomType))
            {
                currentRoom = roomType;
                currentRoomI = Array.IndexOf(roomTypes, roomType);
                return;
            }
            else
            {
                throw new Exception("invalid roomType string");
            }
        }
        catch (Exception)
        {
            Debug.Log("invalid roomType string");
        }
    }
    
    public int GetRoomTypeI()
    {
        return currentRoomI;
    }

    public void SetRoomTypeI(int roomType)
    {
        //REMOVE:Debug.Log("setting type: "+roomType);
        try
        {
            //REMOVE:Debug.Log("in try");
            if (roomTypes.Length > roomType)
            {
                currentRoom = roomTypes[roomType];
                currentRoomI = roomType;
                //REMOVE:Debug.Log("set type: "+currentRoom+"("+currentRoomI+")");
                environMan.ChangeWall(roomType);
            }
            else
            {
                throw new Exception("invalid roomType int");
            }
        }
        catch (Exception)
        {
            Debug.Log("invalid roomType int");
        }
    }
}
