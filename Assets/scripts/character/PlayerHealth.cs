using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    
    public int spores;
    public AudioManager audioMan;
    private float health;
    private PlayerMove playerMove;
    private SceneSwitcher _sceneSwitcher;
    public int roundCount = -1;
    public bool seeking = false;
    public bool isPaused = false;
    public bool isHealing = false;
    public bool hasBigSpore = false;
    
    public float maxHealth = 100;
    public int seekCost = 4;
    public int healCost = 2;
    public int healAmount = 2;
    public int bossRound = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        health = maxHealth;
        HudManager.Instance.UpdHealth((int)health);
        HudManager.Instance.UpdSpores(spores);
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        spores = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)){
            RemSpores(50);}//test giving spores to the player //REMOVE:
        if(Input.GetKeyDown(KeyCode.P)){
            GiveSpores(50);}//test removing spores from the player //REMOVE:
        if(Input.GetKeyDown(KeyCode.U)){
            DamagePlayer(10);}//test damaging the player //REMOVE:
        if(Input.GetKeyDown(KeyCode.I)){
            GiveHealth(10);}//test giving health to the player //REMOVE:
        if (Input.GetKeyDown(KeyCode.J)) {
            _sceneSwitcher.SwitchScene("Start"); }//REMOVE:
        if (Input.GetKeyDown(KeyCode.K)) {
            _sceneSwitcher.SwitchScene("TheRoom"); }//REMOVE:
        if (Input.GetKeyDown(KeyCode.L)) {
            _sceneSwitcher.SwitchScene("Shop"); }//REMOVE:

        if (!isPaused)
        {
            if (!seeking)
            {
                //convert spores into health
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    //REMOVE:Debug.Log("shift key down");
                    StartCoroutine(HealCoroutine());
                }

                if (!isHealing)
                {
                    audioMan.StopInhale();
                }
            }

            //see through rooms on right click
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(SeekCoroutine());
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPaused = HudManager.Instance.ToggleExitMenuB();
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
        //REMOVE:Debug.Log("loading new scene: "+scene.name);
        HudManager.Instance.UpdHealth((int)health);
        HudManager.Instance.UpdSpores(spores);
    }

    //damages the player by int value damage
    public void DamagePlayer(float damage)
    {
        //play audioMan.DamageSound()
        audioMan.DamageSound();
        health -= damage;
        HudManager.Instance.StopAllCoroutines();
        HudManager.Instance.StartCoroutine(HudManager.Instance.AtkIndCoroutine());
        //checks if player has died and if true resets current scene
        if(health <= 0){
            Debug.Log("player died");
            _sceneSwitcher.SwitchScene("Death");
        }
        HudManager.Instance.UpdHealth((int)health);
    }

    public void GiveSpores(int amount)
    {
        spores += amount;
        HudManager.Instance.UpdSpores(spores);
    }

    public bool RemSpores(int amount)
    {
        try
        {
            if(spores < amount){
                throw new Exception("Insufficient Spores");
            }
            else{
                spores -= amount;
                HudManager.Instance.UpdSpores(spores);
                return true;
            }
        }
        catch (Exception)
        {
            //REMOVE:Debug.Log("Insufficient Spores");
            return false;
        }

        
    }

    public void GiveHealth(int amount)
    {
        health += amount;
        if(health > maxHealth){
            health = maxHealth;
        }
        HudManager.Instance.UpdHealth((int)health);
    }

    public float CheckHealth()
    {
        return health;
    }

    private IEnumerator HealCoroutine()
    {
        //REMOVE:Debug.Log("heal start");
        while (Input.GetKey(KeyCode.LeftShift) && (int)health != (int)maxHealth && spores != 0)
        {
            
            //REMOVE:Debug.Log("health low");
            if (RemSpores(healCost))
            {
                if (!audioMan.CheckHealing())
                {
                    //REMOVE:Debug.Log("not healing");
                    audioMan.StartCoroutine(audioMan.StartHeal());
                }
                //REMOVE:Debug.Log("healing");
                playerMove.playerSpeed = 5f;
                GiveHealth(healAmount);
                isHealing = true;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (audioMan.CheckHealing())
        {
            audioMan.StartCoroutine(audioMan.StopHeal());
        }
        playerMove.playerSpeed = 17f;
        audioMan.StopInhale();
        isHealing = false;
    }
    
    private IEnumerator SeekCoroutine()
    {
        Debug.Log("seek start");//REMOVE:
        while (Input.GetMouseButton(1) && spores != 0)
        {
            if (RemSpores(seekCost))
            {
                Time.timeScale = 0.5f;
                //REMOVE:Debug.Log("health low");
                if (!audioMan.CheckHealing())
                {
                    //REMOVE:Debug.Log("not healing");
                    audioMan.StartCoroutine(audioMan.StartHeal());
                }
                playerMove.playerSpeed = 7f;
                seeking = true;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.05f);
        }
        Time.timeScale = 1f;
        if (audioMan.CheckHealing())
        {
            audioMan.StartCoroutine(audioMan.StopHeal());
        }
        seeking = false;
        playerMove.playerSpeed = 17f;
        audioMan.StopInhale();
    }

    public void BuffMaxHealth(int buff)
    {
        maxHealth += (float)buff;
    }
    public void BuffSeekCost(int buff)
    {
        if (seekCost > 1)
        {
            seekCost -= buff;
        }
    }
    public void BuffHealCost(int buff)
    {
        healCost -= buff;
    }
    public void BuffHealAmount(int buff)
    {
        healAmount += buff;
    }
}
