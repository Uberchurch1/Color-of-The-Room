using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public List<Enemy> enemiesInTrigger = new List<Enemy>();//enemies in gun hitbox
    public List<Enemy> enemiesAlive = new List<Enemy>();//enemies currently in world
    private List<Enemy> enemiesInMelee = new List<Enemy>();//enemies in melee hitbox
    private WaveTracker waveTracker;
    private List<EnemySpawn> activeSpawners = new List<EnemySpawn>();
    public HudManager hudMan;
    private PlayerHealth player;
    
    private void Start() {
        waveTracker = GetComponent<WaveTracker>();
        player = FindObjectOfType<PlayerHealth>();
        StartCoroutine(CheckActiveCoroutine());
    }

    private void Update() 
    {
        
    }

    //ends wave if there are no enemies alive and no active spawners
    public void CheckEndWave()
    {
        Debug.Log("checking end wave");//REMOVE:
        if(enemiesAlive.Count == 0 && activeSpawners.Count == 0){
            Debug.Log("end wave = true");//REMOVE:
            
            waveTracker.waveOngoing = false;
            Debug.Log("ongoing = false");//REMOVE:
            StartCoroutine(waveTracker.EndWave());
        }
    }

    //add/remove enemies from gun hitbox
    public void AddEnemy(Enemy enemy)
    {
        enemiesInTrigger.Add(enemy);
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        enemiesInTrigger.Remove(enemy);
    }
    
    //add/remove enemies from melee hitbox
    public void AddMeleeEnemy(Enemy enemy)
    {
        enemiesInMelee.Add(enemy);
        hudMan.ShowGrab();
    }
    
    public void RemoveMeleeEnemy(Enemy enemy)
    {
        enemiesInMelee.Remove(enemy);
        hudMan.HideGrab();
    }
    
    public int EnemyMeleeCount()
    {
        return enemiesInMelee.Count;
    }

    public void MeleeDamge(float damage)
    {
        hudMan.GrabTrigger();
        foreach (var enemy in enemiesInMelee)
        {
            enemy.TakeDamage(damage, true);
            player.GiveHealth(1);
        }
    }

    //add/remove enemies from world list
    public void AddLiveEnemy(Enemy enemy)
    {
        enemiesAlive.Add(enemy);
    }
    
    public void RemoveLiveEnemy(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
    }

    //add/remove enemy spawners
    public void AddSpawner(EnemySpawn spawner)
    {
        activeSpawners.Add(spawner);
    }
    
    public void RemoveSpawner(EnemySpawn spawner)
    {
        activeSpawners.Remove(spawner);
    }

    private IEnumerator CheckActiveCoroutine()
    {
        while (true)
        {
            enemiesInTrigger.RemoveAll(enemy => enemy == null);
            

            enemiesInMelee.RemoveAll(enemy => enemy == null);
            

            enemiesAlive.RemoveAll(enemy => enemy == null);
            

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyInactive()
    {
        foreach (var enemy in enemiesInTrigger)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveEnemy(enemy);
                Destroy(enemy);
            }
        }
        foreach (var enemy in enemiesInMelee)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveMeleeEnemy(enemy);
                Destroy(enemy);
            }
        }
        foreach (var enemy in enemiesAlive)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveLiveEnemy(enemy);
                Destroy(enemy);
            }
        }
    }

}
