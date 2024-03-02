using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int spores = 0;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
        RemSpores(10);}//test giving sporesd to the player REMOVE:
        if(Input.GetKeyDown(KeyCode.RightShift)){
        GiveSpores(10);}//test removing spores from the player REMOVE:
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
    }

    //test damage REMOVE:
    private void TestDmg(int damage){
            DamagePlayer(damage);
            Debug.Log("player damaged by "+damage);
    }

    public void GiveSpores(int amount)
    {
        spores += amount;
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
    }
}
