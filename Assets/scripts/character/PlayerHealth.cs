using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int spores = 0;
    public AudioManager audioMan;
    
    private int health;
    private PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        health = maxHealth;
        HudManager.Instance.UpdHealth(health);
        HudManager.Instance.UpdSpores(spores);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
        RemSpores(10);}//test giving sporesd to the player REMOVE:
        if(Input.GetKeyDown(KeyCode.RightShift)){
        GiveSpores(10);}//test removing spores from the player REMOVE:
        if(Input.GetKeyDown(KeyCode.LeftControl)){
        DamagePlayer(10);}//test damaging the player REMOVE:
        if(Input.GetKeyDown(KeyCode.RightControl)){
        GiveHealth(10);}//test giving health to the player REMOVE:
        
        //convert spores into health
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("q key down");//REMOVE:
            StartCoroutine(HealCoroutine());
        }
    }

    //damages the player by int value damage
    public void DamagePlayer(int damage)
    {
        health -= damage;
        //checks if player has died and if true resets current scene
        if(health <= 0){
            Debug.Log("player died");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);//change this to death scene TODO:
        }
        HudManager.Instance.UpdHealth(health);
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
            Debug.Log("Insufficient Spores");
            return false;
        }

        
    }

    public void GiveHealth(int amount)
    {
        health += amount;
        if(health > maxHealth){
            health = maxHealth;
        }
        HudManager.Instance.UpdHealth(health);
    }

    private IEnumerator HealCoroutine()
    {
        Debug.Log("heal start");//REMOVE:
        while (Input.GetKey(KeyCode.Q) && health != maxHealth && spores != 0)
        {
            
            Debug.Log("health low");//REMOVE:
            if (RemSpores(1))
            {
                if (!audioMan.CheckHealing())
                {
                    Debug.Log("not healing");//REMOVE:
                    audioMan.StartCoroutine(audioMan.StartHeal());
                }
                Debug.Log("healing");//REMOVE:
                playerMove.playerSpeed = 1f;
                GiveHealth(1);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (audioMan.CheckHealing())
        {
            audioMan.StartCoroutine(audioMan.StopHeal());
        }
        playerMove.playerSpeed = 17f;
    }
}
