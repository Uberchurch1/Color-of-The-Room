using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    private Dictionary<int, bool> itemDict = new Dictionary<int, bool>()
    {
        { 0, true }, { 1, true }, { 2, true }, { 3, true }, { 4, true }, { 5, true },{6,true}
    };
    private HudManager _hudManager;
    private PlayerHealth _playerHealth;
    private Gun playerGun;
    private PlayerMelee _playerMelee;
    private AudioManager audioMan;
    // Start is called before the first frame update
    void Start()
    {
        audioMan = FindObjectOfType<AudioManager>();
        _hudManager = HudManager.Instance;
        _playerHealth = FindObjectOfType<PlayerHealth>();
        playerGun = FindObjectOfType<Gun>();
        _playerMelee = FindObjectOfType<PlayerMelee>();
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        RestockAll();
    }

    private void OnEnable()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        RestockAll();
    }

    public void RestockAll()
    {
        
        if (_playerHealth.seekCost >= 2)
        {
            //REMOVE:Debug.Log("restocked item 6");
            RestockItem(6);
        }

        for (int i = 1; i <= 5; i++)
        {
            //REMOVE:Debug.Log("restocked item: "+i);
            RestockItem(i);
        }
    }

    public bool OpenShop()
    {
        audioMan.DoorOpen();
        StartCoroutine(OpenShopCoroutine());
        _hudManager.ToggleShopMenuB();
        return true;
    }

    private IEnumerator OpenShopCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        audioMan.StartCoroutine(audioMan.ShopGreeting());
    }

    public bool CheckAvailable(int item)
    {
        //REMOVE:Debug.Log("item: "+item+" available: "+itemDict[item]);
        return itemDict[item];
    }

    public bool BuyItem(int item,int price)
    {
        if (item == 0)
        {
            if (_playerHealth.hasBigSpore)
            {
                audioMan.StartCoroutine(audioMan.ShopBuySpecialItem());
                //end game
                return true;
            }
            else
            {
                audioMan.StartCoroutine(audioMan.ShopSpecialItem());
                return false;
            }
        }
        else if(_playerHealth.RemSpores(price))
        {
            itemDict[item] = false;
            if (item == 1)
            {
                playerGun.BuffGunDmg(10);
            }
            else if (item == 2)
            {
                playerGun.BuffGunPierce(1);
            }
            else if (item == 3)
            {
                _playerMelee.BuffMeleeDmg(10);
            }
            else if (item == 4)
            {
                _playerMelee.BuffMeleeHeal(2);
            }
            else if (item == 5)
            {
                _playerHealth.BuffMaxHealth(5);
                _playerHealth.BuffHealAmount(1);
            }
            else if(item == 6)
            {
                _playerHealth.BuffSeekCost(1);
            }
            //play random voice lines
            audioMan.StartCoroutine(audioMan.ShopBoughtItem());
            return true;
        }
        else
        {
            audioMan.StartCoroutine(audioMan.ShopNotEnoughSpores());
            return false;
        }
    }

    public void RestockItem(int item)
    {
        itemDict[item] = true;
    }
}
