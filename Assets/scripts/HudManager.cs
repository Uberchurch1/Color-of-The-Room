using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI spores;

    public Image sporeIndicator;
    public Image healthIndicator;
    public Sprite[] healthSheet;
    public Sprite[] sporeSheet;
    public GameObject grabIndicator;
    public Animator grabAnim;
    public TextMeshProUGUI attacked;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI turnAround;
    public GameObject exitMenu;
    public GameObject shopMenu;
    
    private PlayerHealth _playerHealth;
    private BabyManager babyMan;
    private EnemyManager enemyMan;
    private int roomSceneI;
    private int shopSceneI;
    public SceneSwitcher _sceneSwitcher;
    private static HudManager _instance;
    public static HudManager Instance {
        get {return _instance;}
    }

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //_sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        roomSceneI = SceneManager.GetSceneByName("TheRoom").buildIndex;
        shopSceneI = SceneManager.GetSceneByName("Shop").buildIndex;
        Debug.Log("current scene: "+SceneManager.GetActiveScene().name+" #"+SceneManager.GetActiveScene().buildIndex);//REMOVE:
        Debug.Log("roomSceneI: "+roomSceneI);//REMOVE:
        Debug.Log("shopSceneI: "+shopSceneI);//REMOVE:
        if (_sceneSwitcher.CheckCurrentScene("TheRoom"))
        {
            enemyMan = FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
        }
        babyMan = FindObjectOfType<BabyManager>().GetComponent<BabyManager>();
        attacked.alpha = 0f;
        turnAround.alpha = 0f;
        countdown.text = "";
        exitMenu.SetActive(false);
        shopMenu.SetActive(false);
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }
    
    
    public void UpdHealth(int healthVal)
    {
        health.text = healthVal.ToString();
        float index = Math.Clamp((healthVal / 100f) * healthSheet.Length, 0, healthSheet.Length-1);
        int Index = (int)Math.Floor(index);
        healthIndicator.sprite = healthSheet[Index];
    }

    public void UpdSpores(int sporeVal)
    {
        spores.text = sporeVal.ToString();
        float index = Math.Clamp((sporeVal / 100f) * sporeSheet.Length, 0, sporeSheet.Length-1);
        int Index = (int)Math.Floor(index);
        sporeIndicator.sprite = sporeSheet[Index];
    }

    public void ShowGrab()
    {
        grabAnim.SetBool("HoverActive", true);
    }

    public void HideGrab()
    {
        if (_sceneSwitcher.CheckCurrentScene("TheRoom"))
        {
            if ((enemyMan.EnemyMeleeCount() == 0) && (babyMan.BabyCount() == 0))
            {
                grabAnim.SetBool("HoverActive", false);
            }
        }
        else
        {
            grabAnim.SetBool("HoverActive", false);
        }
    }

    public IEnumerator GrabTriggerCoroutine()
    {
        grabAnim.SetTrigger("GrabActive");
        yield return new WaitForSeconds(.26f);
        grabAnim.SetBool("HoverActive", false);
    }

    public IEnumerator AtkIndCoroutine()
    {
        attacked.alpha = 1.5f;
        while (attacked.alpha > 0)
        {
            attacked.alpha -= .25f;
            yield return new WaitForSeconds(.25f);
        }
    }

    public IEnumerator CountdownCoroutine()
    {
        GetComponent<AudioSource>().Play();
        for (int i = 5; i > 0; i--)
        {
            countdown.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdown.text = "";
    }

    public bool ToggleExitMenu()
    {
        if (exitMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            exitMenu.SetActive(false);
            Time.timeScale = 1;
            AudioListener.pause = false;
            return false;
        }
        else
        {
            exitMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            return true;
        }
    }

    public bool ToggleShopMenuB()
    {
        ToggleShopMenu();
        return shopMenu.activeSelf;
    }
    
    public void ToggleShopMenu()
    {
        if (shopMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            shopMenu.SetActive(false);
        }
        else
        {
            shopMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        _playerHealth.isPaused = shopMenu.activeSelf;
    }
}
