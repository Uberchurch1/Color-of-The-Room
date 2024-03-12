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

    private void Start()
    {
        ItemDescGroup = GetComponentInChildren<ItemDesc>().gameObject;
        button = GetComponentInChildren<Button>().gameObject;
        ItemDescGroup.SetActive(false);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        shop = FindObjectOfType<Shop>();
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
    }

    private void OnEnable()
    {
        shop = FindObjectOfType<Shop>();
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        if (_sceneSwitcher.CheckCurrentScene("Room"))
        {
            if (shop.CheckAvailable(itemNum))
            {
                button.SetActive(true);
            }
            else
            {
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
