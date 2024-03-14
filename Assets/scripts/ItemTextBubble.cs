using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemTextBubble : MonoBehaviour
{
    private GameObject ItemDescGroup;
    private Shop shop;
    private GameObject button;
    private SceneSwitcher _sceneSwitcher;
        [FormerlySerializedAs("ItemNum")] 
    public int itemNum;
    public int price;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        ItemDescGroup = GetComponentInChildren<ItemDesc>().gameObject;
        button = GetComponentInChildren<Button>().gameObject;
        ItemDescGroup.SetActive(false);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        button = GetComponentInChildren<Button>().gameObject;
        shop = FindObjectOfType<Shop>();
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        if (_sceneSwitcher.CheckCurrentScene("Room"))
        {
            //OnEnable();
        }
    }

    private void OnEnable()
    {
        button = GetComponentInChildren<Button>().gameObject;
        Debug.Log("active scene: "+SceneManager.GetActiveScene().name);
        //REMOVE:Debug.Log("item: "+itemNum+" enabled");
        shop = FindObjectOfType<Shop>();
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        if (_sceneSwitcher.CheckCurrentScene("Shop"))
        {
            //REMOVE:Debug.Log("checking availability");
            if (shop.CheckAvailable(itemNum))
            {
                //REMOVE:Debug.Log("item: "+itemNum+" active");
                button.SetActive(true);
            }
            else
            {
                //REMOVE:Debug.Log("item: "+itemNum+" not active");
                button.SetActive(false);
            }
        }
    }

    public void ToggleVisibleDesc()
    {
        if (ItemDescGroup.activeSelf)
        {
            ItemDescGroup.SetActive(false);
        }
        else
        {
            ItemDescGroup.SetActive(true);
        }
    }

    public void Buy()
    {
        if (shop.BuyItem(itemNum, price))
        {
            ItemDescGroup.SetActive(false);
            button.SetActive(false);
        }
    }
}
