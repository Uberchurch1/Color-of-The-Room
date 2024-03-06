using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private string[] roomTypes;
    private string currentRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        roomTypes = new string[] { "blue", "brown" };
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
    
    public string GetRoomTypeI()
    {
        return currentRoom;
    }

    public void SetRoomTypeI(int roomType)
    {
        try
        {
            if (roomTypes.Length < roomType)
            {
                currentRoom = roomTypes[roomType];
                return;
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
