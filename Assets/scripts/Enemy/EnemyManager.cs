using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public List<Enemy> enemiesInTrigger = new List<Enemy>();//enemies in gun hitbox
    public List<Enemy> enemiesAlive = new List<Enemy>();//enemies currently in world
    private WaveTracker waveTracker;
    private List<EnemySpawn> activeSpawners = new List<EnemySpawn>();
    
    private void Start() {
        waveTracker = GetComponent<WaveTracker>();
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
            waveTracker.EndWave();
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

    public void CheckActive()
    {
        foreach (var enemy in enemiesInTrigger)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveEnemy(enemy);
                Destroy(enemy);
            }
        }
    }
}
