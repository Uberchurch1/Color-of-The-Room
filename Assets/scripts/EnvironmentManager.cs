using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Material[] environMats;
    private Material environMat;
    private GameObject[] environObjects;
    private RoomManager roomMan;
    private int environRoom = -1;
    // Start is called before the first frame update
    void Start()
    {
        roomMan = FindObjectOfType<RoomManager>();
        environObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            environObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (roomMan.GetRoomTypeI() != environRoom)
        {
            environRoom = roomMan.GetRoomTypeI();
            environMat = environMats[environRoom];
            UpdMats();
        }
    }

    void UpdMats()
    {
        List<Material> environList = new List<Material>(){environMat};
        foreach (var environObj in environObjects)
        {
            environObj.GetComponent<MeshRenderer>().SetMaterials(environList);
        }
    }
}
