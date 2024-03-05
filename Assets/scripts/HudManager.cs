using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private BabyManager babyMan;

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
        babyMan = FindObjectOfType<BabyManager>().GetComponent<BabyManager>();
    }
    
    
    public void UpdHealth(int healthVal)
    {
        health.text = healthVal.ToString();
        float index = (healthVal / 100f) * healthSheet.Length;
        int Index = (int)Math.Floor(index);
        healthIndicator.sprite = healthSheet[Index];
    }

    public void UpdSpores(int sporeVal)
    {
        spores.text = sporeVal.ToString();
        float index = (sporeVal / 100f) * sporeSheet.Length;
        int Index = (int)Math.Floor(index);
        sporeIndicator.sprite = sporeSheet[Index];
    }

    public void ShowGrab()
    {
        grabAnim.SetBool("HoverActive", true);
    }

    public void HideGrab()
    {
        grabAnim.SetBool("HoverActive", false);
    }

    public void GrabTrigger()
    {
        grabAnim.SetTrigger("GrabActive");
    }
}
