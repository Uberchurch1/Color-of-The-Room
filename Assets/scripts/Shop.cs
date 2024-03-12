using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private Dictionary<int, bool> itemDict = new Dictionary<int, bool>()
    {
        { 0, true }, { 1, true }, { 2, true }, { 3, true }, { 4, true }, { 5, true }
    };
    private HudManager _hudManager;
    private PlayerHealth _playerHealth;
    private AudioManager audioMan;
    // Start is called before the first frame update
    void Start()
    {
        audioMan = FindObjectOfType<AudioManager>();
        _hudManager = HudManager.Instance;
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public bool OpenShop()
    {
        audioMan.DoorOpen();
        _hudManager.ToggleShopMenuB();
        return true;
    }

    public bool CheckAvailable(int item)
    {
        return itemDict[item];
    }

    public bool BuyItem(int item,int price)
    {
        if (_playerHealth.RemSpores(price))
        {
            itemDict[item] = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RestockItem(int item)
    {
        itemDict[item] = true;
    }
}
