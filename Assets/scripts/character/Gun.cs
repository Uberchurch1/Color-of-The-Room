using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{

    public float smallDmg = 10f;
    public float bigDmg = 25f;
    public float range = 20f;
    public float vertRange = 20f;
    public float fireRate = 1f;
    public float gunShotRadius = 20f;
    public int piercing = 1;
    public bool reloadB = true;
    public int maxAmmo = 5;
    public int ammo;

    private EnemyManager enemyManager;
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    private float timeToFire;
    private BoxCollider gunTrigger;
    private int enemyLayer;
    private SceneSwitcher _sceneSwitcher;
    private PlayerHealth _playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        ammo = maxAmmo;
        //creates a trigger hit box(big box that will damage all enemies inside)
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, vertRange, range);
        gunTrigger.center = new Vector3(0, 0, range*0.5f);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        if (_sceneSwitcher.CheckCurrentScene("TheRoom"))
        {
            enemyManager = FindObjectOfType<EnemyManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerHealth.isPaused && !_playerHealth.seeking)
        {
            //check if can shoot(fire delay)
            if (Input.GetMouseButtonDown(0) && Time.time > timeToFire)
            {
                //REMOVE:Debug.Log("mouse down & timetofire");
                CheckAmmo();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (reloadB)
                {
                    //REMOVE:Debug.Log("trying reload");
                    Reload();
                }
            }
        }
    }

    void fire()
    {
        //REMOVE:Debug.Log("in fire");
        //alert enemies in gun shot radius
        Collider[] enemyColliders;
        enemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
        //REMOVE:Debug.Log("found enemies");
        foreach (var enemyCollider in enemyColliders)
        {
            //REMOVE:Debug.Log("alerting enemy x");
            enemyCollider.GetComponent<EnemyAwareness>().isAggro = true;
        }
        //REMOVE:Debug.Log("playing audio");

        //play shoot audio
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
        //REMOVE:Debug.Log("starting hitTotal");
        //checks for each enemy in the trigger hit box
        Dictionary<int, Enemy> enemyDict = new Dictionary<int, Enemy>();//creates an array to store enemies that will be hit
        //REMOVE:Debug.Log("dict created");
        //adds enemies that will be hit to a dictionary (equal to the piercing value-1 or equal to the amount of enemies in enemiesInTrigger [whichever is less])
        for (int j = 0; (j < piercing) && (j < enemyManager.enemiesInTrigger.Count); j++){
            //REMOVE:Debug.Log("j is "+j);
            Vector3 dir = Vector3.zero;
            int enemyIndex = -1;
            //finds closest enemy in enemiesInTrigger
            for(int i = 0; i < enemyManager.enemiesInTrigger.Count; i++){
                //REMOVE:Debug.Log("i is "+i);
                if(!enemyDict.ContainsKey(i)){
                    //REMOVE:Debug.Log("doesnt contain key");
                    Enemy enemy = enemyManager.enemiesInTrigger[i];
                    Vector3 dirTemp = enemy.transform.position - transform.position;
                    if (i == 0){
                        //REMOVE:Debug.Log("i was 0");
                        dir = dirTemp;
                        enemyIndex = i;
                        //REMOVE:Debug.Log("enemyIndex set: "+enemyIndex+" = "+i);
                    }
                    else if(dir.sqrMagnitude > dirTemp.sqrMagnitude){
                        //REMOVE:Debug.Log("dirTemp was smaller");
                        dir = dirTemp;
                        enemyIndex = i;
                        //REMOVE:Debug.Log("enemyIndex set: "+enemyIndex+" = "+i);
                    }
                }
            }
            //adds that enemy to dictionary
            if(enemyIndex != -1){
                enemyDict.Add(enemyIndex, enemyManager.enemiesInTrigger[enemyIndex]);
                //REMOVE:Debug.Log("added enemy #"+enemyIndex);
            }
            else{
                //REMOVE:Debug.Log("enemyIndex was -1");
            }
        }
        //REMOVE:Debug.Log("starting raycast");
        //try to raycast each enemy in the dictionary
        foreach (KeyValuePair<int, Enemy> entry in enemyDict)
        {
            var enemy = entry.Value;
            //REMOVE:Debug.Log("hitting enemy x");
            var dir = enemy.transform.position - transform.position;
            //checks for line of sight with enemies
            //REMOVE:Debug.Log("raycasting");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, range, raycastLayerMask))//can multiply range to reach corners but will overextend
            {
                //REMOVE:Debug.Log("raycast hit"+hit.collider);
                //REMOVE:Debug.Log(hit.transform.gameObject.layer);
                //REMOVE:Debug.Log(enemyLayer);
                if(hit.transform.gameObject.layer == enemyLayer)//try this or (hit.transform == enemy.transform)
                {
                    //REMOVE:Debug.Log("hit enemy");
                    //applies different damage amounts base off of half range
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);
                    if(dist > range * 0.5f)
                    {
                        //REMOVE:Debug.Log("small damage");
                        enemy.TakeDamage(smallDmg, false);
                    }
                    else
                    {
                        //REMOVE:Debug.Log("big gamage");
                        enemy.TakeDamage(bigDmg, false);
                    }
                    
                    
                }
            }

        }
        
        timeToFire = Time.time + fireRate;
    }

    //reloads players gun to the highest possible ammount <= maxAmmo
    public void Reload(){
        //checks if gun is at maxAmmo
        //REMOVE:Debug.Log("line 104");
        if(ammo < maxAmmo){
            //REMOVE:Debug.Log("line 106");
            //reloads as much ammo as possible into the gun
            for(int i = (maxAmmo-ammo); i >= 0; i--){
                //REMOVE:Debug.Log("checking "+i+" remSpores");
                if(GetComponentInParent<PlayerHealth>().RemSpores(i)){
                    ammo += i;
                    //REMOVE:Debug.Log("reloaded "+i+" ammo");
                    return;
                }
            }
        }
        else{
            //REMOVE:Debug.Log("max ammo");
        }
    }

    //checks if the gun can shoot and if true calls fire
    private void CheckAmmo(){
        //REMOVE:Debug.Log("checking ammo");
        //checks if gun is using ammo or spores
        if(reloadB){
            //checks if player has enough ammo to shoot
            if(ammo > 0){
                ammo -= 1;
                fire();
            }
        }
        else{
            //checks if player has enough spores to shoot
            //REMOVE:Debug.Log("cehcking spores");
            if(GetComponentInParent<PlayerHealth>().RemSpores(1)){
                //REMOVE:Debug.Log("calling fire");
                fire();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //add potential enemy to list(when looking at enemy)
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if(enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //remove enemy from list(when not looking at enemy)
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if(enemy)
        {
            //REMOVE:Debug.Log("enemy exit: "+enemy);
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
