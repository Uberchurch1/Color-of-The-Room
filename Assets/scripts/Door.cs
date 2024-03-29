using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private int roomSceneI;
    private int shopSceneI;
    private int titleSceneI;
    private int introSceneI;
    public SceneSwitcher _sceneSwitcher;
    public bool unlocked = false;
    public AudioClip open;
    public AudioClip locked;
    private AudioSource audioSource;
    private AudioManager audioMan;
    
    // Start is called before the first frame update
    void Start()
    {
        roomSceneI = SceneManager.GetSceneByName("TheRoom").buildIndex;
        shopSceneI = SceneManager.GetSceneByName("Shop").buildIndex;
        audioMan = FindObjectOfType<AudioManager>();
        //_sceneSwitcher = FindObjectOfType<SceneSwitcher>();
    }

    private void Awake()
    {
        if (_sceneSwitcher.CheckCurrentScene("Shop"))
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Open()
    {
        if (unlocked)
        {
            audioMan.DoorOpen();
            TryChangeScene();
            return true;
        }
        else
        {
            audioMan.DoorLocked();
            return false;
        }
    }

    public void TryChangeScene()
    {
        if (_sceneSwitcher.CheckCurrentScene("TheRoom"))
        {
            _sceneSwitcher.SwitchScene("Shop");
        }
        else
        {
            if (_sceneSwitcher.CheckCurrentScene("Start"))
            {
                FindObjectOfType<PlayerHealth>().spores = 0;
            }
            _sceneSwitcher.SwitchScene("TheRoom");
        }
    }
}
