using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grab : MonoBehaviour
{
    public float range = 1.5f;
    public LayerMask raycastLayerMask;
    private BoxCollider grabTrigger;
    
    private HudManager hudMan;
    private BabyManager babyManager;
    private SceneSwitcher _sceneSwitcher;

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
            if (!TryEat())
            {
                TryOpen();
            }
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        hudMan = FindObjectOfType<HudManager>();
        babyManager = FindObjectOfType<BabyManager>();
    }

    private bool TryEat()
    {
        if (babyManager.babiesInTrigger.Count != 0)
        {
            //checks only the first baby
            foreach (var baby in babyManager.babiesInTrigger)
            {
                hudMan.GrabTriggerCoroutine();
                baby.Eat();
                //play eat audio
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().Play();
                return true;
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    private bool TryOpen()
    {
        foreach (var door in babyManager.doorsInTrigger)
        {
            hudMan.GrabTriggerCoroutine();
            return door.Open();
        }
        Debug.Log("did not try open");//REMOVE:
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("grab entered: "+other);//REMOVE:
        Baby baby = other.transform.GetComponent<Baby>();
        if(baby){
            babyManager.AddBaby(baby);
            Debug.Log("added baby");//REMOVE:
        }

        Door door = other.transform.GetComponent<Door>();
        if(door){
            babyManager.AddDoor(door);
            Debug.Log("added door");//REMOVE:
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Baby baby = other.transform.GetComponent<Baby>();
        if(baby){
            babyManager.RemoveBaby(baby);
        }
        
        Door door = other.transform.GetComponent<Door>();
        if(door){
            babyManager.RemoveDoor(door);
        }
    }
}
