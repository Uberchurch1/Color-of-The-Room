using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    public Material[] environMats0;
    public Material[] environMats0B;
    public Material[] environMats1;
    private float[] timings0 = new float[11] { .25f, .2f, .2f, .15f, .15f, .15f, .15f, .15f, .2f, .2f, .25f };
    private float[] timings1 = new float[5] { .2f, .2f, .2f, .2f, .2f};
    private Material environMat;
    private GameObject[] environObjects;
    private RoomManager roomMan = null;
    // Start is called before the first frame update
    void Start()
    {
        roomMan = FindObjectOfType<RoomManager>();
        environObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            environObjects[i] = transform.GetChild(i).gameObject;
        }

        ChangeWall(roomMan.GetRoomTypeI());
    }

    // Update is called once per frame
    void Update()
    {
        if (roomMan == null)
        {
            roomMan = FindObjectOfType<RoomManager>();
        }
    }

    public void ChangeWall(int room)
    {
        StopAllCoroutines();
        if (room == 0)
        {
            StartCoroutine(WallCoroutine0());
        }
        else
        {
            StartCoroutine(WallCoroutine1());
        }
    }

    private IEnumerator WallCoroutine0()
    {
        while (roomMan.GetRoomTypeI() == 0)
        {
            for (int i = 0; i < environMats0.Length; i++)
            {
                environMat = environMats0[i];
                UpdMats();
                yield return new WaitForSeconds(timings0[i]);
            }
            for (int i = environMats0.Length-2; i > 0; i--)
            {
                if (i == 7 && Random.Range(0, 2) == 0)
                {
                    for (int j = 0; j < environMats0B.Length; j++)
                    {
                        environMat = environMats0B[j];
                        UpdMats();
                        yield return new WaitForSeconds(.15f);
                        i--;
                    }
                }
                else
                {
                    environMat = environMats0[i];
                    UpdMats();
                    yield return new WaitForSeconds(timings0[i]);
                }
            }
        }
    }
    
    private IEnumerator WallCoroutine1()
    {
        while (roomMan.GetRoomTypeI() == 1)
        {
            for (int i = 0; i < environMats1.Length; i++)
            {
                environMat = environMats1[i];
                UpdMats();
                yield return new WaitForSeconds(timings1[i]);
            }
            for (int i = environMats1.Length-2; i > 0; i--)
            {
                environMat = environMats1[i];
                UpdMats();
                yield return new WaitForSeconds(timings1[i]);
            }
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
