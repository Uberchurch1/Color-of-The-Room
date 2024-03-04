using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI spores;

    public Image healthIndicator;
    public Sprite health1;
    public Sprite health2;
    public Sprite health3;
    public Sprite health4;
    public Sprite health5;
    public Sprite health6;
    public Sprite health7;
    public Sprite health8;

    private static HudManager _instance;
    public static HudManager Instance {
        get {return _instance;}
    }

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }
    
    
    public void updHealth(int healthVal)
    {
        health.text = healthVal.ToString();
        updHealthInd(healthVal);
    }

    public void updHealthInd(int healthVal)
    {
        if(healthVal >= 87){
            healthIndicator.sprite = health1;
        }
        else if(healthVal >= 74){
            healthIndicator.sprite = health2;
        }
        else if(healthVal >= 61){
            healthIndicator.sprite = health3;
        }
        else if(healthVal >= 48){
            healthIndicator.sprite = health4;
        }
        else if(healthVal >= 35){
            healthIndicator.sprite = health5;
        }
        else if(healthVal >= 22){
            healthIndicator.sprite = health6;
        }
        else if(healthVal >= 9){
            healthIndicator.sprite = health7;
        }
        else{
            healthIndicator.sprite = health8;
        }
    }

    public void updSpores(int sporeVal)
    {
        spores.text = sporeVal.ToString();
    }
}
