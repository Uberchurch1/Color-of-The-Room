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
    private PlayerHealth _playerHealth;
    private AudioManager audioMan;

    // Start is called before the first frame update
    void Start()
    {
        grabTrigger = GetComponent<BoxCollider>();
        grabTrigger.size = new Vector3(1.5f, 1.5f, range);
        grabTrigger.center = new Vector3(0,0, range*0.5f);
        _playerHealth = GetComponentInParent<PlayerHealth>();
        audioMan = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerHealth.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!TryEat())
                {
                    if (!TryOpenShop())
                    {
                        TryOpenDoor();
                    }
                }
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
        hudMan = HudManager.Instance;
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
                //play audioMan.EatSound()
                StartCoroutine(audioMan.EatSound());
                return true;
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    private bool TryOpenDoor()
    {
        foreach (var door in babyManager.doorsInTrigger)
        {
            hudMan.GrabTriggerCoroutine();
            return door.Open();
        }
        //REMOVE:Debug.Log("did not try open");
        return false;
    }

    private bool TryOpenShop()
    {
        foreach (var shop in babyManager.shopsInTrigger)
        {
            hudMan.GrabTriggerCoroutine();
            return shop.OpenShop();
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //REMOVE:Debug.Log("grab entered: "+other);
        Baby baby = other.transform.GetComponent<Baby>();
        if(baby){
            babyManager.AddBaby(baby);
            //REMOVE:Debug.Log("added baby");
        }

        Door door = other.transform.GetComponent<Door>();
        if(door){
            babyManager.AddDoor(door);
            //REMOVE:Debug.Log("added door");
        }
        Shop shop = other.transform.GetComponent<Shop>();
        if(shop){
            babyManager.AddShop(shop);
            //REMOVE:Debug.Log("added shop");
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
        Shop shop = other.transform.GetComponent<Shop>();
        if(shop){
            babyManager.RemoveShop(shop);
            //REMOVE:Debug.Log("added shop");
        }
    }
}
