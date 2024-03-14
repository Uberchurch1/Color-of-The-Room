using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI spores;
    public Image counterBackground;
    public TextMeshProUGUI eatBaby;
    
    public Image sporeIndicator;
    public Image healthIndicator;
    public Image background;
    public Sprite[] healthSheet;
    public Sprite[] sporeSheet;
    public GameObject grabIndicator;
    public Animator grabAnim;
    public TextMeshProUGUI attacked;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI turnAround;
    public GameObject exitMenu;
    public GameObject shopMenu;
    public GameObject deathMenu;
    public Image blackScreen;
    public GameObject creditsMenu;
    public Animator gunAnim;
    public TextMeshProUGUI RMB;
        [FormerlySerializedAs("holdE")]
    public TextMeshProUGUI pressE;
    public TextMeshProUGUI pressSpace;
    public TextMeshProUGUI heal;
    public TextMeshProUGUI shoot;
    public float playerSens = 1;
    public float playerVolume = 1;

    private AudioManager audioMan;
    private PlayerHealth _playerHealth;
    private MouseLook _mouseLook;
    private BabyManager babyMan;
    private EnemyManager enemyMan;
    private RoomManager _roomManager;
    public SceneSwitcher _sceneSwitcher;
    private static HudManager _instance;
    public static HudManager Instance {
        get {return _instance;}
    }

    private void Start()
    {
        if (_sceneSwitcher.CheckCurrentScene("Start"))
        {
            ToggleExitMenuB();
            _playerHealth.isPaused = true;
        }
        else if (_sceneSwitcher.CheckCurrentScene("Death"))
        {
            ToggleDeathMenu();
        }
    }

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        //REMOVE:Debug.Log("current scene: "+SceneManager.GetActiveScene().name+" #"+SceneManager.GetActiveScene().buildIndex);
        //REMOVE:Debug.Log("roomSceneI: "+roomSceneI);
        //REMOVE:Debug.Log("shopSceneI: "+shopSceneI);
        if (_sceneSwitcher.CheckCurrentScene("TheRoom"))
        {
            enemyMan = FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
        }
        if (!_sceneSwitcher.CheckCurrentScene("Death"))
        {
            babyMan = FindObjectOfType<BabyManager>().GetComponent<BabyManager>();
        }
        attacked.alpha = 0f;
        RMB.alpha = 0f;
        pressE.alpha = 0f;
        pressSpace.alpha = 0f;
        turnAround.alpha = 0f;
        heal.alpha = 0f;
        shoot.alpha = 0f;
        countdown.text = "";
        exitMenu.SetActive(false);
        shopMenu.SetActive(false);
        deathMenu.SetActive(false);
        creditsMenu.SetActive(false);
        eatBaby.enabled = false;
        blackScreen.color = new Color(0, 0, 0, 0);
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _mouseLook = FindObjectOfType<MouseLook>();
        audioMan = FindObjectOfType<AudioManager>();
        _roomManager = FindObjectOfType<RoomManager>();
    }

    private void Update()
    {
        if (_sceneSwitcher.CheckCurrentScene("Start"))
        {
            if (_playerHealth.CheckHealth() <= 100)
            {
                heal.alpha = 1f;
            }
            else
            {
                heal.alpha = 0f;
            }
            if (_playerHealth.spores >= 0)
            {
                shoot.alpha = 1f;
            }
            else
            {
                shoot.alpha = 0f;
            }
        }
        else
        {
            heal.alpha = 0f;
            shoot.alpha = 0f;
        }
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

    public void ShowGrab(bool melee)
    {
        if (melee)
        {
            pressSpace.alpha = 1f;
        }
        else
        {
            pressE.alpha = 1f;
        }
        grabAnim.SetBool("HoverActive", true);
    }

    public void HideGrab()
    {
        if (_sceneSwitcher.CheckCurrentScene("TheRoom"))
        {
            if ((enemyMan.EnemyMeleeCount() == 0) && (babyMan.BabyCount() == 0))
            {
                grabAnim.SetBool("HoverActive", false);
                pressSpace.alpha = 0f;
                pressE.alpha = 0f;
            }
        }
        else
        {
            grabAnim.SetBool("HoverActive", false);
            pressSpace.alpha = 0f;
            pressE.alpha = 0f;
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
        if (_sceneSwitcher.CheckCurrentScene("Start"))
        {
            RMB.alpha = 1.25f;
        }
        attacked.alpha = 1.25f;
        background.color = new Color(1, 0.75f, 0.75f);
        yield return new WaitForSeconds(0.25f);
        while (attacked.alpha > 0)
        {
            attacked.alpha -= .05f;
            RMB.alpha -= 0.05f;
            background.color += new Color(0, 0.01f, 0.01f);
            yield return new WaitForSeconds(.05f);
        }

        RMB.alpha = 0f;
    }

    public IEnumerator SpAttackCoroutine(int room)
    {
        eatBaby.enabled = true;
        yield return new WaitForSeconds(5f);
        StartCoroutine(CountdownCoroutine());
        while (_roomManager.GetRoomTypeI() == room)
        {
            yield return new WaitForSeconds(0.8f);
        }
        StopCoroutine(CountdownCoroutine());
        countdown.text = "";
        eatBaby.enabled = false;
        GetComponent<AudioSource>().Stop();
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

    public void ToggleExitMenu()
    {
        if (exitMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            exitMenu.SetActive(false);
            Time.timeScale = 1;
            AudioListener.pause = false;
            counterBackground.enabled = true;
            spores.enabled = true;
            _playerHealth.isPaused = false;
        }
        else
        {
            exitMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
            //audioMan.IgnorePause();not needed
            AudioListener.pause = true;
            counterBackground.enabled = false;
            spores.enabled = false;
            _playerHealth.isPaused = true;
        }
    }
    public bool ToggleExitMenuB()
    {
        ToggleExitMenu();
        return exitMenu.activeSelf;
    }
    
    public void ToggleDeathMenu()
    {
        if (deathMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            deathMenu.SetActive(false);
            Time.timeScale = 1;
            AudioListener.pause = false;
            counterBackground.enabled = true;
            spores.enabled = true;
        }
        else
        {
            deathMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
            //audioMan.IgnorePause();not needed
            AudioListener.pause = true;
            counterBackground.enabled = false;
            spores.enabled = false;
        }
    }

    public void Restart()
    {
        Destroy(_playerHealth.gameObject);
        _sceneSwitcher.SwitchScene("Start");
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
            StartCoroutine(audioMan.ShopExit());
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

    public void GunShoot()
    {
        gunAnim.SetTrigger("fire");
    }

    public void GunWalk(bool isWalking)
    {
        gunAnim.SetBool("isWalking",isWalking);
    }

    public IEnumerator CreditsFade()
    {
        _playerHealth.isPaused = true;
        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0,0,0,0.05f);
            yield return new WaitForSeconds(0.08f);
        }
        creditsMenu.SetActive(true);
        while (blackScreen.color.a > 0)
        {
            blackScreen.color -= new Color(0,0,0,0.05f);
            yield return new WaitForSeconds(0.16f);
        }
    }

    public void UpdSensitivity(float value)
    {
        playerSens = value;
        _mouseLook.sliderSens = playerSens;
    }
}
