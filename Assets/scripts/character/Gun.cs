using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float smallDmg = 10f;
    public float bigDmg = 30f;
    public float range = 20f;
    public float vertRange = 20f;
    public float fireRate = 1f;
    public float gunShotRadius = 20f;
    public int piercing = 1;
    public bool reloadB = true;
    public int maxAmmo = 5;
    public int ammo;

    public EnemyManager enemyManager;
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    private float timeToFire;
    private BoxCollider gunTrigger;
    private int enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        ammo = maxAmmo;
        //creates a trigger hit box(big box that will damage all enemies inside)
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, vertRange, range);
        gunTrigger.center = new Vector3(0, 0, range*0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //check if can shoot(fire delay)
        if(Input.GetMouseButtonDown(0) && Time.time > timeToFire)
        {
            Debug.Log("mouse down & timetofire");//REMOVE:
            CheckAmmo();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            if(reloadB){
                Debug.Log("trying reload");//REMOVE:
                Reload();
            }
        }
    }

    void fire()
    {
        Debug.Log("in fire");//REMOVE:
        //alert enemies in gun shot radius
        Collider[] enemyColliders;
        enemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
        Debug.Log("found enemies");//REMOVE:
        foreach (var enemyCollider in enemyColliders)
        {
            Debug.Log("alerting enemy x");//REMOVE:
            enemyCollider.GetComponent<EnemyAwareness>().isAggro = true;
        }
        Debug.Log("playing audio");//REMOVE:

        //play shoot audio
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
        Debug.Log("starting hitTotal");//REMOVE:
        //checks for each enemy in the trigger hit box
        Dictionary<int, Enemy> enemyDict = new Dictionary<int, Enemy>();//creates an array to store enemies that will be hit
        Debug.Log("dict created");//REMOVE:
        //adds enemies that will be hit to a dictionary (equal to the piercing value-1 or equal to the amount of enemies in enemiesInTrigge [whichever is less])
        for (int j = 0; (j < piercing) && (j < enemyManager.enemiesInTrigger.Count); j++){
            Debug.Log("j is "+j);//REMOVE:
            Vector3 dir = Vector3.zero;
            int enemyIndex = -1;
            //finds closest enemy in enemiesInTrigger
            for(int i = 0; i < enemyManager.enemiesInTrigger.Count; i++){
                Debug.Log("i is "+i);//REMOVE:
                if(!enemyDict.ContainsKey(i)){
                    Debug.Log("doesnt contain key");//REMOVE:
                    Enemy enemy = enemyManager.enemiesInTrigger[i];
                    Vector3 dirTemp = enemy.transform.position - transform.position;
                    if (i == 0){
                        Debug.Log("i was 0");//REMOVE:
                        dir = dirTemp;
                        enemyIndex = i;
                        Debug.Log("enemyIndex set: "+enemyIndex+" = "+i);//REMOVE:
                    }
                    else if(dir.sqrMagnitude > dirTemp.sqrMagnitude){
                        Debug.Log("dirTemp was smaller");//REMOVE:
                        dir = dirTemp;
                        enemyIndex = i;
                        Debug.Log("enemyIndex set: "+enemyIndex+" = "+i);//REMOVE:
                    }
                }
            }
            //adds that enemy to dictionary
            if(enemyIndex != -1){
                enemyDict.Add(enemyIndex, enemyManager.enemiesInTrigger[enemyIndex]);
                Debug.Log("added enemy #"+enemyIndex);//REMOVE:
            }
            else{
                Debug.Log("enemyIndex was -1");//REMOVE:
            }
        }
        Debug.Log("starting raycast");//REMOVE:
        //try to raycast each enemy in the dictionary
        foreach (KeyValuePair<int, Enemy> entry in enemyDict)
        {
            var enemy = entry.Value;
            Debug.Log("hitting enemy x");//REMOVE:
            var dir = enemy.transform.position - transform.position;
            //checks for line of sight with enemies
            Debug.Log("raycasting");//REMOVE:
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, range, raycastLayerMask))//can multiply range to reach corners but will overextend
            {
                Debug.Log("raycast hit"+hit.collider);//REMOVE:
                Debug.Log(hit.transform.gameObject.layer);//REMOVE:
                Debug.Log(enemyLayer);//REMOVE:
                if(hit.transform.gameObject.layer == enemyLayer)//try this or (hit.transform == enemy.transform)
                {
                    Debug.Log("hit enemy");//REMOVE:
                    //applies different damage amounts base off of half range
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);
                    if(dist > range * 0.5f)
                    {
                        Debug.Log("small damage");//REMOVE:
                        enemy.TakeDamage(smallDmg);
                    }
                    else
                    {
                        Debug.Log("big gamage");//REMOVE:
                        enemy.TakeDamage(bigDmg);
                    }
                    
                    
                }
            }

        }
        
        timeToFire = Time.time + fireRate;
    }

    //reloads players gun to the highest possible ammount <= maxAmmo
    public void Reload(){
        //checks if gun is at maxAmmo
        Debug.Log("line 104");//REMOVE:
        if(ammo < maxAmmo){
            Debug.Log("line 106");//REMOVE:
            //reloads as much ammo as possible into the gun
            for(int i = (maxAmmo-ammo); i >= 0; i--){
                Debug.Log("checking "+i+" remSpores");//REMOVE:
                if(GetComponentInParent<PlayerHealth>().RemSpores(i)){
                    ammo += i;
                    Debug.Log("reloaded "+i+" ammo");//REMOVE:
                    return;
                }
            }
        }
        else{
            Debug.Log("max ammo");//REMOVE:
        }
    }

    //checks if the gun can shoot and if true calls fire
    private void CheckAmmo(){
        Debug.Log("checking ammo");//REMOVE:
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
            Debug.Log("cehcking spores");//REMOVE:
            if(GetComponentInParent<PlayerHealth>().RemSpores(1)){
                Debug.Log("calling fire");//REMOVE:
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
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
